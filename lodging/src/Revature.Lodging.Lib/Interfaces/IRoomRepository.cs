using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Revature.Lodging.Lib.Interfaces
{
  public interface IRoomRepository
  {
    /// <summary>
    ///   Gets all Room objects from database, given a Complex ID.
    /// </summary>
    /// <param name="complexId"> Used to specify Room objects </param>
    /// <returns> Collection of Room objects </returns>
    public Task<IEnumerable<Room>> GetRoomsByComplexIdAsync(int complexId);

    /// <summary>
    ///   Gets a single Room object from database, given a Room ID.
    /// </summary>
    /// <param name="roomId"> Used to specify Room object </param>
    /// <returns> A single Room object </returns>
    public Task<Room> GetRoomByIdAsync(int roomId);

    /// <summary>
    ///   Updates an existing Room object from database.
    /// </summary>
    /// <param name="room"> Used to indicate modified Room object </param>
    public void UpdateRoomAsync(Room room);

    /// <summary>
    ///   Deletes an existing Room object from database, given a Room ID.
    /// </summary>
    /// <param name="roomId"> Used to specify Room object </param>
    public void DeleteRoomByIdAsync(int roomId);

    /// <summary>
    ///   Adds a single Room object to database.
    /// </summary>
    /// <param name="room"> Used to indicate a new Room object </param>
    public void AddRoomAsync(Room room);
  }
}
