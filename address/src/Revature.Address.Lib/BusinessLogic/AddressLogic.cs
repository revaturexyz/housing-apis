using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleApi;
using GoogleApi.Entities.Maps.Geocoding;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Revature.Address.Lib.Models.DistanceMatrix;

namespace Revature.Address.Lib.BusinessLogic
{
  /// <summary>
  /// Contains the logic for making calls to Google Api's
  /// </summary>
  public class AddressLogic : IAddressLogic
  {
    private readonly ILogger _logger;
    private readonly string _key;
    private readonly IGoogleApiAccess _googleApiAccess;

    // Configures JsonSerializer with a snake case naming policy
    private readonly JsonSerializerOptions _distanceMatrixSerializerOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy()
    };

    /// <summary>
    /// Constructor for AddressLogic object
    /// </summary>
    /// <param name="logger"></param>
    public AddressLogic(IGoogleApiAccess googleApiAccess)
    {
      _googleApiAccess = googleApiAccess;
    }

    /// <summary>
    /// Makes a call to Google's Distance Matrix API to check if two given address
    /// are within a specified distance in miles of each other.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <param name="distance"></param>
    /// <returns>Return true or false</returns>
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
    /// Makes a call to Google's Geocode API to check if a given address exists
    /// </summary>
    /// <param name="address"></param>
    /// <returns>Returns true or false</returns>
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
    /// and API key and sets the units for response data to imperial
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <returns>Returns the url query string </returns>
    public string GetGoogleApiUrl(string origin, string destination)
    {
      // Google distance matrix parameters
      return $"?units=imperial&origins={origin}&destinations={destination}&key=";
    }

    /// <summary>
    /// Replaces white space with '+'s in Address information for proper url integration
    /// </summary>
    /// <param name="address"></param>
    /// <returns>Returns properly formatted address string</returns>
    public string FormatAddress(Address address)
    {
      // formats address for Google API
      return address.Street.Replace(" ", "+") + "+" + address.City.Replace(" ", "+") + "," + address.State.Replace(" ", "+") + "+" + address.ZipCode;
    }
  }
}
