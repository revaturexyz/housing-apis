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
        AmenityComplexId = amenityComplex.AmenityComnplexId,
        AmentityId = amenityComplex.AmenityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    public Entities.AmenityFloor MapAmenityFloortoE(AmenityFloorPlan amenityFloorPlan)
    {
      return new Entities.AmenityFloor
      {
        AmenityFloorPlanId = amenityFloorPlan.AmenityFloorPlanId,
        AmenityId = amenityFloorPlan.AmenityId,
        FloorPlanId = amenityFloorPlan.FloorPlanId
      };
    }

    public AmenityFloorPlan MapAmenityFloortoLib(Entities.AmenityFloorPlan amenityFloorPlan)
    {
      return new AmenityFloorPlan
      {
        AmenityFloorPlanId = amenityFloorPlan.AmenityFloorPlanId,
        AmenityId = amenityFloorPlan.AmenityId,
        FloorPlanId = amenityFloorPlan.FloorPlanId
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
        FloorPlanId = floorPlan.FloorPlanId,
        FloorPlanName = floorPlan.FloorPlanName,
        NumberOfBeds = floorPlan.NumberOfBeds,
        RoomTypeId = floorPlan.RoomTypeId,
        ComplexId = floorPlan.ComplexId
      };
    }

    public FloorPlan MapFloorPlantoLib(Entities.FloorPlan floorPlan)
    {
      return new FloorPlan
      {
        FloorPlanId = floorPlan.FloorPlanId,
        FloorPlanName = floorPlan.FloorPlanName,
        NumberOfBeds = floorPlan.NumberOfBeds,
        RoomTypeId = floorPlan.RoomTypeId,
        ComplexId = floorPlan.ComplexId
      };
    }

    public Entities.Gender MapGendertoE(Gender gender)
    {
      return new Entities.Gender
      {
        GenderId = gender.GenderId,
        GenderType = gender.GenderType
      };
    }

    public Gender MapGendertoLib(Entities.Gender gender)
    {
      return new Gender
      { 
        GenderId = gender.GenderId,
        GenderType = gender.GenderType
      };
    }

    public Entities.Room MapRoomtoE(Room room)
    {
      return new Entities.Room
      {
        RoomId = room.RoomId,
        RoomNumber = room.RoomNumber,
        NumberBeds = room.NumberBeds,
        NumberOccupants = room.NumberOccupants,
        GenderId = room.GenderId,
        LeaseStart = room.LeaseStart,
        LeaseEnd = room.LeaseEnd,
        RoomTypeId = room.RoomTypeId,
        ComplexId = room.ComplexId,
        FloorPlanId = room.FloorPlanId
      };
    }

    public Room MapRoomtoLib(Entities.Room room)
    {
      var tempRoom = new Room
      {
        RoomId = room.RoomId,
        RoomNumber = room.RoomNumber,
        NumberBeds = room.NumberBeds,
        NumberOccupants = room.NumberOccupants,
        GenderId = room.GenderId,
        RoomTypeId = room.RoomTypeId,
        ComplexId = room.ComplexId,
        FloorPlanId = room.FloorPlanId
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
