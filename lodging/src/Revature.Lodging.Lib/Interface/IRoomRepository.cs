using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Lib.Interface
{
  /// <summary>
  /// Interface for the Repository.
  /// </summary>
  public interface IRoomRepository
  {
    /// <summary>
    /// Method that returns a list of rooms based on some parameters sent by the complex service.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Either ComplexId or RoomId is not in the DB.</exception>
    public Task<IEnumerable<Room>> GetFilteredRoomsAsync(
      Guid complexId,
      string roomNumber,
      int? numberOfBeds,
      string roomType,
      string gender,
      DateTime? endDate,
      Guid? roomId,
      bool? empty = null,
      bool? vacancy = null);

    /// <summary>
    /// Method that adds a Room to the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task CreateRoomAsync(Room myRoom);

    /// <summary>
    /// Method that gets a specific Room from the database.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when room isn't found.</exception>
    public Task<Room> ReadRoomAsync(Guid roomId);

    /// <summary>
    /// Method that updates the room's lease.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when room isn't found.</exception>
    /// <remarks>Update room functionality of complex service.</remarks>
    public Task UpdateRoomAsync(Room myRoom);

    /// <summary>
    /// Method that deletes a Room.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when room isn't found.</exception>
    public Task DeleteRoomAsync(Guid roomId);

    /// <summary>
    /// Method that saves any changes to the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SaveAsync();

    /// <summary>
    /// Method returns a list of Room ids of vacant rooms based on tenant gender and end date of their stay, for further filtering on the tenant service.
    /// </summary>
    public Task<IList<Tuple<Guid, int>>> GetVacantFilteredRoomsByGenderandEndDateAsync(string gender, DateTime endDate);

    /// <summary>
    /// Method that updates room occupants when an occupant is assigned a room.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when a room matching the roomId is not found, or the gender type isn't found. </exception>
    /// <remarks>Sets a room's gender when Gender is null, i.e. when the room was previously unoccupied.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task AddRoomOccupantsAsync(Guid roomId, string tenantGender);

    /// <summary>
    /// Method that updates occupants when an occupant vacates a room.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when room isn't found.</exception>
    /// <remarks>Reverts gender of room back to null if updated room is empty.</remarks>
    public Task SubtractRoomOccupantsAsync(Guid roomId);

    /// <summary>
    /// Deletes a complex and deletes all rooms that is connected to that complex.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when room to be deleted isn't found in DB.</exception>
    public Task<List<Guid>> DeleteComplexRoomAsync(Guid complexId);
  }
}
