using System;
namespace Revature.Lodging.DataAccess.Entities
{
    public class AmenityFloorPlan
    {
        public Guid AmenityFloorPlanID { get; set; }

        public Guid AmenityID { get; set; }
    public Amenity Amenity { get; set; }

        public Guid FloorPlanID { get; set; }
    public FloorPlan FloorPlan { get; set; }
  }
}
