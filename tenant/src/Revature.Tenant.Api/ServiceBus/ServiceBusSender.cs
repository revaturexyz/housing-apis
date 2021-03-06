using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Revature.Tenant.Lib.Models;

namespace Revature.Tenant.Api.ServiceBus
{
  /// <summary>
  /// The purpose of this class is to serialize and send a mesesage to the queue to be verified.
  /// </summary>
  public class ServiceBusSender : IServiceBusSender
  {
    private readonly QueueClient _queueClient;
    private readonly ILogger<ServiceBusSender> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBusSender"/> class injected with IConfiguration and ILogger.
    /// </summary>
    public ServiceBusSender(IConfiguration queueConfiguration, ILogger<ServiceBusSender> logger)
    {
      _logger = logger;
      _queueClient = new QueueClient(
        queueConfiguration.GetConnectionString("ServiceBus"),
        queueConfiguration.GetSection("Queues")["AssignedRoom"]);
    }

    /// <summary>
    /// ServiceBus message for sending a tenant room id.
    /// </summary>
    /// <param name="roomMessage">The details room service needs to update their rooms.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SendRoomIdMessage(RoomMessage roomMessage)
    {
      var data = JsonSerializer.Serialize(roomMessage);
      var message = new Message(Encoding.UTF8.GetBytes(data));

      _logger.LogInformation("Service Bus is sending message with room id, gender, and operation type", data);
      await _queueClient.SendAsync(message);
      _logger.LogInformation("Message sent!");
    }
  }
}
