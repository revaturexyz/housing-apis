using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Tenant.Api.ServiceBus;
using Revature.Tenant.Lib.Interface;
using Revature.Tenant.Lib.Models;

namespace Revature.Tenant.Api.Controllers
{
  /// <summary>
  /// Controller for the functionality of assigning tenants to rooms.
  /// </summary>
  [Route("api/Tenant/")]
  [ApiController]
  [Authorize]
  public class TenantRoomController : ControllerBase
  {
    private readonly ITenantRoomRepository _repository;
    private readonly ILogger _logger;
    private readonly ITenantRepository _repo2;
    private readonly IRoomService _roomService;
    private readonly IServiceBusSender _serviceBusSender;

    public TenantRoomController(ITenantRoomRepository repository, ITenantRepository repo2, ILogger<TenantRoomController> logger, IRoomService roomService, IServiceBusSender serviceBusSender)
    {
      _repository = repository;
      _repo2 = repo2;
      _logger = logger;
      _roomService = roomService;
      _serviceBusSender = serviceBusSender;
    }

    /// <summary>
    /// Controller method that returns tenants that aren't assigned a room.
    /// </summary>
    [HttpGet]
    [Route("Unassigned")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Coordinator")]
    public async Task<IActionResult> GetTenantsNotAssignedRoom()
    {
      _logger.LogInformation("Getting roomless tenants...");
      var tenants = await _repository.GetRoomlessTenantsAsync();
      _logger.LogInformation("Successfully gathered roomless tenants...");

      return Ok(tenants);
    }

    /// <summary>
    /// Controller method for getting the tenant information of the occupants in vacant rooms.
    /// </summary>
    [HttpGet]
    [Route("Assign/AvailableRooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Coordinator")]
    public async Task<IActionResult> GetTenantsByRoomId([FromQuery] string gender, [FromQuery] DateTime endDate)
    {
      _logger.LogInformation("Requesting room id + total beds from Room Service...");
      try
      {
        var availableRooms = await _roomService.GetVacantRoomsAsync(gender, endDate);

        var roomsWithTenants = new List<RoomInfo>();

        _logger.LogInformation("Getting Tenants by Room Id...");

        foreach (var room in availableRooms)
        {
          roomsWithTenants.Add(
            new RoomInfo
            {
              RoomId = room.item1,
              NumberOfBeds = room.item2,
              Tenants = await _repository.GetTenantsByRoomIdAsync(room.item1)
            });
        }

        _logger.LogInformation("Success.");

        return Ok(roomsWithTenants);
      }
      catch (HttpRequestException ex)
      {
        _logger.LogError("Could not retrieve info from Room Service.", ex);
        return BadRequest();
      }
    }

    /// <summary>
    /// Controller method for assigning a tenant to a specific room.
    /// </summary>
    [HttpPut]
    [Route("Assign/{tenantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator")]
    public async Task<IActionResult> AssignTenantToRoom(Guid tenantId, [FromQuery] Guid roomId)
    {
      try
      {
        _logger.LogInformation("Assigning tenant to room");

        var tenant = await _repo2.GetByIdAsync(tenantId);
        tenant.RoomId = roomId;
        _repo2.Put(tenant);
        await _repo2.SaveAsync();

        var roomMessage = new RoomMessage()
        {
          Gender = tenant.Gender,
          RoomId = roomId,
          OperationType = 0
        };

        _logger.LogInformation("Alerting room service to assign tenant to room");
        await _serviceBusSender.SendRoomIdMessage(roomMessage);
        _logger.LogInformation("Success! Room service has been alerted");

        return NoContent();
      }
      catch (ArgumentNullException ex)
      {
        _logger.LogInformation("Tenant cannot be found", ex);
        return NotFound();
      }
    }
  }
}
