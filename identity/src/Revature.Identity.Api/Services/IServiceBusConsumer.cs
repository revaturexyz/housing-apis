using System.Threading.Tasks;

namespace Revature.Identity.Api.Services
{
  /// <summary>
  /// Interface for ServiceBusConsumer
  /// </summary>
  interface IServiceBusConsumer
  {
    void RegisterOnMessageHandlerAndReceiveMessages();

    Task CloseQueueAsync();
  }
}
