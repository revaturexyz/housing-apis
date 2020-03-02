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
      return new Entities.Room
      {
        Id = room.Id,
        RoomNumber = room.RoomNumber,
        NumberOfBeds = room.NumberOfBeds,
        NumberOfOccupants = room.NumberOfOccupants,
        LeaseStart = room.LeaseStart,
        LeaseEnd = room.LeaseEnd,
        ComplexId = room.ComplexId,
        GenderId = room.GenderId,
        RoomTypeId = room.RoomTypeId
      }; 
    }

    /// <summary>
    /// Entities.Room => Lib.Models.Room
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public static Lib.Models.Room Map(Entities.Room room)
    {
      var rm = new Lib.Models.Room
      {
        Id = room.Id,
        RoomNumber = room.RoomNumber,
        NumberOfBeds = room.NumberOfBeds,
        NumberOfOccupants = room.NumberOfOccupants,
        ComplexId = room.ComplexId,
        GenderId = room.GenderId,
        RoomTypeId = room.RoomTypeId
      };
      rm.SetLease(room.LeaseStart, room.LeaseEnd);
      return rm;
    }
  }
}
