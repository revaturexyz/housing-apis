using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Revature.Lodging.Lib.Interface;
using Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Api.ServiceBus
{

  /// <summary>
  /// This class will have a queue listen for messages from the tenant service.
  /// Once received the message is deserialized and a specific action is performed.
  /// </summary>
  public class ServiceBusConsumer : BackgroundService, IServiceBusConsumer
  {
    //Creates a Queue, alongside the service provider and logger.
    private readonly QueueClient _occupancyUpdateQueue;
    private readonly IServiceProvider _services;
    private readonly ILogger<ServiceBusConsumer> _logger;

    /// <summary>
    /// Constructor injects IConfiguration, IServiceProvider, and ILogger.
    /// </summary>
    /// <returns></returns>
    public ServiceBusConsumer(IConfiguration configuration, IServiceProvider services, ILogger<ServiceBusConsumer> logger)
    {
      //This queue listens to the tenant service, and will complete actions related to the occupants per room.
      _occupancyUpdateQueue = new QueueClient(configuration.GetConnectionString("ServiceBus"), configuration["Queues:TQueue"]);
      _services = services;
      _logger = logger;
    }

    /// <summary>
    /// Registers the message and then calls the process message.
    /// </summary>
    public void RegisterOnMessageHandlerAndReceivedMessages()
    {
      var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
      {
        MaxConcurrentCalls = 2,
        AutoComplete = false
      };
      _occupancyUpdateQueue.RegisterMessageHandler(ProcessOccupancyUpdateAsync, messageHandlerOptions);
    }

    /// <summary>
    /// Attempts to deserialize the message provided by tenant. Upon successfully obtaining the TenantMessage object,
    /// a switch determines whether Tenant wants to create an occupant within a room, or remove an occupant from a room.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task ProcessOccupancyUpdateAsync(Message message, CancellationToken token)
    {
      //A repository is created temporarily in the method through the use of using.
      //The repository allows us to add/remove occupants from rooms within the database.
      using var scope = _services.CreateScope();
      var repo = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
      try
      {
        //The deserialization of the message should result in a tenant message. Tenant message contains
        //a roomId (The specific room we interact with), a gender (The gender of the new occupant), and
        //an operationType, which can be used to specify if we add an occupant or remove an occupant.
        _logger.LogInformation("Attempting to deserialize a tenant message from the service bus consumer");
        var receivedMessage = JsonSerializer.Deserialize<TenantMessage>(message.Body);
        switch(receivedMessage.OperationType)
        {
          //Adds an occupant to a specific room. If the room gender is null, the room gender is assigned to the gender provided by message.
          case OperationType.Create:
            _logger.LogInformation("Adding occupants to a room", message.Body);
            await repo.AddRoomOccupantsAsync(receivedMessage.RoomId, receivedMessage.Gender);
            break;

          //Removes an occupant from a specific room. If the number of occupants equals 0, the gender is removed from the room.
          case OperationType.Delete:
            _logger.LogInformation("Subtracting an occupant from a room", message.Body);
            await repo.SubtractRoomOccupantsAsync(receivedMessage.RoomId);
            break;            
        }
      }
      //If the message could not be handled, we catch a JsonException here.
      catch (JsonException ex)
      {
        _logger.LogError("Message did not convert properly", ex);
      }
      //Alerts the bus service that the message was received.
      finally
      {
        await _occupancyUpdateQueue.CompleteAsync(message.SystemProperties.LockToken);
      }
    }

    /// <summary>
    /// Handles an exception for recieving a message.
    /// </summary>
    /// <param name="exceptionReceivedEventArgs"></param>
    /// <returns></returns>
    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      return Task.CompletedTask;
    }

    /// <summary>
    /// Closes the queue after receiving a message..
    /// </summary>
    public async Task CloseQueueAsync()
    {
      await _occupancyUpdateQueue.CloseAsync();
    }

    /// <summary>
    /// Inherited from the Background Service, but is currently unused.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      RegisterOnMessageHandlerAndReceivedMessages();
      return Task.CompletedTask;
    }
  }
}
