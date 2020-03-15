using System.Threading.Tasks;
using Revature.Lodging.Api.Services;

namespace Revature.Lodging.Api
{
  public static class ApiMapper
  {
    /// <summary>
    /// Maps the Library Amentiy model to the Api Amenity model.
    /// </summary>
    /// <param name="amenity"></param>
    /// <returns></returns>
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
    /// <param name="amenity"></param>
    /// <returns></returns>
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
    /// Maps the Library Room model to the ApiRoomtoSend model.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public static Models.ApiRoomToSend Map(Lib.Models.Room room)
    {
      return new Models.ApiRoomToSend
      {
        RoomId = room.Id,
        RoomNumber = room.RoomNumber,
        ComplexId = room.ComplexId,
        NumberOfBeds = room.NumberOfBeds,
        RoomType = room.RoomType,
        LeaseStart = room.LeaseStart,
        LeaseEnd = room.LeaseEnd
      };
    }

    /// <summary>
    /// Maps the ApiRoom model to the Library Room model.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
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
    /// <param name="room"></param>
    /// <param name="isCoordinator"></param>
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
    /// <param name="complex"></param>
    /// <param name="_addressRequest"></param>
    /// <returns></returns>
    public static async Task<Models.ApiComplex> Map(Lib.Models.Complex complex, IAddressRequest _addressRequest)
    {
      return new Models.ApiComplex
      {
        ComplexId = complex.Id,
        ComplexName = complex.ComplexName,
        ContactNumber = complex.ContactNumber,
        ProviderId = complex.ProviderId,
        Address = await _addressRequest.GetAddressAsync(complex.AddressId)
      };
    }

    /// <summary>
    /// Maps the ApiComplex model to the Library Complex model.
    /// </summary>
    /// <param name="complex"></param>
    /// <returns></returns>
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
