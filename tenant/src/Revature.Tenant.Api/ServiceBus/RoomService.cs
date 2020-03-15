using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Revature.Tenant.Lib.Models;

namespace Revature.Tenant.Api.ServiceBus
{
  /// <summary>
  /// Service that communicates with the room service.
  /// </summary>
  public class RoomService : IRoomService
  {
    private readonly HttpClient _client;
    private readonly ILogger<RoomService> _logger;

    public RoomService(
      HttpClient client,
      IConfiguration configuration,
      IHttpContextAccessor httpContextAccessor,
      ILogger<RoomService> logger)
    {
      _logger = logger;

      client.BaseAddress = new Uri(configuration["AppServices:Lodging"]);

      var incomingHeaders = httpContextAccessor.HttpContext.Request.Headers;
      if (incomingHeaders.TryGetValue(HeaderNames.Authorization, out StringValues authValues))
      {
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, authValues as IEnumerable<string>);
        _logger.LogInformation("Configured HttpClient with Authorization header");
      }
      else
      {
        _logger.LogWarning("No Authorization header found to configure HttpClient with");
      }

      _client = client;
    }

    /// <summary>
    /// Method that gets vacant rooms from the room service.
    /// </summary>
    /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful.</exception>
    public async Task<List<AvailRoom>> GetVacantRoomsAsync(string gender, DateTime endDate)
    {
      var resourceURI = "api/rooms?gender=" + gender + "&endDate=" + endDate;
      _logger.LogInformation("Getting rooms from room service api");
      using var response = await _client.GetAsync(resourceURI);
      if (response.IsSuccessStatusCode)
      {
        _logger.LogInformation("Success! Room service responded with rooms");
        var contentAsString = await response.Content.ReadAsStringAsync();
        var availableRooms = JsonSerializer.Deserialize<List<AvailRoom>>(contentAsString);
        return availableRooms;
      }
      else
      {
        _logger.LogError("Unable to receive rooms from room service");
        _logger.LogError(response.StatusCode.ToString());
        _logger.LogError(await response.Content.ReadAsStringAsync());
        throw new HttpRequestException("Unable to receive response from room service");
      }
    }
  }
}
