using System.Threading.Tasks;

namespace Revature.Lodging.Api.ServiceBus
{
  /// <summary>
  /// Interface for the ServiceBusConsumer
  /// </summary>
  public interface IServiceBusConsumer
  {
    void RegisterOnMessageHandlerAndReceivedMessages();
    Task CloseQueueAsync();
  }
}
