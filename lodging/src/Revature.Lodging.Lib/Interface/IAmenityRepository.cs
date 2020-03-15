using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Lib.Interface
{
  public interface IAmenityRepository
  {
    /// <summary>
    /// Create new single Amenity in database by logic amenity object.
    /// </summary>
    public Task<bool> CreateAmenityAsync(Logic.Amenity amenity);

    /// <summary>
    /// Read all existed amenities from the database.
    /// </summary>
    public Task<List<Logic.Amenity>> ReadAmenityListAsync();

    /// <summary>
    /// Read amenity list for specific complex from database by complex ID.
    /// </summary>
    public Task<List<Logic.Amenity>> ReadAmenityListByComplexIdAsync(Guid complexId);

    /// <summary>
    /// Read amenity list for specific room from database by room ID.
    /// </summary>
    public Task<List<Logic.Amenity>> ReadAmenityListByRoomIdAsync(Guid roomId);

    /// <summary>
    /// Update existed single amenity info in the database by logic amenity object.
    /// </summary>
    public Task<bool> UpdateAmenityAsync(Logic.Amenity amenity);

    /// <summary>
    /// Delete existed single amenity info in the database by logic amenity object.
    /// </summary>
    public Task<bool> DeleteAmenityAsync(Logic.Amenity amenity);

    /// <summary>
    /// Create new single Amenities of Room in database by logic amenitycomplex object.
    /// </summary>
    public Task<bool> CreateAmenityComplexAsync(Logic.ComplexAmenity ac);

    /// <summary>
    /// Delete all amenity records from Amenity of complex in database by complex ID.
    /// </summary>
    public Task<bool> DeleteAmenityComplexAsync(Guid complexId);

    /// <summary>
    /// Create new single Amenities of Room in database by amenityroom object.
    /// </summary>
    public Task<bool> CreateAmenityRoomAsync(Logic.RoomAmenity ar);

    /// <summary>
    /// Delete all amenity record from Amenity of room in database by room ID.
    /// </summary>
    public Task<bool> DeleteAmenityRoomAsync(Guid roomId);
  }
}
