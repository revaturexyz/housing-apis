using System.Threading.Tasks;

namespace Revature.Address.Lib.BusinessLogic
{
  /// <summary>
  /// Contains the logic for making calls to Google APIs.
  /// </summary>
  public class AddressLogic : IAddressLogic
  {
    private readonly IGoogleApiAccess _googleApiAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressLogic"/> class.
    /// </summary>
    public AddressLogic(IGoogleApiAccess googleApiAccess)
    {
      _googleApiAccess = googleApiAccess;
    }

    /// <summary>
    /// Makes a call to Google's Distance Matrix API to check if two given address
    /// are within a specified distance in miles of each other.
    /// </summary>
    /// <returns>Return true or false.</returns>
    public async Task<bool> IsInRangeAsync(Address origin, Address destination, int distance)
    {
      double actualDistance = await _googleApiAccess.GetDistanceAsync(origin, destination)
        .ConfigureAwait(false);

      var inRange = actualDistance <= distance;

      return inRange;
    }

    /// <summary>
    /// Makes a call to Google's Geocode API to check if a given address exists.
    /// </summary>
    /// <returns>Returns true or false.</returns>
    public async Task<bool> IsValidAddressAsync(Address address)
    {
      return await _googleApiAccess.IsValidAddressAsync(address).ConfigureAwait(false);
    }

    public async Task<Address> NormalizeAddressAsync(Address address)
    {
      var normalized = await _googleApiAccess.NormalizeAddressAsync(address).ConfigureAwait(false);
      normalized.Id = address.Id;
      return normalized;
    }
  }
}
