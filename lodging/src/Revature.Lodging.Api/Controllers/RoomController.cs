using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Revature.Lodging.DataAccess;
using Revature.Lodging.Lib;

namespace Revature.Lodging.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RoomController : ControllerBase
  {
    private readonly IRoomRepository _repository;
    private readonly ILogger _logger;

    public RoomController(IRoomRepository repository, ILogger<RoomController> logger)
    {
      _repository = repository;
      _logger = logger;
    }

    /// <summary>
    /// GET Method that returns a list of all th rooms from a complex given complex ID
    /// </summary>
    /// <param name="complexID"></param>
    /// <returns>list of rooms and Ok status if successful or NotFound status if complex ID is not found</returns>
    [HttpGet("GetRoomByComplex/{complexID}", Name = "GetRoomByComplex")]
    public IActionResult GetRoomsByComplexID(Guid complexID)
    {
      _logger.LogInformation("Getting all rooms...");
      var rooms = _repository.GetAllRooms().ToList();

      if (rooms.Any(x => x.ComplexId == complexID))
      {
        _logger.LogInformation("Getting all rooms in the specified complex...");
        var roomsInComplex = _repository.GetRoomsByComplexId(complexID);

        _logger.LogInformation("Success");
        return Ok(roomsInComplex);
      }

      _logger.LogError("ComplexID not found");
      return NotFound();
    }


    /// <summary>
    /// GET method that returns a room given the roomID
    /// </summary>
    /// <param name="roomID"></param>
    /// <returns>room with Ok status if successful or NotFound status if roomID does not exist in the database</returns>
    [HttpGet("GetRoomByID/{roomID}", Name = "GetRoomByID")]
    public IActionResult GetRoomByID(Guid roomID)
    {
      _logger.LogInformation("Getting all rooms...");
      var rooms = _repository.GetAllRooms().ToList();

      if (rooms.Any(x => x.ComplexId == roomID))
      {
        _logger.LogInformation("Getting specified room...");
        var roomsInComplex = _repository.GetRoomByID(roomID);

        _logger.LogInformation("Success");
        return Ok(roomsInComplex);
      }

      _logger.LogError("roomID not found");
      return NotFound();
    }


    /// <summary>
    /// PUT method that updates a room given its roomID plus a room object from the body
    /// </summary>
    /// <param name="room"></param>
    /// <param name="roomID"></param>
    /// <returns>updated room from database with Ok status if successful, NotFound status if roomID does not exists in databse,
    ///           or BadRequest status if there is an database update exception</returns>
    [HttpPut("{roomID}")]
    public IActionResult UpdateRoomAtNumber([FromBody] Lib.Models.Room room, Guid roomID)
    {
      try
      {
        _logger.LogInformation("Getting all rooms...");
        var rooms = _repository.GetAllRooms().ToList();

        if (rooms.Any(x => x.RoomId == roomID))
        {
          _logger.LogInformation("Updating room....");
          _repository.UpdateRoomAtNumber(room, roomID);

          _logger.LogInformation("Retrieving the updated room information");
          var updatedRoom = _repository.GetRoomByID(roomID);

          _logger.LogInformation("Success");
          return Ok(updatedRoom);

        }

        _logger.LogError("roomID not found");
        return NotFound();
      }
      catch (DbUpdateException)
      {

        _logger.LogError("Could not update in the database");
        return BadRequest();
      }
      
    }

    /// <summary>
    /// DELETE method that removes a room from the database given the roomID
    /// </summary>
    /// <param name="roomID"></param>
    /// <returns>deleted room and OK status if successful, NotFoundCode if roomID deoes not exist in the database,
    ///         or BadRequest if there is any error deleting the room from the database</returns>
    [HttpDelete("{roomID}")]
    public IActionResult DeleteRoomByNumber(Guid roomID)
    {
      try
      {
        _logger.LogInformation("Getting all rooms...");
        var rooms = _repository.GetAllRooms().ToList();

        if (rooms.Any(x => x.RoomId == roomID))
        {
          _logger.LogInformation("Retrieving room to be deleted");
          var deletedRoom = _repository.GetRoomByID(roomID);

          _logger.LogInformation("Deleting room....");
          _repository.DeleteRoomByNumber(roomID);

          _logger.LogInformation("Success");
          return Ok(deletedRoom);

        }

        _logger.LogError("roomID not found");
        return NotFound();
      }
      catch (DbUpdateException)
      {

        _logger.LogError("Could not delete room in the database");
        return BadRequest();
      }
    }


    /// <summary>
    /// POST method that creates a room and stores it in the database
    /// </summary>
    /// <param name="room"></param>
    /// <returns>new room and CreatedAt (201) status if Successful or BadRequest status if there was an exception (ex. a constraint is violated)</returns>
    [HttpPost]
    public IActionResult AddRoom([FromBody] Lib.Models.Room room)
    {
      try
      {
        _logger.LogInformation("Adding room to the database...");
        _repository.AddRoom(room);

        _logger.LogInformation("Success");
        return CreatedAtRoute("GetRoomByID", new { RoomID = room.RoomId }, room);
      }
      catch (DbUpdateException)
      {
        _logger.LogError("Could not add to the database");
        return BadRequest();

      }
    }
  }
}
