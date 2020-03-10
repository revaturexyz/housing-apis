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

        var rooms = await _repository.GetFilteredRoomsAsync(  // Return rooms matching criteria
        complexId,
        roomNumber,
        numberOfBeds,
        roomType,
        gender,
        endDate,
        roomId,
        empty,
        vacancy);

        // Create a list of ApiRoom to be returned by the action
        var apiRooms = new List<ApiRoom>();

        // Mapping library models to API models to add to the ApiRoom list
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
            LeaseEnd = room.LeaseEnd,
            Amenities = (from amenity in await _amenityRepo.ReadAmenityListByRoomIdAsync(room.Id)
                         select new ApiAmenity()
                         {
                           AmenityId = amenity.Id,
                           AmenityType = amenity.AmenityType,
                           Description = amenity.Description
                         }).ToList()
          };

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

        var result = await _repository.ReadRoomAsync(roomId); //Return Room matching the incoming roomId
        
        // Map the library model to an API model
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
          LeaseEnd = result.LeaseEnd,
          Amenities = (from amenity in await _amenityRepo.ReadAmenityListByRoomIdAsync(roomId) select new ApiAmenity() {
            AmenityId = amenity.Id,
            AmenityType = amenity.AmenityType,
            Description = amenity.Description
          }).ToList()
        };

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

        await _repository.CreateRoomAsync(createdRoom);   // Create a new Room entry in the database

        // Read a list of all amenities in the database.
        var existingAmenities = await _amenityRepo.ReadAmenityListAsync();

        // Check whether the amenities in the posted room already exist or not. If not, add them to Amenities list.
        foreach(var postedAmenity in room.Amenities)
        {
          Logic.RoomAmenity roomAmenity = new Logic.RoomAmenity();

          // If the amenity already exists in the database, add it to the new room entity.
          if(existingAmenities.Any(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower()))
          {
            var amenity = existingAmenities.FirstOrDefault(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower());

            roomAmenity.Id = Guid.NewGuid();
            roomAmenity.RoomId = createdRoom.Id;
            roomAmenity.AmenityId = amenity.Id;
          }

          // If the amenity does not exist, create it and add it to the database.
          else
          {
            Logic.Amenity amenity = new Logic.Amenity()
            {
              Id = Guid.NewGuid(),
              AmenityType = postedAmenity.AmenityType,
              Description = null
            };
            // Post the amenity to the database.
            await _amenityRepo.CreateAmenityAsync(amenity);

            // Create a new entry in the RoomAmenity table.
            roomAmenity.Id = Guid.NewGuid();
            roomAmenity.RoomId = createdRoom.Id;
            roomAmenity.AmenityId = amenity.Id;
          }
          // Post a new entry for AmenityRoom in the database.
          await _amenityRepo.CreateAmenityRoomAsync(roomAmenity);

        }

        await _repository.SaveAsync();    // Save changes to the database.

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

        // Map incoming parameters to the specified Room entry in the database.
        Logic.Room newRoom = new Logic.Room()
        {
          Id = room.RoomId,
          RoomNumber = room.RoomNumber,
          ComplexId = room.ComplexId,
          NumberOfBeds = room.NumberOfBeds,
          NumberOfOccupants = room.NumberOfOccupants,
          Gender = room.Gender,
          RoomType = room.ApiRoomType
        };

        newRoom.SetLease(room.LeaseStart, room.LeaseEnd);   // Set lease for the room.

        // Delete amenities from the retrieved room for the purpose of replacing them easily.
        await _amenityRepo.DeleteAmenityRoomAsync(roomId);
        _logger.LogInformation($"(API)old amenities for room id: {room.RoomId} is deleted.");

        // Updates room to match input parameters.
        await _repository.UpdateRoomAsync(newRoom);
        // Read list of all amenities in the database.
        var existingAmenities = await _amenityRepo.ReadAmenityListAsync();

        // If the room has no amenities, assign an empty list to prevent errors.
        if (room.Amenities == null)
        {
          room.Amenities = new List<ApiAmenity>();
        }

        // Check each amenity in the updated room to ensure it exists. If not, create it.
        foreach (var postedAmenity in room.Amenities)
        {
          Logic.RoomAmenity roomAmenity = new Logic.RoomAmenity();

          // If the amenity exists in the database, create a new RoomAmenity entry.
          if (existingAmenities.Any(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower()))
          {
            var amenity = existingAmenities.FirstOrDefault(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower());

            roomAmenity.Id = Guid.NewGuid();
            roomAmenity.RoomId = roomId;
            roomAmenity.AmenityId = amenity.Id;
          }
          
          // Else, create the new Amenity entry, then create an AmenityRoom entry.
          else
          {
            Logic.Amenity amenity = new Logic.Amenity()
            {
              Id = Guid.NewGuid(),
              AmenityType = postedAmenity.AmenityType,
              Description = null
            };
            // Add the Amenity object to the database.
            await _amenityRepo.CreateAmenityAsync(amenity);

            roomAmenity.Id = Guid.NewGuid();
            roomAmenity.RoomId = roomId;
            roomAmenity.AmenityId = amenity.Id;
          }
          // Add the AmenityRoom object to the database.
          await _amenityRepo.CreateAmenityRoomAsync(roomAmenity);
        }
        // Save changes to the database.
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

        await _repository.DeleteRoomAsync(roomId);  // Deletes room by roomId
        await _repository.SaveAsync();              // Save changes to the database.

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
