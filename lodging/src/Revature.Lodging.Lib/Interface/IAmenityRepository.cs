using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Lib.Interface
{
  public interface IAmenityRepository
  {
    #region AmenityCrudOperations

    /// <summary>
    /// Create new single Amenity in database by logic amenity object
    /// </summary>
    /// <param name="amenity"></param>
    /// <returns></returns>
    public Task<bool> CreateAmenityAsync(Logic.Amenity amenity);

    /// <summary>
    /// Read all existed amenities from the database
    /// </summary>
    /// <returns></returns>
    public Task<List<Logic.Amenity>> ReadAmenityListAsync();

    /// <summary>
    /// Read amenity list for specific complex from database by complex Id
    /// </summary>
    /// <param name="complexId"></param>
    /// <returns></returns>
    public Task<List<Logic.Amenity>> ReadAmenityListByComplexIdAsync(Guid complexId);

    /// <summary>
    /// Read amenity list for specific room from database by room Id 
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public Task<List<Logic.Amenity>> ReadAmenityListByRoomIdAsync(Guid roomId);

    /// <summary>
    /// Update existed single amenity info in the database by logic amenity object
    /// </summary>
    /// <param name="amenity"></param>
    /// <returns></returns>
    public Task<bool> UpdateAmenityAsync(Logic.Amenity amenity);

    /// <summary>
    /// Delete existed single amenity info in the database by logic amenity object
    /// </summary>
    /// <param name="amenity"></param>
    /// <returns></returns>
    public Task<bool> DeleteAmenityAsync(Logic.Amenity amenity);

    #endregion

    #region AmenityComplexCrudOperations

    /// <summary>
    /// Create new single Amenities of Room in database by logic amenitycomplex object
    /// </summary>
    /// <param name="ac"></param>
    /// <returns></returns>
    public Task<bool> CreateAmenityComplexAsync(Logic.ComplexAmenity ac);

    /// <summary>
    /// Delete ALL amenity record from Amenity of complex in database by complex Id
    /// </summary>
    /// <param name="complexId"></param>
    /// <returns></returns>
    public Task<bool> DeleteAmenityComplexAsync(Guid complexId);

    #endregion

    #region AmenityRoomCrudOperations

    /// <summary>
    /// Create new single Amenities of Room in database by amenityroom object
    /// </summary>
    /// <param name="ar"></param>
    /// <returns></returns>
    public Task<bool> CreateAmenityRoomAsync(Logic.RoomAmenity ar);

    /// <summary>
    /// Delete ALL amenity record from Amenity of room in database by room Id
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public Task<bool> DeleteAmenityRoomAsync(Guid roomId);

    #endregion
  }
}
