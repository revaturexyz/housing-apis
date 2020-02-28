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
    public Amenity Map(Lib.Models.Amenity amenity);
    public AmenityComplex Map(Lib.Models.AmenityComplex amenityComplex);
    public AmenityFloorPlan Map(Lib.Models.AmenityFloorPlan amenityFloorPlan);
    public AmenityRoom Map(Lib.Models.AmenityRoom amenityRoom);
    public Complex Map(Lib.Models.Complex complex);
    public FloorPlan Map(Lib.Models.FloorPlan floorPlan);
    public Gender Map(Lib.Models.Gender gender);
    public Room Map(Lib.Models.Room room);
    public RoomType Map(Lib.Models.RoomType roomType);

    //Maps Data Access to Library Models
    public Lib.Models.Amenity Map(Entities.Amenity amenity);
    public Lib.Models.AmenityComplex Map(Entities.AmenityComplex amenityComplex);
    public Lib.Models.AmenityFloorPlan Map(Entities.AmenityFloorPlan amenityFloorPlan);
    public Lib.Models.AmenityRoom Map(Entities.AmenityRoom amenityRoom);
    public Lib.Models.Complex Map(Entities.Complex complex);
    public Lib.Models.FloorPlan Map(Entities.FloorPlan floorPlan);
    public Lib.Models.Gender Map(Entities.Gender gender);
    public Lib.Models.Room Map(Entities.Room room);
    public Lib.Models.RoomType Map(Entities.RoomType roomType);
  }
}
