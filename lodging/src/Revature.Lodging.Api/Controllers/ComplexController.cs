using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Lodging.Api.Models;
using Revature.Lodging.Api.Services;
using Revature.Lodging.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class ComplexController : Controller
  {
    private readonly IComplexRepository _complexRepository;
    private readonly ILogger<ComplexController> _log;
    private readonly IAddressRequest _addressRequest;

    public ComplexController(IComplexRepository complexRepository, ILogger<ComplexController> logger, IAddressRequest ar)
    {
      _complexRepository = complexRepository ?? throw new ArgumentNullException(nameof(complexRepository), "Complex repo cannot be null");
      _log = logger;
      _addressRequest = ar;
    }

    #region GET

    /// <summary>
    /// (GET)
    /// Gets all existing Complex objects from database
    /// </summary>
    /// <returns> Collection of Complex objects with a list of associated amenities. </returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    //GET: api/complex/
    public async Task<ActionResult<IEnumerable<ApiComplex>>> GetAllComplexesAsync()
    {
      try
      {
        var complexes = await _complexRepository.ReadComplexListAsync();
        var apiComplexes = new List<ApiComplex>();

        //foreach Complex object, get Address object from Address service using AddressId
        //create ApiComplex object for each Complex we have, and add them to a list that will be returned
        foreach (var complex in complexes)
        {
          var addressId = complex.AddressId;
          var address = await _addressRequest.GetAddressAsync(addressId);

          var apiComplex = new ApiComplex
          {
            ComplexId = complex.Id,
            Address = address,
            ProviderId = complex.ProviderId,
            ComplexName = complex.ComplexName,
            ContactNumber = complex.ContactNumber,
            ComplexAmenities = await _complexRepository.ReadAmenityListByComplexIdAsync(complex.Id)
          };
          _log.LogInformation("A list of amenities for complex Id {com.ComplexId} were found!", complex.Id);
          apiComplexes.Add(apiComplex);
        }

        return Ok(apiComplexes);
      }
      catch (Exception ex)
      {
        _log.LogError("{ex}: Internal Server Error", ex);
        return StatusCode(500, ex.Message);
      }
    }

    /// <summary>
    /// (GET)
    /// Get an existing Complex object by a ComplexId from database
    /// </summary>
    /// <param name="complexId"> Specifies the Complex object </param>
    /// <returns> Complex object with associated amenities </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{complexId}")]
    //GET: api/complex/{complexId}
    public async Task<ActionResult<ApiComplex>> GetComplexByIdAsync([FromRoute]Guid complexId)
    {
      try
      {
        var complex = await _complexRepository.ReadComplexByIdAsync(complexId);
        _log.LogInformation("A complex with Id: {complexId} was found", complexId);

        var addressId = complex.AddressId;
        var address = await _addressRequest.GetAddressAsync(addressId);

        var apiComplex = new ApiComplex
        {
          ComplexId = complex.Id,
          Address = address,
          ProviderId = complex.ProviderId,
          ComplexName = complex.ComplexName,
          ContactNumber = complex.ContactNumber,
          ComplexAmenities = await _complexRepository.ReadAmenityListByComplexIdAsync(complex.Id)
        };
        _log.LogInformation("A list of amenities for complex Id {lcomplex.ComplexId} was found!", complex.Id);

        return Ok(apiComplex);
      }
      catch (Exception ex)
      {
        _log.LogError("{ex}: Internal Server Error", ex);
        return StatusCode(500, ex.Message);
      }
    }

    /// <summary>
    /// (GET)
    /// Call Repository and Address service to get specific complex info
    /// by complex name and phone number as parameters
    /// then return single Api Complex model
    /// </summary>
    /// <param name="complexName"></param>
    /// <param name="ComplexNumber"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{complexName}/{ComplexNumber}")]
    //GET: api/complex/{complexName/ComplexNumber}
    public async Task<ActionResult<ApiComplex>> GetComplexByNameAndNumberAsync([FromRoute]string complexName, string complexNumber)
    {
      try
      {
        var lcomplex = await _complexRepository.ReadComplexByNameAndNumberAsync(complexName, complexNumber);
        _log.LogInformation("a complex with name: {complexName} and phone: {ComplexNumber} was found", complexName, complexNumber);

        var aId = lcomplex.AddressId;
        var address = await _addressRequest.GetAddressAsync(aId);

        var apiComplex = new ApiComplex
        {
          ComplexId = lcomplex.Id,
          Address = address,
          ProviderId = lcomplex.ProviderId,
          ComplexName = lcomplex.ComplexName,
          ContactNumber = lcomplex.ContactNumber,
          ComplexAmenities = await _complexRepository.ReadAmenityListByComplexIdAsync(lcomplex.Id)
        };
        _log.LogInformation("a list of amenities for complex Id {lcomplex.ComplexId} were found!", lcomplex.Id);

        return Ok(apiComplex);
      }
      catch (Exception ex)
      {
        _log.LogError("{ex}: Internal Server Error", ex);
        return StatusCode(500, ex.Message);
      }
    }

    /// <summary>
    /// (GET)
    /// Get Complex objects given a ProviderId from database
    /// </summary>
    /// <param name="providerId"> Indicates needed Complex objects </param>
    /// <returns> Collection of Complex objects with associated amenities </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("providerId/{providerId}")]
    //GET: api/complex/providerId/{providerID}
    public async Task<ActionResult<IEnumerable<ApiComplex>>> GetComplexesByProviderId([FromRoute]Guid providerId)
    {
      try
      {
        var complexes = await _complexRepository.ReadComplexByProviderIdAsync(providerId);
        _log.LogInformation("A list of complexes for provider Id: {providerId} were found", providerId);

        var apiComplexes = new List<ApiComplex>();

        foreach (var complex in complexes)
        {
          var addressId = complex.AddressId;
          var address = await _addressRequest.GetAddressAsync(addressId);

          var apiComplex = new ApiComplex
          {
            ComplexId = complex.Id,
            Address = address,
            ProviderId = complex.ProviderId,
            ComplexName = complex.ComplexName,
            ContactNumber = complex.ContactNumber,
            ComplexAmenities = await _complexRepository.ReadAmenityListByComplexIdAsync(complex.Id)
          };
          _log.LogInformation("A list of amenities for complex Id {complex.ComplexId} was found!", complex.Id);

          apiComplexes.Add(apiComplex);
        }

        return Ok(apiComplexes);
      }
      catch (Exception ex)
      {
        _log.LogError("{ex}: Internal Server Error", ex);
        return StatusCode(500, ex.Message);
      }
    }
    #endregion

    #region POST

    /// <summary>
    /// (POST)
    /// Adds a new Complex object to the database;
    /// Sends Complex address to Address service;
    /// </summary>
    /// <param name="apiComplex"> Indicates the new Complex object to be added </param>
    /// <returns> Added Complex object </returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    //POST: api/complex/
    public async Task<ActionResult<ApiComplex>> PostComplexAsync([FromBody]ApiComplex apiComplex)
    {
      // Creates an Address object from Address properties in apiComplex object argument
      var complexAddress = new ApiAddress()
      {
        StreetAddress = apiComplex.Address.StreetAddress,
        City = apiComplex.Address.City,
        State = apiComplex.Address.State,
        ZipCode = apiComplex.Address.ZipCode,
        Country = apiComplex.Address.Country,
      };

      // Retrieves an Address object using AddressRequest and storing the AddressId
      var addressId = (await _addressRequest.PostAddressAsync(complexAddress)).AddressId;

      // Generates a new Guid for a ComplexId
      var complexId = Guid.NewGuid();

      // Creates a Complex object with the information in the apiComplex object argument
      var complex = new Logic.Complex()
      {
        Id = complexId,
        AddressId = addressId,
        ProviderId = apiComplex.ProviderId,
        ContactNumber = apiComplex.ContactNumber,
        ComplexName = apiComplex.ComplexName
      };

      // Instantiates a blank ComplexAmenity library model
      //var complexAmenity = new Logic.ComplexAmenity();

      try
      {
        // Adds created Complex object into the database
        await _complexRepository.CreateComplexAsync(complex);
        _log.LogInformation("(API)new complex in the database is inserted");

        // Gets all existing Amenity objects from database
        var existingAmenities = await _complexRepository.ReadAmenityListAsync();
        _log.LogInformation("(API)list of Amenity is found");

        // Sets the ComplexId property of ComplexAmenity object
        // complexAmenity.ComplexId = complex.Id;

        //foreach (var postedAmenity in apiComplex.ComplexAmenities)
        //{
        //  foreach (var existingAmenity in existingAmenities)
        //  {
        //    if (existingAmenity.AmenityType == postedAmenity.AmenityType)
        //    {
        //      complexAmenity.AmenityId = existingAmenity.Id;
        //      complexAmenity.Id = Guid.NewGuid();
        //    }
        //  }

        //  await _complexRepository.CreateAmenityComplexAsync(complexAmenity);
        //  _log.LogInformation($"(API)a list of amenities for complex id: {complex.Id} was created");
        //}

        // Create ComplexAmenity objects from the list of Amenities passed in with apiComplex
        foreach (var postedAmenity in apiComplex.ComplexAmenities)
        {
          // Instantiates a new ComplexAmenity object
          Logic.ComplexAmenity complexAmenity = new Logic.ComplexAmenity();

          // if there are any existing Amenity object with a matching AmenityType, link this existing Amenity object
          // with the new ComplexAmenity object; otherwise, create a new Amenity object with the posted AmenityType and
          // link this new Amenity object with the new ComplexAmenity object
          if (existingAmenities.Any(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower()))
          {
            var amenity = existingAmenities.FirstOrDefault(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower());

            complexAmenity.Id = Guid.NewGuid();
            complexAmenity.ComplexId = complexId;
            complexAmenity.AmenityId = amenity.Id;
          } else
          {
            Logic.Amenity amenity = new Logic.Amenity()
            {
              Id = Guid.NewGuid(),
              AmenityType = postedAmenity.AmenityType,
              Description = null
            };
            await _complexRepository.CreateAmenityAsync(amenity);

            complexAmenity.Id = Guid.NewGuid();
            complexAmenity.ComplexId = complexId;
            complexAmenity.AmenityId = amenity.Id;
          }

          await _complexRepository.CreateAmenityComplexAsync(complexAmenity);
        }

        return Created($"api/Complex/{complex.Id}", apiComplex);

      }
      catch (Exception ex)
      {
        _log.LogError($"(API){ex}: unable to create complex");
        return StatusCode(500, ex.Message);
      }
    }

    /// <summary>
    /// (POST)
    /// Call Repository to insert Amenity of rooms into the database
    /// Repackage the Rooms' object and send them to Room service
    /// Needs to take enumarable collections of Api Room model as parameters
    /// </summary>
    /// <param name="apiRooms"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("PostRooms")]
    //POST: api/complex/PostRooms
    public async Task<ActionResult> PostRoomsAsync([FromBody]IEnumerable<ApiRoom> apiRooms)
    {
      var apiRoomtoSends = new List<ApiRoomtoSend>();
      var amenityRoom = new Logic.RoomAmenity();

      try
      {
        foreach (var apiRoom in apiRooms)
        {
          var arts = new ApiRoomtoSend
          {
            RoomId = Guid.NewGuid(),
            RoomNumber = apiRoom.RoomNumber,
            ComplexId = apiRoom.ComplexId,
            NumberOfBeds = apiRoom.NumberOfBeds,
            RoomType = apiRoom.ApiRoomType,
            LeaseStart = apiRoom.LeaseStart,
            LeaseEnd = apiRoom.LeaseEnd,
            QueOperator = 0,
          };

          //await _roomServiceSender.SendRoomsMessages(arts);

          amenityRoom.Id = Guid.NewGuid();
          amenityRoom.RoomId = arts.RoomId;

          foreach (var amenity in apiRoom.Amenities)
          {
            amenityRoom.AmenityId = amenity.AmenityId;
            await _complexRepository.CreateAmenityRoomAsync(amenityRoom);
            _log.LogInformation("a list of amenities with room id: {0} was created", arts.RoomId);
          }
        }

        return StatusCode(201);
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "error while creating room");
        return StatusCode(500, ex.Message);
      }
    }

    #endregion

    #region PUT

    /// <summary>
    /// (PUT)
    /// Updates an existing Complex object in the database
    /// </summary>
    /// <param name="apiComplex"> Indicates the updated Complex object </param>
    /// <returns> Appropriate status code </returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut]
    //PUT: api/complex/
    public async Task<ActionResult> UpdateComplexAsync([FromBody]ApiComplex apiComplex)
    {
      var complex = new Logic.Complex()
      {
        Id = apiComplex.ComplexId,
        AddressId = apiComplex.Address.AddressId,
        ProviderId = apiComplex.ProviderId,
        ContactNumber = apiComplex.ContactNumber,
        ComplexName = apiComplex.ComplexName
      };

      await _complexRepository.DeleteAmenityComplexAsync(complex.Id);
      _log.LogInformation($"(API)old amenities for complex id: {complex.Id} is deleted");

      //var amenityComplex = new Logic.ComplexAmenity();

      try
      {
        await _complexRepository.UpdateComplexAsync(complex);
        _log.LogInformation("(API) complex is updated");

        var existingAmenities = await _complexRepository.ReadAmenityListAsync();
        _log.LogInformation("(API) list of amenity is read");

        //Guid amenityComplexId;
        //amenityComplex.ComplexId = complex.Id;

        //foreach (var amenity in apiComplex.ComplexAmenity)
        //{
        //  foreach (var am in amenities)
        //  {
        //    if (am.AmenityType == amenity.AmenityType)
        //    {
        //      amenityComplex.AmenityId = am.Id;

        //      amenityComplexId = Guid.NewGuid();
        //      amenityComplex.Id = amenityComplexId;
        //    }
        //  }

        //  await _complexRepository.CreateAmenityComplexAsync(amenityComplex);
        //  _log.LogInformation("(API)new list of amenity of complex is created");
        //}

        // Create ComplexAmenity objects from the list of Amenities passed in with apiComplex
        foreach (var postedAmenity in apiComplex.ComplexAmenities)
        {
          // Instantiates a new ComplexAmenity object
          Logic.ComplexAmenity complexAmenity = new Logic.ComplexAmenity();

          // if there are any existing Amenity object with a matching AmenityType, link this existing Amenity object
          // with the new ComplexAmenity object; otherwise, create a new Amenity object with the posted AmenityType and
          // link this new Amenity object with the new ComplexAmenity object
          if (existingAmenities.Any(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower()))
          {
            var amenity = existingAmenities.FirstOrDefault(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower());

            complexAmenity.Id = Guid.NewGuid();
            complexAmenity.ComplexId = complex.Id;
            complexAmenity.AmenityId = amenity.Id;
          }
          else
          {
            Logic.Amenity amenity = new Logic.Amenity()
            {
              Id = Guid.NewGuid(),
              AmenityType = postedAmenity.AmenityType,
              Description = null
            };
            await _complexRepository.CreateAmenityAsync(amenity);

            complexAmenity.Id = Guid.NewGuid();
            complexAmenity.ComplexId = complex.Id;
            complexAmenity.AmenityId = amenity.Id;
          }

          await _complexRepository.CreateAmenityComplexAsync(complexAmenity);
        }

        //send ApiComplexAddress to Address service to update the address

        return StatusCode(200);

      }
      catch (Exception ex)
      {
        _log.LogError($"(API){ex}: unable to update complex");
        return StatusCode(500, ex.Message);
      }
    }

    /// <summary>
    /// (PUT)
    /// Call Repo to delete and re-add new list of Amenity
    /// re-pack the Api Room model to Api RoomtoSend model
    /// send Api RoomtoSend object to Room service to delete single room
    /// </summary>
    /// <param name="apiRoom"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut("editroom")]
    //PUT: api/complex/editroom
    public async Task<ActionResult> PutRoomAsync([FromBody]ApiRoom apiRoom)
    {
      var arts = new ApiRoomtoSend();
      var amenityRoom = new Logic.RoomAmenity();

      try
      {
        arts.RoomId = apiRoom.RoomId;
        arts.RoomNumber = apiRoom.RoomNumber;
        arts.ComplexId = apiRoom.ComplexId;
        arts.NumberOfBeds = apiRoom.NumberOfBeds;
        arts.RoomType = apiRoom.ApiRoomType;
        arts.LeaseStart = apiRoom.LeaseStart;
        arts.LeaseEnd = apiRoom.LeaseEnd;
        arts.QueOperator = 2;

        amenityRoom.Id = Guid.NewGuid();
        amenityRoom.RoomId = arts.RoomId;

        await _complexRepository.DeleteAmenityRoomAsync(apiRoom.RoomId);
        _log.LogInformation(")Amenity of Room Id {apiRoom.RoomId} is deleted", apiRoom.RoomId);

        //await _roomServiceSender.SendRoomsMessages(arts);

        foreach (var amenity in apiRoom.Amenities)
        {
          amenityRoom.AmenityId = amenity.AmenityId;
          await _complexRepository.CreateAmenityRoomAsync(amenityRoom);
          _log.LogInformation("list of amenity with room id: {arts.RoomId} is created", arts.RoomId);
        }

        return StatusCode(200);
      }
      catch (Exception ex)
      {
        _log.LogError("{ex}: Internal Server Error", ex);
        return StatusCode(500, ex.Message);
      }
    }

    #endregion

    #region DELETE
    /// <summary>
    /// (DELETE)
    /// Deletes a Complex object by ComplexId from database
    /// </summary>
    /// <param name="complexId"> Indicates the Complex object to be deleted</param>
    /// <returns> Appropriate status code </returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{complexId}")]
    //DELETE: api/complex/{complexId}
    public async Task<ActionResult> DeleteComplexByIdAsync([FromRoute]Guid complexId)
    {
      try
      {
        //var arts = new ApiRoomtoSend
        //{
        //  ComplexId = complexId,
        //  QueOperator = 3
        //};

        //await _complexRepository.DeleteAmenityComplexAsync(complexId);
        //_log.LogInformation("deleted amenity of complex Id: {complexId}", complexId);


        await _complexRepository.DeleteComplexAsync(complexId);
        _log.LogInformation("deleted complex of complex Id: {complexId}", complexId);

        return StatusCode(200);
      }
      catch (Exception ex)
      {
        _log.LogError("{ex}: Internal Server Error", ex);
        return StatusCode(500, ex.Message);
      }
    }

    /// <summary>
    /// (DELETE)
    /// Call Repo to delete amenity of room in the database
    /// re-pack Api Room as Api RoomtoSend
    /// Send RoomtoSend to Room serivice to delete single room
    /// Needs Api Room object as parameter
    /// </summary>
    /// <param name="apiComplex"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("deleteroom")]
    //PUT: api/complex/deleteroom
    public async Task<ActionResult> DeleteRoomAsync([FromBody]ApiRoom room)
    {
      try
      {
        var roomtoDelete = new ApiRoomtoSend
        {
          RoomId = room.RoomId,
          RoomNumber = room.RoomNumber,
          ComplexId = room.ComplexId,
          NumberOfBeds = room.NumberOfBeds,
          RoomType = room.ApiRoomType,
          LeaseStart = room.LeaseStart,
          LeaseEnd = room.LeaseEnd,
          QueOperator = 1
        };

        //send {send} to room service to delete a room
        //await _roomServiceSender.SendRoomsMessages(roomtoDelete);

        await _complexRepository.DeleteAmenityRoomAsync(room.RoomId);
        _log.LogInformation("deleted amenity of room Id: {Room.RoomId}", room.RoomId);

        return StatusCode(200);
      }
      catch (Exception ex)
      {
        _log.LogError("{ex}: Internal Server Error", ex);
        return StatusCode(500, ex.Message);
      }
    }
    #endregion
  }
}
