using System;
namespace Revature.Lodging.DataAccess.Entities
{
    public class FloorPlan
    {
        public Guid FloorPlanID { get; set; }

        public string FloorPlanName {get; set;}

        public int NumberBeds {get; set;}

        public int RoomTypeID {get; set;}

        public Guid ComplexID {get; set;}
    }
}
