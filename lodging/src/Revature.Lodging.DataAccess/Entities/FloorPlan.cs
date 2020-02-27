using System;
using System.Collections.Generic;

namespace Revature.Lodging.DataAccess.Entities
{
    public class FloorPlan
    {
        public Guid FloorPlanID { get; set; }

        public string FloorPlanName {get; set;}

        public int NumberBeds {get; set;}

        public int RoomTypeID {get; set;}

        public Guid ComplexID {get; set;}

    public ICollection<AmenityFloorPlan> AmenityFloorPlan { get; set; }
    public ICollection<Room> Room { get; set; }
    public RoomType RoomType { get; set; }
    public Complex Complex { get; set; }
  }
}
