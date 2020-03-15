using System.Threading.Tasks;
using Revature.Tenant.Lib.Models;

namespace Revature.Tenant.Api.ServiceBus
{
  public interface IServiceBusSender
  {
    /// <summary>
    /// ServiceBus message for sending a tenant room id.
    /// </summary>
    /// <param name="roomMessage">The details room service needs to be able to update a room.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SendRoomIdMessage(RoomMessage roomMessage);
  }
}
