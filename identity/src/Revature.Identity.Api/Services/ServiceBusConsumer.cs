using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Revature.Identity.Lib.Interface;
using Revature.Identity.Lib.Model;
//using Microsoft.ServiceBus.Messaging;

namespace Revature.Identity.Api.Services
{
  /// <summary>
  /// This classes purpose is to connect to the queue and listen/receive a message sent from the tenant service.
  /// Based on their message we will call upon the repository accordingly
  /// This class will eventually be expanded to handle incoming notifications from the lodging and tenant services as well.
  /// </summary>
  public class ServiceBusConsumer : BackgroundService, IServiceBusConsumer
  {
    //_tenantQueue will be used for receiving from the Tenant service
    private readonly QueueClient _tenantQueue;
    private readonly IServiceProvider _services;
    private readonly ILogger<ServiceBusConsumer> _logger;

    /// <summary>
    /// Constructor injecting IConfiguration, IServiceProvider, and ILogger
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="services"></param>
    /// <param name="logger"></param>
    public ServiceBusConsumer(IConfiguration configuration, IServiceProvider services, ILogger<ServiceBusConsumer> logger)
    {
      //declare queues
      _tenantQueue = new QueueClient(configuration.GetConnectionString("ServiceBus"), configuration["Queues:TenantCUD"]);

      _services = services;
      _logger = logger;
    }

    /// <summary>
    /// Registers the message and then calls the process message
    /// </summary>
    public void RegisterOnMessageHandlerAndReceiveMessages()
    {
      var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
      {
        MaxConcurrentCalls = 2,
        AutoComplete = false
      };

      _tenantQueue.RegisterMessageHandler(ProcessTenantUpdateAsync, messageHandlerOptions);
    }
    /// <summary>
    /// Method used to process messages from tenant service
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="JsonException">Thrown when message isn't deserialized properly</exception>
    private async Task ProcessTenantUpdateAsync(Message message, CancellationToken token)
    {
      // Dispose of this scope after done using repository service
      //   Necessary due to singleton service (bus service) consuming a scoped service (repo)
      using var scope = _services.CreateScope();
      var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository>();
      try
      {
        _logger.LogInformation("Attempting to deserialize tenant message from service bus consumer", message.Body);
        // Expect to receive a tuple of <Guid, string> from the tenant service
        var receivedMessage = JsonSerializer.Deserialize<TenantMessage>(message.Body);

        //TenantMessage returns both a Tuple that has a type of <Guid, string> and an operation type that
        //states which operation to do, either adding or subtracting an occupant
        switch (receivedMessage.OperationType)
        {
          case OperationType.Create:
            _logger.LogInformation("Creating new tenant account", message.Body);
            TenantAccount tenant = new TenantAccount
            {
              Email = receivedMessage.Email,
              Name = receivedMessage.Name,
              TenantId = receivedMessage.TenantId
            };
            repo.AddTenantAccount(tenant);
            await repo.SaveAsync();
            break;

          case OperationType.Update:
            _logger.LogInformation("Updating a tenant account", message.Body);
            TenantAccount tenant1 = await repo.GetTenantAccountByIdAsync(receivedMessage.TenantId);
            tenant1.Email = receivedMessage.Email;
            tenant1.Name = receivedMessage.Name;
            await repo.UpdateTenantAccountAsync(tenant1);

            await repo.SaveAsync();
            break;
          case OperationType.Delete:
            _logger.LogInformation("Deleting a tenant account", message.Body);

            await repo.DeleteTenantAccountAsync(receivedMessage.TenantId);
            await repo.SaveAsync();

            break;
        }

      }
      catch (JsonException ex)
      {
        _logger.LogError("Message did not convert properly", ex);
      }
      finally
      {
        // Alert bus service that message was received
        await _tenantQueue.CompleteAsync(message.SystemProperties.LockToken);
      }
    }
    /// <summary>
    /// The exception handler for receiving a message.
    /// </summary>
    /// <param name="exceptionReceivedEventArgs"></param>
    /// <returns></returns>
    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      return Task.CompletedTask;
    }

    /// <summary>
    /// Closes the queue after receiving the message.
    /// </summary>
    /// <returns></returns>
    public async Task CloseQueueAsync()
    {
      await _tenantQueue.CloseAsync();
    }
    /// <summary>
    /// Inherited from the Background service, so far no use for it just yet
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">Inherited but not utilized</exception>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      RegisterOnMessageHandlerAndReceiveMessages();
      return Task.CompletedTask;
    }
  }
}
