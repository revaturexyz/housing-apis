using Revature.Lodging.Lib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Revature.Lodging.DataAccess.Entities;
using System.Linq;

namespace Revature.Lodging.DataAccess
{
  class RoomRepository : IRoomRepository
  {
    private readonly LodgingDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Adds a new room to the database
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public async Task AddRoom(Lib.Models.Room room)
    {
      var roomEntity =_mapper.MapRoomtoE(room);
      await _context.AddAsync(roomEntity);
      await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a room from the database given its roomID
    /// </summary>
    /// <param name="roomID"></param>
    /// <returns></returns>
    public async Task DeleteRoomByNumber(Guid roomID)
    {
      var roomEntity = await _context.Room.FindAsync(roomID);
      _context.Remove(roomEntity);
      await _context.SaveChangesAsync();
    }

    //helper function that returns all rooms in the database
    public IEnumerable<Lib.Models.Room> GetAllRooms()
    {
      var query = from e in _context.Room
                  select e;

      return query.Select(x => _mapper.MapRoomtoLib(x));
    }

    /// <summary>
    /// Returns a room given its RoomID
    /// </summary>
    /// <param name="roomID"></param>
    /// <returns></returns>
    public async Task<Lib.Models.Room> GetRoomByID(Guid roomID)
    {
      var roomEntity = await _context.Room.FindAsync(roomID);
      return _mapper.MapRoomtoLib(roomEntity);

    }

    /// <summary>
    /// Get all rooms from a complex given complexID
    /// </summary>
    /// <param name="complexID"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Lib.Models.Room>> GetRoomsByComplexId(Guid complexID)
    {
      IEnumerable<Entities.Room> complexRoomsEntity = _context.Room.Where(x => x.ComplexId == complexID).ToList();
    
      return complexRoomsEntity.Select(x => _mapper.MapRoomtoLib(x));
    }

    /// <summary>
    /// Updates a room passing the details you want updated
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public async Task UpdateRoomAtNumber(Lib.Models.Room room, Guid roomID)
    {
      var roomEntity = await _context.Room.FindAsync(roomID);

      roomEntity.RoomNumber = (room.RoomNumber != null) ? room.RoomNumber : roomEntity.RoomNumber;
      roomEntity.NumberOfBeds = (room.NumberBeds > 0) ? room.NumberBeds : roomEntity.NumberOfBeds;
      roomEntity.NumberOfOccupants = (room.NumberOccupants > 0 || room.NumberOccupants <= room.NumberBeds) ? room.NumberBeds : roomEntity.NumberOfOccupants;
      SetLease(ref roomEntity, room);

      _context.Room.Update(roomEntity);
      await _context.SaveChangesAsync();
    }

    //method that compared lease start and end dates to make sure they maske sense
    private void SetLease(ref Entities.Room roomEntity, Lib.Models.Room room)
    {
      if (room.LeaseStart.CompareTo(room.LeaseEnd) < 0)
      {
        roomEntity.LeaseStart = room.LeaseStart;
        roomEntity.LeaseEnd = room.LeaseEnd;
      }

    }
  }
}
