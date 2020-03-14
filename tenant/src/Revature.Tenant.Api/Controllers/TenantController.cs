using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Tenant.Api.Models;
using Revature.Tenant.Lib.Interface;

namespace Revature.Tenant.Api.Controllers
{
  /// <summary>
  /// This Controller maintains all Tenant Actions that can be accessed through the Tenant Service Rest API
  /// </summary>
  // root uri: api/Tenant
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class TenantController : ControllerBase
  {
    private readonly ITenantRepository _tenantRepository;
    private readonly ILogger _logger;
    private readonly IAddressService _addressService;
    private readonly ServiceBus.IIdentityService _tenantService;

    public TenantController(ITenantRepository tenantRepository, IAddressService addressService, ServiceBus.IIdentityService tenantService, ILogger<TenantController> logger = null)
    {
      _tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository), "Tenant cannot be null");
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _addressService = addressService ?? throw new ArgumentNullException(nameof(addressService));
      _tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
    }

    /// <summary>
    /// Get all tenants
    /// </summary>
    /// <returns>If no search parameters, all tenants. Else, All tenants who match parameters.</returns>
    // GET: api/Tenant
    [HttpGet(Name = "GetAllAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator")]
    public async Task<ActionResult<IEnumerable<ApiTenant>>> GetAllAsync([FromQuery] string firstName = null, [FromQuery] string lastName = null, [FromQuery] string gender = null, [FromQuery] string trainingCenter = null)
    {
      //Parse training center string to guid if it exists
      Guid? trainingCenterGuid;
      if (trainingCenter != null)
        trainingCenterGuid = Guid.Parse(trainingCenter);
      else
        trainingCenterGuid = null;

      //Call repository GetAllAsync
      _logger.LogInformation("GET - Getting tenants");
      var tenants = await _tenantRepository.GetAllAsync(firstName, lastName, gender, trainingCenterGuid);

      //Maps batches and cars correctly, based on null or not null
      var newTenants = new List<Lib.Models.Tenant>();
      foreach (var tenant in tenants)
      {
        Lib.Models.Batch batch;
        int? batchId;
        if (tenant.Batch != null)
        {
          batch = new Lib.Models.Batch
          {
            Id = tenant.Batch.Id,
            BatchCurriculum = tenant.Batch.BatchCurriculum,
            TrainingCenter = tenant.Batch.TrainingCenter
          };
          batch.SetStartAndEndDate(tenant.Batch.StartDate, tenant.Batch.EndDate);
          batchId = tenant.BatchId;
        }
        else
        {
          batch = null;
          batchId = null;
        }
        tenant.Batch = batch;
        tenant.BatchId = batchId;
        Lib.Models.Car car;
        int? carId;
        if (tenant.Car != null)
        {
          car = new Lib.Models.Car()
          {
            Id = tenant.Car.Id,
            LicensePlate = tenant.Car.LicensePlate,
            Make = tenant.Car.Make,
            Model = tenant.Car.Model,
            Color = tenant.Car.Color,
            Year = tenant.Car.Year,
            State = tenant.Car.State
          };
          carId = tenant.CarId;
          tenant.CarId = carId;
        }
        else
        {
          car = null;
        }
        tenant.Car = car;

        newTenants.Add(tenant);
      }

      //Cast all Logic Tenants into ApiTenants
      var apiTenants = new List<ApiTenant>();
      foreach (var apiTenant in newTenants)
      {
        var newApiTenant = new ApiTenant
        {
          Id = apiTenant.Id,
          Email = apiTenant.Email,
          Gender = apiTenant.Gender,
          FirstName = apiTenant.FirstName,
          LastName = apiTenant.LastName,
          AddressId = apiTenant.AddressId,
          RoomId = apiTenant.RoomId,
          CarId = apiTenant.CarId,
          BatchId = apiTenant.BatchId,
          TrainingCenter = apiTenant.TrainingCenter
        };
        apiTenants.Add(newApiTenant);
      }
      //Return OK with a list of tenants
      return Ok(apiTenants);
    }

    /// <summary>
    /// Get Tenant by Id
    /// </summary>
    /// <param name="id">The Guid Id of a tenant</param>
    /// <returns>A Tenant with Batch and Car details, or NotFound if not found, or Internal Service Error for other exceptions</returns>
    // GET: api/Tenant/5
    //api/[controller]
    [HttpGet("{id}", Name = "GetByIdAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator,Tenant")]
    public async Task<ActionResult<ApiTenant>> GetByIdAsync([FromRoute] Guid id)
    {
      _logger.LogInformation("GET - Getting notifications by Tenant ID: {TenantId}", id);
      try
      {
        //Call repository method GetByIdAsync
        var tenant = await _tenantRepository.GetByIdAsync(id);

        /* if role is tenant and tenant.Email != claims email
         *  return 401Forbidden
         */
        var principal = HttpContext.User.Identities.SingleOrDefault();
        var roles = principal.Claims.Where(c => c.Type == "groups").Select(c => c.Value).ToList();

        if (roles.Contains("Tenant"))
        {
          try
          {
            var email = principal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).Single();

            if (email?.ToLower() != tenant?.Email?.ToLower())
            {
              _logger.LogWarning("Tenant trying to access other tenant account");
              return Forbid();
            }
          }
          catch (ArgumentNullException)
          {
            _logger.LogError("Email does not exist in Okta account");
            return NotFound();
          }
        }
        ApiAddress address;
        try
        {
          address = await _addressService.GetAddressAsync(tenant.AddressId);
        }
        catch
        {
          address = null;
        }

        //cast tenant into Api Tenant
        var apiTenant = new ApiTenant
        {
          Id = tenant.Id,
          Email = tenant.Email,
          Gender = tenant.Gender,
          FirstName = tenant.FirstName,
          LastName = tenant.LastName,
          AddressId = tenant.AddressId,
          RoomId = tenant.RoomId,
          CarId = tenant.CarId,
          BatchId = tenant.BatchId,
          TrainingCenter = tenant.TrainingCenter
        };

        if (apiTenant.CarId != null)
        {
          apiTenant.ApiCar = new ApiCar
          {
            Id = tenant.Car.Id,
            Color = tenant.Car.Color,
            Make = tenant.Car.Make,
            Model = tenant.Car.Model,
            LicensePlate = tenant.Car.LicensePlate,
            State = tenant.Car.State,
            Year = tenant.Car.Year
          };
        }

        if (apiTenant.BatchId != null)
        {
          apiTenant.ApiBatch = new ApiBatch
          {
            Id = tenant.Batch.Id,
            BatchCurriculum = tenant.Batch.BatchCurriculum,
            StartDate = tenant.Batch.StartDate,
            EndDate = tenant.Batch.EndDate,
            TrainingCenter = tenant.Batch.TrainingCenter
          };
        }
        apiTenant.ApiAddress = address;
        //return OK with the ApiTenant Model, including car and batch if applicable
        return Ok(apiTenant);
      }
      catch (ArgumentException)
      {
        _logger.LogWarning("Tenant was not found");

        return NotFound();
      }
      catch (Exception e)
      {
        _logger.LogError("Get request failed. Error: " + e.Message);

        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
      }
    }

    /// <summary>
    /// Get all batches by training center id
    /// </summary>
    /// <param name="trainingCenterString">String of guid of Training Center</param>
    /// <returns>A list of batches whose TrainingCenter is the same as the parameter, or NotFound if not found, or Internal Service Error for other exceptions</returns>
    // GET: api/Tenant/Batch/[guid]
    [HttpGet("Batch", Name = "GetAllBatches")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator")]
    public async Task<ActionResult<IEnumerable<ApiBatch>>> GetAllBatches([FromQuery] string trainingCenterString)
    {
      try
      {
        //Cast string into Guid
        var trainingCenter = Guid.Parse(trainingCenterString);
        //Call Repo method GetBatchesAsync
        var batches = await _tenantRepository.GetBatchesAsync(trainingCenter);
        //Return Ok with a list of batches
        return Ok(batches);
      }
      catch (ArgumentException)
      {
        _logger.LogWarning("Training Center was not found");

        return NotFound();
      }
      catch (Exception e)
      {
        _logger.LogError("Get request failed. Error: " + e.Message);

        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
      }
    }

    /// <summary>
    /// Posts Tenant to Db
    /// </summary>
    /// <param name="tenant">A tenant api model of a new tenant</param>
    /// <returns>An apiTenant model of the new tenant, or NotFound if not found, or Conflict for Invalid Operations, or Internal Service Error for other exceptions/returns>
    // POST: api/Tenant/Register
    [HttpPost("Register", Name = "RegisterTenant")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Coordinator")]
    public async Task<ActionResult<ApiTenant>> PostAsync([FromBody] ApiTenant tenant)
    {
      _logger.LogInformation("POST - Making tenant.");
      try
      {
        _logger.LogInformation("Posting Address to Address Service...");
        var postedAddress = await _addressService.GetAddressAsync(tenant.ApiAddress);
        Guid sharedId = Guid.NewGuid();
        tenant.Id = sharedId;
        tenant.AddressId = postedAddress.Id;
        //cast ApiTenant in Logic Tenant
        var newTenant = ApiMapper.Map(tenant);

        if (tenant.ApiCar?.LicensePlate != null)
        {
          newTenant.Car = ApiMapper.Map(tenant.ApiCar);
          newTenant.CarId = 0;
        }
        else
        {
          newTenant.Car = null;
          newTenant.CarId = null;
        }


        //Call Repository Methods AddAsync and SaveAsync
        await _tenantRepository.AddAsync(newTenant);
        await _tenantRepository.SaveAsync();
        _logger.LogInformation("POST Persisted to dB");

        //Send the create message to Identity

        string Name = tenant.FirstName + " " + tenant.LastName;
        await _tenantService.CreateAccount(sharedId, tenant.Email, Name);

        //Return Created and the model of the new tenant
        return Created($"api/Tenant/{newTenant.Id}", newTenant);
      }
      catch (ArgumentException)
      {
        _logger.LogWarning("Not Found");
        return NotFound();
      }
      catch (InvalidOperationException e)
      {
        _logger.LogError("POST request failed. Error: " + e.Message);
        return Conflict(e.Message);
      }
      catch (Exception e)
      {
        _logger.LogError("POST request failed. Error: " + e.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
      }
    }

    /// <summary>
    /// Posts Tenant to Db
    /// </summary>
    /// <param name="tenant">A tenant api model of an existing tenant</param>
    /// <returns>An apiTenant model of the existing tenant, or NotFound if not found, or Conflict for Invalid Operations, or Internal Service Error for other exceptions</returns>
    // PUT: api/Tenant/Update
    [HttpPut("Update", Name = "UpdateTenant")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Coordinator")]
    public async Task<ActionResult> UpdateAsync([FromBody] ApiTenant tenant)
    {
      try
      {
        _logger.LogInformation("PUT - Updating tenant with tenantid {tenantId}.", tenant.Id);
        _logger.LogInformation("Posting Address to Address Service...");
        var postedAddress = await this._addressService.GetAddressAsync(tenant.ApiAddress);
        tenant.AddressId = postedAddress.Id;
        //cast ApiTenant in Logic Tenant
        var newTenant = ApiMapper.Map(tenant);

        if (tenant.ApiCar != null)
        {
          newTenant.Car = ApiMapper.Map(tenant.ApiCar);
        }

        //Call repository method Put and Save Async
        _tenantRepository.Put(newTenant);
        await _tenantRepository.SaveAsync();
        _logger.LogInformation("PUT persisted to dB");

        string Name = tenant.FirstName + " " + tenant.LastName;
        await _tenantService.UpdateAccount((Guid)tenant.Id, tenant.Email, Name);

        //Return NoContent
        return StatusCode(StatusCodes.Status204NoContent);
      }
      catch (ArgumentException)
      {
        _logger.LogWarning("PUT request failed. Not Found Exception");
        return NotFound();
      }
      catch (InvalidOperationException e)
      {
        _logger.LogError("PUT request failed. Error: " + e.Message);
        return Conflict(e.Message);
      }
      catch (Exception e)
      {
        _logger.LogError("PUT request failed. Error: " + e.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
      }
    }

    /// <summary>
    /// Delete a tenant by id
    /// </summary>
    /// <param name="id">Guid Id, converted from string in Body</param>
    /// <returns>Status Code 204 if successful, or NotFound if not found, or Conflict for Invalid Operations, or Internal Service Error for other exceptions</returns>
    [HttpDelete("Delete/{id}", Name = "DeleteTenant")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Coordinator")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
    {
      try
      {
        await _tenantRepository.DeleteByIdAsync(id);
        await _tenantRepository.SaveAsync();
        await _tenantService.DeleteAccount(id, "unused", "unused");

        return StatusCode(StatusCodes.Status204NoContent);
      }
      catch (ArgumentException)
      {
        _logger.LogWarning("DELETE request failed. Not Found Exception");
        return NotFound();
      }
      catch (InvalidOperationException e)
      {
        _logger.LogError("DELETE request failed. Error: " + e.Message);
        return Conflict(e.Message);
      }
      catch (Exception e)
      {
        _logger.LogError("DELETE request failed. Error: " + e.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
      }
    }
  }
}
