using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Address.Api.Models;
using Revature.Address.Lib.Interfaces;
using Revature.Address.Lib.BusinessLogic;

namespace Revature.Address.Api.Controllers
{
  /// <summary>
  /// This controller handles http requests sent to the
  /// address service
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class AddressController : ControllerBase
  {

    private readonly IDataAccess _db;
    private readonly ILogger _logger;
    private readonly IAddressLogic _addressLogic;

    public AddressController(IDataAccess dataAccess, IAddressLogic addressLogic, ILogger<AddressController> logger = null)
    {
      _db = dataAccess;
      _addressLogic = addressLogic;
      _logger = logger;
    }

    /// <summary>
    /// This method returns an address matching the given addressId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/address/5
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AddressModel>> GetAddressById(Guid id)
    {

      var address = (await _db.GetAddressAsync(id: id)).FirstOrDefault();

      if (address == null)
      {
        _logger.LogError("Address at {id} could not be found", id);
        return NotFound();
      }

      _logger.LogInformation("Got Address");
      return Ok(new AddressModel
      {
        Street = address.Street,
        City = address.City,
        State = address.State,
        Country = address.Country,
        ZipCode = address.ZipCode
      });
    }

    /// <summary>
    /// This method takes in two addressses from a url
    /// query string and returns true if they are within
    /// 20 miles of each other using Google's Distance MAtrix API
    /// and returns false if they are not.
    /// </summary>
    /// <param name="addresses"></param>
    /// <param name="addressLogic"></param>
    /// <returns></returns>
    // GET: api/address/checkdistance
    [HttpGet("checkdistance")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<bool>> IsInRange([FromQuery] AddressModel from, [FromQuery] AddressModel to)
    {
      var start = new Lib.Address
      {
        Street = from.Street,
        City = from.City,
        State = from.State,
        Country = from.Country,
        ZipCode = from.ZipCode
      };
      var end = new Lib.Address
      {
        Street = to.Street,
        City = to.City,
        State = to.State,
        Country = to.Country,
        ZipCode = to.ZipCode
      };
      if (await _addressLogic.IsInRangeAsync(start, end, 20))
      {
        _logger.LogInformation("These addresses are within range of each other");
        return Ok(true);
      }
      else
      {
        _logger.LogError("These addresses are not in range of each other");
        return Ok(false);
      }
    }

    /// <summary>
    /// This method checks if an address already exists in the database,
    /// and if it does, it returns its addressId. If it doesn't exist it checks if the address
    /// exists with Google's Geocode API and if it does it's added to the database and
    /// its addressId is returned, otherwise a bad request message is returned.
    /// </summary>
    /// <param name="address"></param>
    /// <param name="addressLogic"></param>
    /// <returns></returns>
    // GET: api/address
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(409)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<AddressModel>> PostAddress([FromBody] AddressModel address)
    {
      var newAddress = new Lib.Address
      {
        Street = address.Street,
        City = address.City,
        State = address.State,
        Country = address.Country,
        ZipCode = address.ZipCode
      };
      var checkAddress = (await _db.GetAddressAsync(address: newAddress)).FirstOrDefault();

      if (checkAddress == null)
      {
        _logger.LogInformation("Address does not exist in the database");
        newAddress.Id = Guid.NewGuid();
        if (await _addressLogic.IsValidAddressAsync(newAddress))
        {
          try
          {
            var normalAddress = await _addressLogic.NormalizeAddressAsync(newAddress);

            await _db.AddAddressAsync(normalAddress);
            await _db.SaveAsync();
            _logger.LogInformation("Address successfully created");
            return CreatedAtAction(nameof(GetAddressById), new { id = newAddress.Id}, new AddressModel(){
              Street = normalAddress.Street,
              City = normalAddress.City,
              State = normalAddress.State,
              Country = normalAddress.Country,
              ZipCode = normalAddress.ZipCode
            });
          }
          catch (Exception ex)
          {
            _logger.LogError("{0}", ex);
            return BadRequest("Address entry failed");
          }
        }
        else
        {
          _logger.LogError("Address is not a real address");
          return BadRequest("Address is not a real address");
        }
      }
      else
      {

        _logger.LogError("Address already exists in the database");
        return Conflict(checkAddress.Id);
      }
    }
  }
}
