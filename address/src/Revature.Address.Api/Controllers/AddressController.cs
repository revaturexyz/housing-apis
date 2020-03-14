using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Address.Api.Models;
using Revature.Address.Lib.BusinessLogic;
using Revature.Address.Lib.Interfaces;

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
    public async Task<ActionResult<AddressModel>> GetAddressById(Guid id)
    {

      var address = (await _db.GetAddressAsync(id: id)).FirstOrDefault();
      if (address != null)
        return Ok(Mapper.Map(address));
      else
      {
        return NotFound("Address does not exist in address service");
      }


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
    public async Task<ActionResult<bool>> IsInRange([FromQuery] AddressModel from, [FromQuery] AddressModel to, [FromQuery] int distance = 20)
    {
      try
      {
        var start = Mapper.Map(from);
        var end = Mapper.Map(to);
        if (await _addressLogic.IsInRangeAsync(start, end, distance))
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
      catch (Exception e)
      {
        _logger.LogError(e.Message);
        return BadRequest();
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
    [HttpGet]
    public async Task<ActionResult<AddressModel>> GetAddress([FromQuery] AddressModel address)
    {
      try
      {
        var newAddress = await Mapper.MapVerifyAndNormalize(address, _addressLogic);
        var checkAddress = (await _db.GetAddressAsync(address: newAddress)).FirstOrDefault();

        if (checkAddress == null)
        {
          _logger.LogInformation("Address does not exist in the database");
          newAddress.Id = Guid.NewGuid();
          await _db.AddAddressAsync(newAddress);
          await _db.SaveAsync();
          _logger.LogInformation("Address successfully created");
          return Mapper.Map(newAddress);
        }
        else
        {

          _logger.LogError("Address already exists in the database");
          return Mapper.Map(checkAddress);
        }
      }
      catch (ArgumentException e)
      {

        _logger.LogError(e.Message);
        return BadRequest(e.Message);
      }

    }
  }
}
