using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Lodging.Api.Models;
using Revature.Lodging.Lib.Interface;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Api.Controllers
{
  /// <summary>
  /// Controller for commmunicating with the complex service
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class RoomController : ControllerBase
  {
    private readonly IRoomRepository _repository;
    private readonly IAmenityRepository _amenityRepo;
    private readonly ILogger _logger;

    public RoomController(IRoomRepository repository, IAmenityRepository amenityRepo, ILogger<RoomController> logger)
    {
      _repository = repository;
      _amenityRepo = amenityRepo;
      _logger = logger;
    }

    /// <summary>
    /// GET:
    /// This controller method is to get rooms based on filters applied (roomNumber, numberOfBeds, etc)
    /// </summary>
    /// <param name="complexId"></param>
    /// <param name="roomNumber"></param>
    /// <param name="numberOfBeds"></param>
    /// <param name="roomType"></param>
    /// <param name="gender"></param>
    /// <param name="endDate"></param>
    /// <returns>IEnumerable of type (Room)</returns>
    [HttpGet("complexId/{complexId}")] // /complexId/{complexId}?roomNumber=a&numberOfBeds=b&roomType=c&gender=d&endDate=e&roomId=f
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFilteredRoomsAsync(
      Guid complexId,
      [FromQuery] string roomNumber,
      [FromQuery] int? numberOfBeds,
      [FromQuery] string roomType,
      [FromQuery] string gender,
      [FromQuery] DateTime? endDate,
      [FromQuery] Guid? roomId,
      [FromQuery] bool? vacancy,
      [FromQuery] bool? empty)
    {
      try
      {
        _logger.LogInformation("Getting filtered rooms...");

        var rooms = await _repository.GetFilteredRoomsAsync(
        complexId,
        roomNumber,
        numberOfBeds,
        roomType,
        gender,
        endDate,
        roomId,
        empty,
        vacancy);

        var apiRooms = new List<ApiRoom>();

        foreach(var room in rooms)
        {
          var apiRoom = new ApiRoom()
          {
            RoomId = room.Id,
            RoomNumber = room.RoomNumber,
            ComplexId = room.ComplexId,
            NumberOfBeds = room.NumberOfBeds,
            NumberOfOccupants = room.NumberOfOccupants,
            Gender = room.Gender,
            ApiRoomType = room.RoomType,
            LeaseStart = room.LeaseStart,
            LeaseEnd = room.LeaseEnd
          };
          var tempAmenities = await _amenityRepo.ReadAmenityListByRoomIdAsync(room.Id);
          if (tempAmenities != null)
          {
            foreach (Logic.Amenity a in tempAmenities)
            {
              apiRoom.Amenities.Add(new ApiAmenity()
              {
                AmenityId = a.Id,
                AmenityType = a.AmenityType,
                Description = a.Description
              });
            }
          }       
          apiRooms.Add(apiRoom);
        }

        _logger.LogInformation("Success.");

        return Ok(apiRooms);
      }
      catch (KeyNotFoundException ex)
      {
        _logger.LogError("Either complex Id or room Id was not in the DB", ex);
        return NotFound(ex);
      }
    }

    /// <summary>
    /// GET:
    /// Reads/gets a room based given a roomId
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns>Room</returns>
    [HttpGet("{roomId}", Name = "GetRoomById")]
    [ProducesResponseType(typeof(Lib.Models.Room), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiRoom>> GetRoomByIdAsync([FromRoute]Guid roomId)
    {
      try
      {
        _logger.LogInformation("Getting room ready...");

        var result = await _repository.ReadRoomAsync(roomId);
        var apiRoom = new ApiRoom
        {
          RoomId = result.Id,
          RoomNumber = result.RoomNumber,
          ComplexId = result.ComplexId,
          NumberOfBeds = result.NumberOfBeds,
          NumberOfOccupants = result.NumberOfOccupants,
          Gender = result.Gender,
          ApiRoomType = result.RoomType,
          LeaseStart = result.LeaseStart,
          LeaseEnd = result.LeaseEnd
        };

        var tempAmenities = await _amenityRepo.ReadAmenityListByRoomIdAsync(roomId);
        if (tempAmenities != null)
        {
          foreach (Logic.Amenity a in tempAmenities)
          {
            apiRoom.Amenities.Add(new ApiAmenity()
            {
              AmenityId = a.Id,
              AmenityType = a.AmenityType,
              Description = a.Description
            });
          }
        }
        _logger.LogInformation("Success");

        return Ok(apiRoom);
      }
      catch (Exception ex)
      {
        _logger.LogError("Error occurred.", ex);
        return BadRequest();
      }
    }

    /// <summary>
    /// POST:
    /// Creates a room based on the complex's needs
    /// </summary>
    /// <param name="room"></param>
    /// <returns>N/A</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Lib.Models.Room), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRoomAsync
      ([FromBody]ApiRoom room)
    {
      try
      {
        _logger.LogInformation("Adding a room");

        var createdRoom = new Lib.Models.Room
        {
          ComplexId = room.ComplexId,
          Id = Guid.NewGuid(),
          RoomNumber = room.RoomNumber,
          NumberOfBeds = room.NumberOfBeds,
          NumberOfOccupants = 0,
          //Gender = room.Gender,
          RoomType = room.ApiRoomType //(03/02/2020) should not update Gender and RoomType and adding new room to database?
        };
        createdRoom.SetLease(room.LeaseStart, room.LeaseEnd);

        await _repository.CreateRoomAsync(createdRoom);

        var existingAmenities = await _amenityRepo.ReadAmenityListAsync();

        foreach(var postedAmenity in room.Amenities)
        {
          Logic.RoomAmenity roomAmenity = new Logic.RoomAmenity();

          if(existingAmenities.Any(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower()))
          {
            var amenity = existingAmenities.FirstOrDefault(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower());

            roomAmenity.Id = Guid.NewGuid();
            roomAmenity.RoomId = createdRoom.Id;
            roomAmenity.AmenityId = amenity.Id;
          }
          else
          {
            Logic.Amenity amenity = new Logic.Amenity()
            {
              Id = Guid.NewGuid(),
              AmenityType = postedAmenity.AmenityType,
              Description = null
            };
            await _amenityRepo.CreateAmenityAsync(amenity);

            roomAmenity.Id = Guid.NewGuid();
            roomAmenity.RoomId = createdRoom.Id;
            roomAmenity.AmenityId = amenity.Id;
          }
          await _amenityRepo.CreateAmenityRoomAsync(roomAmenity);

        }

        await _repository.SaveAsync();

        _logger.LogInformation("Success. Room has been added");

        var test = "GetRoomById";

        return CreatedAtAction(
          actionName: test,
          routeValues: new { roomId = createdRoom.Id },
          value: createdRoom);
      }
      catch (ArgumentException ex)
      {
        _logger.LogInformation("Lease was invalid", ex);
        return BadRequest();
      }
    }

    /// <summary>
    /// PUT:
    /// Update a room's information
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="room"></param>
    /// <returns>No Content</returns>
    /// <remarks>Update room functionality of complex service</remarks>
    [HttpPut("{roomId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRoomAsync(Guid roomId,
      [FromBody]ApiRoom room)
    {
      try
      {
        _logger.LogInformation("Updating a room");

        //var roomFromDb = await _repository.ReadRoomAsync(roomId);
        //var amenitiesFromDb = await _amenityRepo.ReadAmenityListByRoomIdAsync(roomId);

        //roomFromDb.SetLease(room.LeaseStart, room.LeaseEnd);


        //var newRoom = new Logic.Room()
        //{
        //  Id = room.RoomId,
        //  RoomNumber = room.RoomNumber,
        //  ComplexId = room.ComplexId,
        //  NumberOfBeds = room.NumberOfBeds,
        //  NumberOfOccupants = room.NumberOfOccupants,
        //  Gender = room.Gender,
        //  RoomType = room.ApiRoomType
        //};

        var newRoom = new Logic.Room();

        newRoom.Id = room.RoomId;
        newRoom.RoomNumber = room.RoomNumber;
        newRoom.ComplexId = room.ComplexId;
        newRoom.NumberOfBeds = room.NumberOfBeds;
        newRoom.NumberOfOccupants = room.NumberOfOccupants;
        newRoom.Gender = room.Gender;
        newRoom.RoomType = room.ApiRoomType;

        newRoom.SetLease(room.LeaseStart, room.LeaseEnd);

        await _amenityRepo.DeleteAmenityRoomAsync(roomId);
        _logger.LogInformation($"(API)old amenities for room id: {room.RoomId} is deleted.");


        await _repository.UpdateRoomAsync(newRoom);
        var existingAmenities = await _amenityRepo.ReadAmenityListAsync();

        if (room.Amenities == null)
        {
          room.Amenities = new List<ApiAmenity>();
        }

        foreach (var postedAmenity in room.Amenities)
        {
          Logic.RoomAmenity roomAmenity = new Logic.RoomAmenity();

          if (existingAmenities.Any(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower()))
          {
            var amenity = existingAmenities.FirstOrDefault(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower());

            roomAmenity.Id = Guid.NewGuid();
            roomAmenity.RoomId = roomId;
            roomAmenity.AmenityId = amenity.Id;
          }
          else
          {
            Logic.Amenity amenity = new Logic.Amenity()
            {
              Id = Guid.NewGuid(),
              AmenityType = postedAmenity.AmenityType,
              Description = null
            };
            await _amenityRepo.CreateAmenityAsync(amenity);

            roomAmenity.Id = Guid.NewGuid();
            roomAmenity.RoomId = roomId;
            roomAmenity.AmenityId = amenity.Id;
          }

          await _amenityRepo.CreateAmenityRoomAsync(roomAmenity);
        }

        await _repository.SaveAsync();

        _logger.LogInformation("Success. Room has been updated");

        return StatusCode(200);
      }
      catch (InvalidOperationException ex)
      {
        _logger.LogError("Room to update was not found in DB", ex);
        return NotFound();
      }
      catch (ArgumentException ex)
      {
        _logger.LogError("Lease is invalid", ex);
        return BadRequest();
      }
    }

    /// <summary>
    /// DELETE:
    /// Delete room based on room Id
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns>No Content</returns>
    [HttpDelete("{roomId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoomAsync(Guid roomId)
    {
      try
      {
        _logger.LogInformation("Deleting room");

        await _repository.DeleteRoomAsync(roomId);
        await _repository.SaveAsync();

        _logger.LogInformation("Success. Room has been deleted");

        return NoContent();
      }
      catch (InvalidOperationException ex)
      {
        _logger.LogError("Room to delete was not found in DB", ex);
        return NotFound();
      }
    }
  }
}