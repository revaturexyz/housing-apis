using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Revature.Lodging.Lib.Interfaces
{
  public interface IAmenityRepository
  {
    #region GET
    //Amenity
    /// <summary>
    /// get all amenities
    /// </summary>
    /// <returns> a clollection of all amenities </returns>
    public Task<IEnumerable<Lib.Models.Amenity>> GetAmneities();

    /// <summary>
    /// get an amenity given AmenityID
    /// </summary>
    /// <param name="amenityID"></param>
    /// <returns> a single amenity object </returns>
    public Task<Lib.Models.Amenity> GetAmenityById(Guid ID);
    //Room Amenity
    /// <summary>
    /// get an amenity room given id
    /// </summary>
    /// <returns> a single amenity room object </returns>
    public Task<Lib.Models.AmenityRoom> GetAmneityRoomById(Guid ID);

    /// <summary>
    /// get all amenities of a single room given room ID
    /// </summary>
    /// <param name="amenityRoomID"></param>
    /// <returns> a collection of amenities in a specified room </returns>
    public Task<IEnumerable<Lib.Models.AmenityRoom>> GetAmenityRoomById(Guid roomID);
    //Complex Amenity
    /// <summary>
    /// get a amenity complex given ID
    /// </summary>
    /// <returns> a single amenity complex object</returns>
    public Task<Lib.Models.AmenityComplex> GetAmneitiesComplex(Guid ID);

    /// <summary>
    /// get all amenities of a single complex given complex id
    /// </summary>
    /// <param name="amenityComplexID"></param>
    /// <returns> a collection of all amenities in a single complex </returns>
    public Task<IEnumerable<Lib.Models.AmenityComplex>> GetAmenityComplexById(Guid complexID);
    //Floor Plan Amenity
    /// <summary>
    /// get amenity floor object given id
    /// </summary>
    /// <returns> a single amenitiy floor plan object </returns>
    public Task<Lib.Models.AmenityFloorPlan> GetAmneitiesFloorPlan(Guid ID);

    /// <summary>
    /// get all amenities for a single floor plan given floor plan Id
    /// </summary>
    /// <param name="amenityFloorPlanID"></param>
    /// <returns> a collection of all amenities in a single floor plan </returns>
    public Task<Lib.Models.AmenityFloorPlan> GetAmenityFloorPlanById(Guid FloorPlanID);
    #endregion

    #region POST
    /// <summary>
    /// Add an amenity to the amenity table
    /// </summary>
    /// <param name="amenity"></param>
    /// <returns>  </returns>
    public Task AddAmenity(Lib.Models.Amenity amenity);

    /// <summary>
    /// Add an amenity room to the amenity room table
    /// </summary>
    /// <param name="amenityRoom"></param>
    /// <returns> </returns>
    public Task AddAmenityRoom(Lib.Models.AmenityRoom amenityRoom);

    /// <summary>
    /// Add an amenity Complex to the amenity complex table 
    /// </summary>
    /// <param name="amenityComplex"></param>
    /// <returns></returns>
    public Task AddAmenityComplex(Lib.Models.AmenityComplex amenityComplex);

    /// <summary>
    /// Add an amenity floor plan to the amenity floor plan table
    /// </summary>
    /// <param name="amenityFloorPlan"></param>
    /// <returns></returns>
    public Task AddAmenityFloorPlan(Lib.Models.AmenityFloorPlan amenityFloorPlan);
    #endregion

    #region PUT
    /// <summary>
    /// Update a amenity with given id
    /// </summary>
    /// <param name="amenity"></param>
    /// <param name="ID"></param>
    /// <returns> </returns>
    public Task UpdateAmenity(Lib.Models.Amenity amenity, Guid ID);

    /// <summary>
    /// Update an amenity room with given id
    /// </summary>
    /// <param name="amenityRoom"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task UpdateAmenityRoom(Lib.Models.AmenityRoom amenityRoom, Guid ID);

    /// <summary>
    /// Update an amenity complex with given id
    /// </summary>
    /// <param name="amenityComplex"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task UpdateAmenityComplex(Lib.Models.AmenityComplex amenityComplex, Guid ID);

    /// <summary>
    /// Update an amenity Floor Plan with given id
    /// </summary>
    /// <param name="amenityFloorPlan"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task UpdayeAmenityFloorPlan(Lib.Models.AmenityFloorPlan amenityFloorPlan, Guid ID);
    #endregion

    #region DELETE
    /// <summary>
    /// delete an amenity with given id
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task DeleteAmenity(Guid ID);

    /// <summary>
    /// delete an amenity room with given id
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task DeleteAmenityRoom(Guid ID);

    /// <summary>
    /// delete an amenity complex with given id
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task DeleteAmenityComplex(Guid ID);

    /// <summary>
    /// delete an amenity floor plan with given id
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Task DeleteAmenityFloorPlan(Guid ID);
    #endregion


  }
}
