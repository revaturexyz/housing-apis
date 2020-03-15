using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleApi;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Revature.Address.Lib.Models.DistanceMatrix;

namespace Revature.Address.Lib.BusinessLogic
{
  public class GoogleApiAccess : IGoogleApiAccess
  {
    private const string GoogleBaseUrl = "https://maps.googleapis.com/maps/api/distancematrix/json";

    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _key;

    // Configures JsonSerializer with a snake case naming policy
    private readonly JsonSerializerOptions _distanceMatrixSerializerOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy()
    };

    public GoogleApiAccess(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<GoogleApiAccess> logger = null)
    {
      _httpClient = httpClientFactory.CreateClient();

      _httpClient.BaseAddress = new Uri(GoogleBaseUrl);

      // Add an Accept header for JSON format.
      _httpClient.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

      _logger = logger;
      _key = configuration["GoogleApiKey"];
    }

    public async Task<double> GetDistance(Address origin, Address destination, int distance)
    {
      if (_key == null)
      {
        _logger.LogError("Google Cloud Platform API key has not been set");
      }

      // formatted address to be used with Google API
      var formattedOrigin = origin.Street.Replace(" ", "+") + "+" + origin.City.Replace(" ", "+") + "," + origin.State.Replace(" ", "+") + "+" + origin.ZipCode;
      var formattedDestination = destination.Street.Replace(" ", "+") + "+" + destination.City.Replace(" ", "+") + "," + destination.State.Replace(" ", "+") + "+" + destination.ZipCode;

      // added parameters to the Google API url
      var googleApiUrlParameter = $"?units=imperial&origins={formattedOrigin}&destinations={formattedDestination}&key=";

      Response deserialized;

      // await for async call and get response.
      using var response = await _httpClient.GetAsync(googleApiUrlParameter + _key).ConfigureAwait(false);
      if (response.IsSuccessStatusCode)
      {
        deserialized = JsonSerializer.Deserialize<Response>(
          await response.Content.ReadAsStringAsync().ConfigureAwait(false), _distanceMatrixSerializerOptions);

        // Parse the response body.
        var distanceValueString = deserialized.Rows[0].Elements[0].Distance.Text;

        // convert the response to a double
        var distanceValueDouble = double.Parse(distanceValueString[0..^2]);
        return distanceValueDouble;
      }
      else
      {
        _logger.LogWarning("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
        {
          throw new ArgumentException($"User input error {response.StatusCode}");
        }

        if ((int)response.StatusCode >= 500 && (int)response.StatusCode < 600)
        {
          throw new ArgumentException($"Google API returned internal server-side error {response.StatusCode}");
        }

        throw new ArgumentException($"Google API call was unsuccessful {response.StatusCode}");
      }
    }

    public async Task<bool> IsValidAddressAsync(Address address)
    {
      var request = new AddressGeocodeRequest
      {
        Address = $"{address.Street} {address.City}, {address.State} {address.ZipCode} {address.Country}",
        Key = _key
      };
      var response = await GoogleMaps.AddressGeocode.QueryAsync(request);
      var results = response.Results.ToArray();

      if (results.Length != 0)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public async Task<Address> NormalizeAddressAsync(Address address)
    {
      var request = new AddressGeocodeRequest
      {
        Address = $"{address.Street} {address.City}, {address.State} {address.ZipCode} {address.Country}",
        Key = _key
      };
      var response = await GoogleMaps.AddressGeocode.QueryAsync(request);
      var results = response.Results.ToList();

      var addressComponents = results[0].AddressComponents.ToList();
      var streetNum = addressComponents.First(t => t.Types.Contains(AddressComponentType.Street_Number));
      var street = addressComponents.First(t => t.Types.Contains(AddressComponentType.Route));
      var city = addressComponents.First(t => t.Types.Contains(AddressComponentType.Locality));
      var state = addressComponents.First(t => t.Types.Contains(AddressComponentType.Administrative_Area_Level_1));
      var country = addressComponents.First(t => t.Types.Contains(AddressComponentType.Country));
      var zipCode = addressComponents.First(t => t.Types.Contains(AddressComponentType.Postal_Code));

      var normalized = new Address
      {
        Street = $"{streetNum.LongName} {street.LongName}",
        City = city.LongName,
        State = state.LongName,
        Country = country.ShortName,
        ZipCode = zipCode.LongName
      };
      return normalized;
    }
  }
}
