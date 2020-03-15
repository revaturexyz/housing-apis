using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Revature.Lodging.Lib.Interface;

namespace Revature.Lodging.Api.Services
{
  /// <summary>
  /// This classes purpose is to connect to the queue and listen/receive a message sent from the tenant service.
  /// Based on their message we will call upon the repository accordingly.
  /// </summary>
  public class RoomServiceReceiver : BackgroundService, IRoomServiceReceiver
  {
    private readonly QueueClient _queueClient;
    private readonly IServiceProvider _services;
    private readonly ILogger<RoomServiceReceiver> _log;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomServiceReceiver"/> class.
    /// </summary>
    public RoomServiceReceiver(IConfiguration configuration, IServiceProvider services, ILogger<RoomServiceReceiver> logger)
    {
      _queueClient = new QueueClient(configuration.GetConnectionString("ServiceBus"), configuration["Queues:DeletedRooms"]);
      _services = services;
      _log = logger;
    }

    /// <summary>
    /// Registers the message and then calls the process message.
    /// </summary>
    public void RegisterOnMessageHandlerAndReceiveMessages()
    {
      var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
      {
        MaxConcurrentCalls = 1,
        AutoComplete = false
      };

      _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
    }

    /// <summary>
    /// Closes the queue after receiving the message.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task CloseQueueAsync()
    {
      await _queueClient.CloseAsync();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      RegisterOnMessageHandlerAndReceiveMessages();
      return Task.CompletedTask;
    }

    /// <summary>
    /// The actual method to process the received message.
    /// Receives and deserializes the message from room service.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
      // Dispose of this scope after done using repository service
      // Necessary due to singleton service (bus service) consuming a scoped service (repo)
      using var scope = _services.CreateScope();
      var repo = scope.ServiceProvider.GetRequiredService</*IComplex*/IAmenityRepository>();
      try
      {
        _log.LogInformation("Attempting to deserialize message from service bus consumer", message.Body);
        var listOfRoomId = JsonSerializer.Deserialize<Guid>(message.Body);

        _log.LogInformation("Attempting to DELETE rooms based on COMPLEX ID", listOfRoomId);

        // foreach (Guid roomId in listOfRoomId)
        // {
        //  await _repo.DeleteAmenityRoomAsync(roomId);
        //  log.LogInformation("Amenity of Room Id: {RoomId} is deleted", roomId);
        // }
        await repo.DeleteAmenityRoomAsync(listOfRoomId);
        _log.LogInformation("Amenity of Room Id: {RoomId} is deleted", listOfRoomId);
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "Message did not convert properly");
      }
      finally
      {
        // Alert bus service that message was received
        await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
      }
    }

    /// <summary>
    /// The exception handler for receiving a message.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      _log.LogError(context.ToString());
      return Task.CompletedTask;
    }
  }
}
