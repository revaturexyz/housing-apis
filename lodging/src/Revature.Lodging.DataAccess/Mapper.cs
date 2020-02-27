using System;
using System.Collections.Generic;
using System.Text;
using Revature.Lodging.Lib.Models;

namespace Revature.Lodging.DataAccess
{
  public class Mapper : IMapper
  {
    public Entities.AmenityComplex MapAmenityComplextoE(AmenityComplex amenityComplex)
    {
      return new Entities.AmenityComplex
      {
        AmenityComplexId = amenityComplex.AmenityComplexId,
        AmenityId = amenityComplex.AmentityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    public AmenityComplex MapAmenityComplextoLib(Entities.AmenityComplex amenityComplex)
    {
      return new AmenityComplex
      {
        AmenityComplexId = amenityComplex.AmenityComplexId,
        AmentityId = amenityComplex.AmenityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    public Entities.AmenityFloorPlan MapAmenityFloortoE(AmenityFloorPlan amenityFloorPlan)
    {
      return new Entities.AmenityFloorPlan
      {
        AmenityFloorPlanID = amenityFloorPlan.AmenityFloorPlanId,
        AmenityID = amenityFloorPlan.AmenityId,
        FloorPlanID = amenityFloorPlan.FloorPlanId
      };
    }

    public AmenityFloorPlan MapAmenityFloortoLib(Entities.AmenityFloorPlan amenityFloorPlan)
    {
      return new AmenityFloorPlan
      {
        AmenityFloorPlanId = amenityFloorPlan.AmenityFloorPlanID,
        AmenityId = amenityFloorPlan.AmenityID,
        FloorPlanId = amenityFloorPlan.FloorPlanID
      };
    }

    public Entities.AmenityRoom MapAmenityRoomtoE(AmenityRoom amenityRoom)
    {
      return new Entities.AmenityRoom
      {
        AmenityRoomId = amenityRoom.AmenityRoomId,
        AmenityId = amenityRoom.AmenityId,
        RoomId = amenityRoom.RoomId
      };
    }

    public AmenityRoom MapAmenityRoomtoLib(Entities.AmenityRoom amenityRoom)
    {
      return new AmenityRoom
      {
        AmenityRoomId = amenityRoom.AmenityRoomId,
        AmenityId = amenityRoom.AmenityId,
        RoomId = amenityRoom.RoomId
      };
    }

    public Entities.Amenity MapAmenitytoE(Amenity amenity)
    {
      return new Entities.Amenity
      {
        AmenityId = amenity.AmenityId,
        AmenityType = amenity.AmenityType,
        Description = amenity.Description
      };
    }

    public Amenity MapAmenitytoLib(Entities.Amenity amenity)
    {
      return new Amenity
      {
        AmenityId = amenity.AmenityId,
        AmenityType = amenity.AmenityType,
        Description = amenity.Description
      };
    }

    public Entities.Complex MapComplextoE(Complex complex)
    {
      return new Entities.Complex
      {
        ComplexId = complex.ComplexId,
        AddressId = complex.AddressId,
        ProviderId = complex.ProviderId,
        ComplexName = complex.ComplexName,
        ContactNumber = complex.ComplexName
      };
    }

    public Complex MapComplextoLib(Entities.Complex complex)
    {
      return new Complex
      {
        ComplexId = complex.ComplexId,
        AddressId = complex.AddressId,
        ProviderId = complex.ProviderId,
        ComplexName = complex.ComplexName,
        ContactNumber = complex.ComplexName
      };
    }

    public Entities.FloorPlan MapFloorPlantoE(FloorPlan floorPlan)
    {
      return new Entities.FloorPlan
      {
        FloorPlanID= floorPlan.FloorPlanID,
        FloorPlanName = floorPlan.FloorPlanName,
        NumberBeds = floorPlan.NumberOfBeds,
        RoomTypeID = floorPlan.RoomTypeId,
        ComplexID = floorPlan.ComplexId
      };
    }

    public FloorPlan MapFloorPlantoLib(Entities.FloorPlan floorPlan)
    {
      return new FloorPlan
      {
        FloorPlanId = floorPlan.FloorPlanID,
        FloorPlanName = floorPlan.FloorPlanName,
        NumberOfBeds = floorPlan.NumberBeds,
        RoomTypeId = floorPlan.RoomTypeID,
        ComplexId = floorPlan.ComplexID
      };
    }

    public Entities.Gender MapGendertoE(Gender gender)
    {
      return new Entities.Gender
      {
        GenderId = gender.GenderId,
        Type = gender.GenderType
      };
    }

    public Gender MapGendertoLib(Entities.Gender gender)
    {
      return new Gender
      { 
        GenderId = gender.GenderId,
        GenderType = gender.Type
      };
    }

    public Entities.Room MapRoomtoE(Room room)
    {
      return new Entities.Room
      {
        RoomId = room.RoomId,
        RoomNumber = room.RoomNumber,
        NumberOfBeds = room.NumberBeds,
        NumberOfOccupants = room.NumberOccupants,
        Gender = room.GenderId,
        LeaseStart = room.LeaseStart,
        LeaseEnd = room.LeaseEnd,
        RoomType = room.RoomTypeId,
        ComplexId = room.ComplexId,
        FloorPlanID = room.FloorPlanId
      };
    }

    public Room MapRoomtoLib(Entities.Room room)
    {
      var tempRoom = new Room
      {
        RoomId = room.RoomId,
        RoomNumber = room.RoomNumber,
        NumberBeds = room.NumberOfBeds,
        NumberOccupants = room.NumberOfOccupants,
        GenderId = room.Gender,
        RoomTypeId = room.RoomType,
        ComplexId = room.ComplexId,
        FloorPlanId = room.FloorPlanID
      };
      tempRoom.SetLease(room.LeaseStart, room.LeaseEnd);
      return tempRoom;
    }

    public Entities.RoomType MapRoomTypetoE(RoomType roomType)
    {
      return new Entities.RoomType
      {
        RoomTypeId = roomType.RoomTypeId,
        Type = roomType.Type
      };
    }

    public RoomType MapRoomTypetoLib(Entities.RoomType roomType)
    {
      return new RoomType
      {
        RoomTypeId = roomType.RoomTypeId,
        Type = roomType.Type
      };
    }
  }
}
