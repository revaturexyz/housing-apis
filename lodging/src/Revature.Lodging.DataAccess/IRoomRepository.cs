using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Revature.Lodging.Lib;

namespace Revature.Lodging.DataAccess
{
  public interface IRoomRepository
  {
    IEnumerable<Lib.Models.Room> GetAllRooms();
    Task<IEnumerable<Lib.Models.Room>> GetRoomsByComplexId(Guid complexID);
    Task<Lib.Models.Room> GetRoomByID(Guid roomID);
    Task UpdateRoomAtNumber(Lib.Models.Room room, Guid roomID);
    Task DeleteRoomByNumber(Guid roomID);
    Task AddRoom(Lib.Models.Room room);
  }
}
