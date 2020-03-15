namespace Revature.Lodging.DataAccess
{
  public static class Mapper
  {
    /// <summary>
    /// Lib.Models.Amenity => Entities.Amenities.
    /// </summary>
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
    /// Entities.Amenity => Lib.Models.Amenity.
    /// </summary>
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
    /// Lib.Models.AmenityComplex => Entities.AmenityComplex.
    /// </summary>
    public static Entities.ComplexAmenity Map(Lib.Models.ComplexAmenity amenityComplex)
    {
      return new Entities.ComplexAmenity
      {
        Id = amenityComplex.Id,
        AmenityId = amenityComplex.AmenityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    /// <summary>
    /// Entities.AmenityComplex => Lib.Models.AmenityComplex.
    /// </summary>
    public static Lib.Models.ComplexAmenity Map(Entities.ComplexAmenity amenityComplex)
    {
      return new Lib.Models.ComplexAmenity
      {
        Id = amenityComplex.Id,
        AmenityId = amenityComplex.AmenityId,
        ComplexId = amenityComplex.ComplexId
      };
    }

    /// <summary>
    /// Lib.Models.AmenityRoom => Entities.AmenityRoom.
    /// </summary>
    public static Entities.RoomAmenity Map(Lib.Models.RoomAmenity amenityRoom)
    {
      return new Entities.RoomAmenity
      {
        Id = amenityRoom.Id,
        AmenityId = amenityRoom.AmenityId,
        RoomId = amenityRoom.RoomId
      };
    }

    /// <summary>
    /// Entities.AmenityRoom => Lib.Models.AmenityRoom.
    /// </summary>
    public static Lib.Models.RoomAmenity Map(Entities.RoomAmenity amenityRoom)
    {
      return new Lib.Models.RoomAmenity
      {
        Id = amenityRoom.Id,
        AmenityId = amenityRoom.AmenityId,
        RoomId = amenityRoom.RoomId
      };
    }

    /// <summary>
    /// Lib.Models.Complex => Entities.Complex.
    /// </summary>
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
    /// Entities.Complex => Lib.Models.Complex.
    /// </summary>
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
    /// Lib.Models.Room => Entities.Room.
    /// </summary>
    public static Entities.Room Map(Lib.Models.Room room)
    {
      // switch statement to convert gender and room type strings into int that represent primary key from respective tables
      int? genderId = null;
      int roomTypeId = 0;
      string gender = room.Gender?.ToLower();
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
    /// Entities.Room => Lib.Models.Room.
    /// </summary>
    public static Lib.Models.Room Map(Entities.Room room)
    {
      // switch statement that converts gender id and room type id from room to strings for Lib.Models.Room
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
