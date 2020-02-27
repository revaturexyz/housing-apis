using System;
using System.Collections.Generic;
using System.Text;
using Revature.Lodging.Lib;
using Revature.Lodging.DataAccess.Entities;

namespace Revature.Lodging.DataAccess
{
  public interface IMapper
  {
    //Maps Library Models to Data Access Entities
    public Amenity MapAmenitytoE(Lib.Models.Amenity amenity);
    public AmenityComplex MapAmenityComplextoE(Lib.Models.AmenityComplex amenityComplex);
    public AmenityFloorPlan MapAmenityFloortoE(Lib.Models.AmenityFloorPlan amenityFloorPlan);
    public AmenityRoom MapAmenityRoomtoE(Lib.Models.AmenityRoom amenityRoom);
    public Complex MapComplextoE(Lib.Models.Complex complex);
    public FloorPlan MapFloorPlantoE(Lib.Models.FloorPlan floorPlan);
    public Gender MapGendertoE(Lib.Models.Gender gender);
    public Room MapRoomtoE(Lib.Models.Room room);
    public RoomType MapRoomTypetoE(Lib.Models.RoomType roomType);

    //Maps Data Access to Library Models
    public Lib.Models.Amenity MapAmenitytoLib(Entities.Amenity amenity);
    public Lib.Models.AmenityComplex MapAmenityComplextoLib(Entities.AmenityComplex amenityComplex);
    public Lib.Models.AmenityFloorPlan MapAmenityFloortoLib(Entities.AmenityFloorPlan amenityFloorPlan);
    public Lib.Models.AmenityRoom MapAmenityRoomtoLib(Entities.AmenityRoom amenityRoom);
    public Lib.Models.Complex MapComplextoLib(Entities.Complex complex);
    public Lib.Models.FloorPlan MapFloorPlantoLib(Entities.FloorPlan floorPlan);
    public Lib.Models.Gender MapGendertoLib(Entities.Gender gender);
    public Lib.Models.Room MapRoomtoLib(Entities.Room room);
    public Lib.Models.RoomType MapRoomTypetoLib(Entities.RoomType roomType);
  }
}
