using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Revature.Lodging.Lib.Interfaces
{
  public interface IFloorPlanRepository
  {
    /// <summary>
    ///   Gets all FloorPlan objects from database.
    /// </summary>
    /// <returns> Collection of FloorPlan objects </returns>
    public Task<IEnumerable<FloorPlan>> GetFloorPlansAsync();

    /// <summary>
    ///   Gets a single FloorPlan object from database, given a FloorPlan ID.
    /// </summary>
    /// <param name="floorPlanId"> Used to specify FloorPlan object </param>
    /// <returns> A single FloorPlan object </returns>
    public Task<FloorPlan> GetFloorPlanByIdAsync(long floorPlanId);

    /// <summary>
    ///   Adds a single FloorPlan object to database.
    /// </summary>
    /// <param name="floorPlan"> Used to indicate FloorPlan object </param>
    public void AddFloorPlanAsync(FloorPlan floorPlan);

    /// <summary>
    ///   Updates an existing FloorPlan object from database, given a FloorPlan ID.
    /// </summary>
    /// <param name="floorPlanId"> Used to specify Room object </param>
    public void UpdateFloorPlanByIdAsync(long floorPlanId);

    /// <summary>
    ///   Deletes an existing FloorPlan object from database, given a FloorPlan ID.
    /// </summary>
    /// <param name="floorPlanId"> Used to specify FloorPlan object </param>
    public void DeleteFloorPlanAsync(long floorPlanId);

    /// <summary>
    ///   Persists any changes made to DbContext to the database.
    /// </summary>
    public void SaveChanges();
  }
}
