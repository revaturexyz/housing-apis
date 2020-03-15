using System.Threading.Tasks;
using Revature.Lodging.Api.Services;

namespace Revature.Lodging.Api
{
  public static class ApiMapper
  {
    /// <summary>
    /// Maps the Library Amentiy model to the Api Amenity model.
    /// </summary>
    public static Models.ApiAmenity Map(Lib.Models.Amenity amenity)
    {
      return new Models.ApiAmenity
      {
        AmenityId = amenity.Id,
        AmenityType = amenity.AmenityType,
        Description = amenity.Description
      };
    }

    /// <summary>
    /// Maps the Api Amenity model to the Library Amenity model.
    /// </summary>
    public static Lib.Models.Amenity Map(Models.ApiAmenity amenity)
    {
      return new Lib.Models.Amenity
      {
        Id = amenity.AmenityId,
        AmenityType = amenity.AmenityType,
        Description = amenity.Description
      };
    }

    /// <summary>
    /// Maps the ApiRoom model to the Library Room model.
    /// </summary>
    public static Lib.Models.Room Map(Models.ApiRoom room)
    {
      var tempRoom = new Lib.Models.Room
      {
        Id = room.RoomId,
        RoomNumber = room.RoomNumber,
        ComplexId = room.ComplexId,
        NumberOfBeds = room.NumberOfBeds,
        RoomType = room.ApiRoomType
      };
      tempRoom.SetLease(room.LeaseStart, room.LeaseEnd);
      return tempRoom;
    }

    /// <summary>
    /// Filters and returns a given room based on the user's role.
    /// </summary>
    /// <returns>Filtered room.</returns>
    public static Lib.Models.Room FilterRoomByRole(Lib.Models.Room room, bool isCoordinator)
    {
      var filteredRoom = room;

      if (!isCoordinator)
      {
        filteredRoom.NumberOfOccupants = (room.NumberOfOccupants > 0) ? 1 : 0;
        filteredRoom.Gender = null;
      }

      return filteredRoom;
    }

    /// <summary>
    /// Maps the Library Complex model to the ApiComplex model.
    /// </summary>
    public static async Task<Models.ApiComplex> Map(Lib.Models.Complex complex, IAddressRequest addressRequest)
    {
      return new Models.ApiComplex
      {
        ComplexId = complex.Id,
        ComplexName = complex.ComplexName,
        ContactNumber = complex.ContactNumber,
        ProviderId = complex.ProviderId,
        Address = await addressRequest.GetAddressAsync(complex.AddressId)
      };
    }

    /// <summary>
    /// Maps the ApiComplex model to the Library Complex model.
    /// </summary>
    public static Lib.Models.Complex Map(Models.ApiComplex complex)
    {
      return new Lib.Models.Complex
      {
        Id = complex.ComplexId,
        ComplexName = complex.ComplexName,
        ContactNumber = complex.ContactNumber,
        ProviderId = complex.ProviderId,
        AddressId = complex.Address.Id
      };
    }
  }
}
