using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Revature.Lodging.Api.Models;

namespace Revature.Lodging.Api.Services
{
  public class AddressRequest : IAddressRequest
  {
    private readonly HttpClient _client;

    /// <summary>
    /// Set Json Serialization to Camel Case policy.
    /// </summary>
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressRequest"/> class.
    /// </summary>
    /// <param name="client">HTTP client.</param>
    /// <param name="addressConfiguration">Configuration set with base URI.</param>
    public AddressRequest(HttpClient client, IConfiguration addressConfiguration)
    {
      client.BaseAddress = new Uri(addressConfiguration.GetSection("AppServices")["Address"]);
      client.DefaultRequestHeaders.Add("Accept", "application/json");

      _client = client;
    }

    /// <summary>
    /// Gets the ID of an address in Address Service - if the address does not already exist, address service can use
    /// the address sent in the query string to Post a new address. The official Address entry will always accopany a success response.
    /// </summary>
    /// <param name="item">A model of an Address.</param>
    /// <returns>A model of the formal Address entry in Address Services Database, including it GUID.</returns>
    public async Task<ApiAddress> PostAddressAsync(ApiAddress item)
    {
      var queryString = "?"
        + "street=" + item.Street + "&"
        + "city=" + item.City + "&"
        + "state=" + item.State + "&"
        + "zipCode=" + item.ZipCode + "&"
        + "country=" + item.Country;

      using var response = await SendRequestAsync<ApiAddress>(HttpMethod.Get, "api/Address" + queryString);
      response.EnsureSuccessStatusCode();

      return await ReadResponseBodyAsync<ApiAddress>(response);
    }

    public async Task<ApiAddress> GetAddressAsync(Guid addressId)
    {
      var pathElement = addressId.ToString();

      using var response = await SendRequestAsync<ApiAddress>(HttpMethod.Get, "api/Address/" + pathElement);
      response.EnsureSuccessStatusCode();

      return await ReadResponseBodyAsync<ApiAddress>(response);
    }

    /// <summary>
    /// Private helper method for sending a HTTP request between services.
    /// </summary>
    /// <returns>The response to the HTTP request.</returns>
    private async Task<HttpResponseMessage> SendRequestAsync<T>(HttpMethod method, string uri, T body = null)
      where T : class
    {
      using var request = new HttpRequestMessage(method, uri);
      if (body is T)
      {
        var json = JsonSerializer.Serialize(body, _jsonOptions);
        var content = new StringContent(json, Encoding.Default, "application/json");
        request.Content = content;
      }

      return await _client.SendAsync(request);
    }

    /// <summary>
    /// A private helper method for interpreting a HTTP Response.
    /// </summary>
    /// <returns>A generic typed object that may be included in the body of a response.</returns>
    private async Task<T> ReadResponseBodyAsync<T>(HttpResponseMessage response)
    {
      using var stream = await response.Content.ReadAsStreamAsync();
      return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
    }
  }
}
