using Revature.Address.Api.Models;

namespace Revature.Address.Api
{
  /// <summary>
  /// Mapper class that maps the address API model to the address libary model, and vice-versa
  /// </summary>
  public static class Mapper
  {
    /// <summary>
    /// Maps an API model object to a library model object
    /// </summary>
    /// <param name="address">API model to be mapped</param>
    /// <returns>Library model mapped from API model</returns>
    public static Lib.Address Map(AddressModel address)
    {
      return new Lib.Address
      {
        Id = address.Id,
        Street = address.Street,
        City = address.City,
        State = address.State,
        Country = address.Country,
        ZipCode = address.ZipCode
      };
    }

    /// <summary>
    /// Maps a library model object to an API model object
    /// </summary>
    /// <param name="address">Library object to be mapped</param>
    /// <returns>API model mapped from library model</returns>
    public static AddressModel Map(Lib.Address address)
    {
      return new AddressModel
      {
        Id = address.Id,
        Street = address.Street,
        City = address.City,
        State = address.State,
        Country = address.Country,
        ZipCode = address.ZipCode
      };
    }
  }
}
