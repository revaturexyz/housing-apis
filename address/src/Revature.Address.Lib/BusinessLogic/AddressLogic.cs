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
      var distanceValueDouble = await _googleApiAccess.GetDistance(origin, destination, distance);
      if (distanceValueDouble <= distance)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Makes a call to Google's Geocode API to check if a given address exists.
    /// </summary>
    /// <returns>Returns true or false.</returns>
    public async Task<bool> IsValidAddressAsync(Address address)
    {
      return await _googleApiAccess.IsValidAddressAsync(address);
    }

    public async Task<Address> NormalizeAddressAsync(Address address)
    {
      var normalized = await _googleApiAccess.NormalizeAddressAsync(address);
      normalized.Id = address.Id;
      return normalized;
    }

    /// <summary>
    /// Builds query portion of the url for Distance Matrix API call with a given origin and destination
    /// and API key and sets the units for response data to imperial.
    /// </summary>
    /// <returns>Returns the url query string. </returns>
    public string GetGoogleApiUrl(string origin, string destination)
    {
      // Google distance matrix parameters
      return $"?units=imperial&origins={origin}&destinations={destination}&key=";
    }

    /// <summary>
    /// Replaces white space with '+'s in Address information for proper url integration.
    /// </summary>
    /// <returns>Returns properly formatted address string.</returns>
    public string FormatAddress(Address address)
    {
      // formats address for Google API
      return address.Street.Replace(" ", "+") + "+" + address.City.Replace(" ", "+") + "," + address.State.Replace(" ", "+") + "+" + address.ZipCode;
    }
  }
}
