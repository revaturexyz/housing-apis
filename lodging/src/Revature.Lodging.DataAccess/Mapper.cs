using System;

namespace Revature.Lodging.DataAccess
{
  public static class Mapper
  {
    /// <summary>
    /// Lib.Models.Amenity => Entities.Amenities
    /// </summary>
    /// <param name="amenity"></param>
    /// <returns></returns>
    public static Entities.Amenity Map(Lib.Models.Amenity amenity)
    {
      return new Entities.Amenity
      {
        Id = amenity.Id,
        AmenityType = amenity.AmenityType,
        Description = amenity.Description
      };
    }

    /// <summary>
    /// Entities.Amenity => Lib.Models.Amenity
    /// </summary>
    /// <param name="amenity"></param>
    /// <returns></returns>
    public static Lib.Models.Amenity Map(Entities.Amenity amenity)
    {
      return new Lib.Models.Amenity
      {
        Id = amenity.Id,
        AmenityType = amenity.AmenityType,
        Description = amenity.Description
      };
    }

    /// <summary>
    /// Lib.Models.AmenityComplex => Entities.AmenityComplex
    /// </summary>
    /// <param name="amenityComplex"></param>
    /// <returns></returns>
    public static Entities.AmenityComplex Map(Lib.Models.AmenityComplex amenityComplex)
    {
      return new Entities.AmenityComplex
      {
        Id = amenityComplex.Id,
        AmenityId = amenityComplex.AmenityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    /// <summary>
    /// Entities.AmenityComplex => Lib.Models.AmenityComplex
    /// </summary>
    /// <param name="amenityComplex"></param>
    /// <returns></returns>
    public static Lib.Models.AmenityComplex Map(Entities.AmenityComplex amenityComplex)
    {
      return new Lib.Models.AmenityComplex
      {
        Id = amenityComplex.Id,
        AmenityId = amenityComplex.AmenityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    /// <summary>
    /// Lib.Models.AmenityRoom => Entities.AmenityRoom
    /// </summary>
    /// <param name="amenityRoom"></param>
    /// <returns></returns>
    public static Entities.AmenityRoom Map(Lib.Models.AmenityRoom amenityRoom)
    {
      return new Entities.AmenityRoom
      {
        Id = amenityRoom.Id,
        AmenityId = amenityRoom.AmenityId,
        RoomId = amenityRoom.RoomId
      };
    }

    /// <summary>
    /// Entities.AmenityRoom => Lib.Models.AmenityRoom
    /// </summary>
    /// <param name="amenityRoom"></param>
    /// <returns></returns>
    public static Lib.Models.AmenityRoom Map(Entities.AmenityRoom amenityRoom)
    {
      return new Lib.Models.AmenityRoom
      {
        Id = amenityRoom.Id,
        AmenityId = amenityRoom.AmenityId,
        RoomId = amenityRoom.RoomId
      };
    }

    /// <summary>
    /// Lib.Models.Complex => Entities.Complex
    /// </summary>
    /// <param name="complex"></param>
    /// <returns></returns>
    public static Entities.Complex Map(Lib.Models.Complex complex)
    {
      return new Entities.Complex
      {
        Id = complex.Id,
        AddressId = complex.AddressId,
        ProviderId = complex.ProviderId,
        ComplexName = complex.ComplexName,
        ContactNumber = complex.ContactNumber
      };
    }

    /// <summary>
    /// Entities.Complex => Lib.Models.Complex
    /// </summary>
    /// <param name="complex"></param>
    /// <returns></returns>
    public static Lib.Models.Complex Map(Entities.Complex complex)
    {
      return new Lib.Models.Complex
      {
        Id = complex.Id,
        AddressId = complex.AddressId,
        ProviderId = complex.ProviderId,
        ComplexName = complex.ComplexName,
        ContactNumber = complex.ContactNumber
      };

    }

    /// <summary>
    /// Lib.Models.Room => Entities.Room
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public static Entities.Room Map(Lib.Models.Room room)
    {
      //switch statement to convert gender and room type strings into int that represent primary key from respective tables
      Nullable<int> genderId = null;
      int roomTypeId = 0;
      string gender = room.Gender.ToLower();
      string roomType = room.RoomType.ToLower();

      if (gender != null)
      {
        switch (gender)
        {
          case "male":
            genderId = 1;
            break;
          case "female":
            genderId = 2;
            break;
        }
      }

      
        switch (roomType)
        {
          case "apartment":
            roomTypeId = 1;
            break;
          case "dormitory":
            roomTypeId = 2;
            break;
          case "townhouse":
            roomTypeId = 3;
            break;
          case "hotel/motel":
            roomTypeId = 4;
            break;
        
        }

      return new Entities.Room
      {
        Id = room.Id,
        RoomNumber = room.RoomNumber,
        NumberOfBeds = room.NumberOfBeds,
        NumberOfOccupants = room.NumberOfOccupants,
        LeaseStart = room.LeaseStart,
        LeaseEnd = room.LeaseEnd,
        ComplexId = room.ComplexId,
        GenderId = genderId,
        RoomTypeId = roomTypeId
      };
      

    }

    /// <summary>
    /// Entities.Room => Lib.Models.Room
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public static Lib.Models.Room Map(Entities.Room room)
    {
      //switch statement that converts gender id and room type id from room to strings for Lib.Models.Room
      string gender = null;
      string roomType = null;

      if (room.GenderId != null)
      {
        switch (room.GenderId)
        {
          case 1:
            gender = "Male";
            break;
          case 2:
            gender = "Female";
            break;
        }
      }

      switch (room.RoomTypeId)
      {
        case 1:
          roomType = "Apartment";
          break;
        case 2:
          roomType = "Dormitory";
          break;
        case 3:
          roomType = "TownHouse";
          break;
        case 4:
          roomType = "Hotel/Motel";
          break;
      }

      var rm = new Lib.Models.Room
      {
        Id = room.Id,
        RoomNumber = room.RoomNumber,
        NumberOfBeds = room.NumberOfBeds,
        NumberOfOccupants = room.NumberOfOccupants,
        ComplexId = room.ComplexId,
        Gender = gender,
        RoomType = roomType
      };
      rm.SetLease(room.LeaseStart, room.LeaseEnd);
      return rm;
    }
  }
}
