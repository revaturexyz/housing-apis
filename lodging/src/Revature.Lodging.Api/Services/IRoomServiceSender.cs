using Revature.Lodging.Api.Models;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Services
{
  public interface IRoomServiceSender
  {
    /// <summary>
    /// ServiceBus message for sending a message about ApiRoomtoSend model
    /// </summary>
    /// <param name="roomToSend"></param>
    /// <returns></returns>
    public Task SendRoomsMessages(ApiRoomtoSend rooms);
  }
}
