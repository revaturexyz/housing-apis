using System;
using System.Collections.Generic;
using System.Text;
using Revature.Lodging.Lib.Models;

namespace Revature.Lodging.DataAccess
{
  public class Mapper : IMapper
  {
    public Entities.AmenityComplex Map(AmenityComplex amenityComplex)
    {
      return new Entities.AmenityComplex
      {
        AmenityComplexId = amenityComplex.AmenityComplexId,
        AmenityId = amenityComplex.AmenityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    public AmenityComplex Map(Entities.AmenityComplex amenityComplex)
    {
      return new AmenityComplex
      {
        AmenityComplexId = amenityComplex.AmenityComplexId,
        AmenityId = amenityComplex.AmenityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    public Entities.AmenityFloorPlan Map(AmenityFloorPlan amenityFloorPlan)
    {
      return new Entities.AmenityFloorPlan
      {
        AmenityFloorPlanID = amenityFloorPlan.AmenityFloorPlanId,
        AmenityID = amenityFloorPlan.AmenityId,
        FloorPlanID = amenityFloorPlan.FloorPlanId
      };
    }

    public AmenityFloorPlan Map(Entities.AmenityFloorPlan amenityFloorPlan)
    {
      return new AmenityFloorPlan
      {
        AmenityFloorPlanId = amenityFloorPlan.AmenityFloorPlanID,
        AmenityId = amenityFloorPlan.AmenityID,
        FloorPlanId = amenityFloorPlan.FloorPlanID
      };
    }

    public Entities.AmenityRoom Map(AmenityRoom amenityRoom)
    {
      return new Entities.AmenityRoom
      {
        AmenityRoomId = amenityRoom.AmenityRoomId,
        AmenityId = amenityRoom.AmenityId,
        RoomId = amenityRoom.RoomId
      };
    }

    public AmenityRoom Map(Entities.AmenityRoom amenityRoom)
    {
      return new AmenityRoom
      {
        AmenityRoomId = amenityRoom.AmenityRoomId,
        AmenityId = amenityRoom.AmenityId,
        RoomId = amenityRoom.RoomId
      };
    }

    public Entities.Amenity Map(Amenity amenity)
    {
      return new Entities.Amenity
      {
        AmenityId = amenity.AmenityId,
        AmenityType = amenity.AmenityType,
        Description = amenity.Description
      };
    }

    public Amenity Map(Entities.Amenity amenity)
    {
      return new Amenity
      {
        AmenityId = amenity.AmenityId,
        AmenityType = amenity.AmenityType,
        Description = amenity.Description
      };
    }

    public Entities.Complex Map(Complex complex)
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

    public Complex Map(Entities.Complex complex)
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

    public Entities.FloorPlan Map(FloorPlan floorPlan)
    {
      return new Entities.FloorPlan
      {
        FloorPlanID= floorPlan.FloorPlanId,
        FloorPlanName = floorPlan.FloorPlanName,
        NumberBeds = floorPlan.NumberOfBeds,
        RoomTypeID = floorPlan.RoomTypeId,
        ComplexID = floorPlan.ComplexId
      };
    }

    public FloorPlan Map(Entities.FloorPlan floorPlan)
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

    public Entities.Gender Map(Gender gender)
    {
      return new Entities.Gender
      {
        GenderID = gender.GenderId,
        Type = gender.Type
      };
    }

    public Gender Map(Entities.Gender gender)
    {
      return new Gender
      { 
        GenderId = gender.GenderID,
        Type = gender.Type
      };
    }

    public Entities.Room Map(Room room)
    {
      return new Entities.Room
      {
        RoomId = room.RoomId,
        RoomNumber = room.RoomNumber,
        NumberOfBeds = room.NumberBeds,
        NumberOfOccupants = room.NumberOccupants,
        Gender = Map(room.Gender),
        LeaseStart = room.LeaseStart,
        LeaseEnd = room.LeaseEnd,
        RoomType = Map(room.RoomTypeId),
        ComplexId = room.ComplexId,
        FloorPlanID = room.FloorPlanId
      };
    }

    public Room Map(Entities.Room room)
    {
      var tempRoom = new Room
      {
        RoomId = room.RoomId,
        RoomNumber = room.RoomNumber,
        NumberBeds = room.NumberOfBeds,
        NumberOccupants = room.NumberOfOccupants,
        Gender = Map(room.Gender),
        RoomTypeId = Map(room.RoomType),
        ComplexId = room.ComplexId,
        FloorPlanId = room.FloorPlanID
      };
      tempRoom.SetLease(room.LeaseStart, room.LeaseEnd);
      return tempRoom;
    }

    public Entities.RoomType Map(RoomType roomType)
    {
      return new Entities.RoomType
      {
        RoomTypeId = roomType.RoomTypeId,
        Type = roomType.Type
      };
    }

    public RoomType Map(Entities.RoomType roomType)
    {
      return new RoomType
      {
        RoomTypeId = roomType.RoomTypeId,
        Type = roomType.Type
      };
    }
  }
}
