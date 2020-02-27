using System;
using System.Collections.Generic;
using System.Text;
using Revature.Lodging.Lib;

namespace Revature.Lodging.DataAccess
{
  public interface IMapper
  {
    //Maps Library Models to Data Access Entities
    public Entities.Amenity MapAmenitytoE(Lib.Models.Amenity amenity);
    public Entities.AmenityComplex MapAmenityComplextoE(Lib.Models.AmenityComplex amenityComplex);
    public Entities.AmenityFloor MapAmenityFloortoE(Lib.Models.AmenityFloorPlan amenityFloorPlan);
    public Entities.AmenityRoom MapAmenityRoomtoE(Lib.Models.AmenityRoom amenityRoom);
    public Entities.Complex MapComplextoE(Lib.Models.Complex complex);
    public Entities.FloorPlan MapFloorPlantoE(Lib.Models.FloorPlan floorPlan);
    public Entities.Gender MapGendertoE(Lib.Models.Gender gender);
    public Entities.Room MapRoomtoE(Lib.Models.Room room);
    public Entities.RoomType MapRoomTypetoE(Lib.Models.RoomType roomType);

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
