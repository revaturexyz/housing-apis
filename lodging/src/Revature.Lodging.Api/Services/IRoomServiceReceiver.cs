using System.Threading.Tasks;

namespace Revature.Lodging.Api.Services
{
  public interface IRoomServiceReceiver
  {
    /// <summary>
    /// Registers the message and then calls the process message.
    /// </summary>
    public abstract void RegisterOnMessageHandlerAndReceiveMessages();

    /// <summary>
    /// Closes the queue after receiving the message.
    /// </summary>
    /// <returns></returns>
    public abstract Task CloseQueueAsync();
  }
}
