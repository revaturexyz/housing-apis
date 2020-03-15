using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Lodging.Api.Models;
using Revature.Lodging.Api.Services;
using Revature.Lodging.Lib.Interface;
using Logic = Revature.Lodging.Lib.Models;

namespace Revature.Lodging.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ComplexController : Controller
  {
    private readonly IComplexRepository _complexRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly ILogger<ComplexController> _log;
    private readonly IAddressRequest _addressRequest;

    public ComplexController(IComplexRepository complexRepository, ILogger<ComplexController> logger, IAddressRequest ar, IAmenityRepository amenityRepository)
    {
      _complexRepository = complexRepository ?? throw new ArgumentNullException(nameof(complexRepository), "Complex repo cannot be null");
      _amenityRepository = amenityRepository ?? throw new ArgumentNullException(nameof(amenityRepository), "Amenity repo cannot be null");
      _log = logger;
      _addressRequest = ar;
    }

    /// <summary>
    /// Gets all existing Complex objects from database.
    /// GET: api/complex
    /// </summary>
    /// <returns>Collection of Complex objects with a list of associated amenities.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    [Authorize(Roles = "Coordinator, Provider")] // OktaSetup
    public async Task<ActionResult<IEnumerable<ApiComplex>>> GetAllComplexesAsync()
    {
      // Gets all the existing complexes from the database
      var complexes = await _complexRepository.ReadComplexListAsync();

      // Initializes an empty list that will hold all complexes and their associated complex amenities
      var apiComplexes = new List<ApiComplex>();

      // For each complex, use the addressId to get address from Address service
      // Create ApiComplex object for each Complex we have, and add them to a list that will be returned
      foreach (var complex in complexes)
      {
        var addressId = complex.AddressId;
        ApiAddress address;

        // Try to get an address with an addressId
        // If it doesn't exist, it will create an address object with just the Id property set
        // This looks wrong and should be reevaluated; address GUID should normally always be valid
        try
        {
          address = await _addressRequest.GetAddressAsync(addressId);
        }
        catch
        {
          address = new ApiAddress()
          {
            Id = addressId
          };
        }

        // Creates a complex with its associated amenities that will be added to the list
        var apiComplex = new ApiComplex
        {
          ComplexId = complex.Id,
          Address = address,
          ProviderId = complex.ProviderId,
          ComplexName = complex.ComplexName,
          ContactNumber = complex.ContactNumber,
          ComplexAmenities = await _amenityRepository.ReadAmenityListByComplexIdAsync(complex.Id)
        };
        _log.LogInformation("A list of amenities for complex Id {com.ComplexId} were found!", complex.Id);
        apiComplexes.Add(apiComplex);
      }

      return Ok(apiComplexes);
    }

    /// <summary>
    /// Get an existing Complex object by a ComplexId from database.
    /// GET: api/complex/{complexId}
    /// </summary>
    /// <param name="complexId"> Specifies the Complex object.</param>
    /// <returns>Complex object with associated amenities.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{complexId}")]
    [Authorize(Roles = "Coordinator, Provider")] // OktaSetup
    public async Task<ActionResult<ApiComplex>> GetComplexByIdAsync([FromRoute]Guid complexId)
    {
      // Gets an existing Complex associated with the complexId
      var complex = await _complexRepository.ReadComplexByIdAsync(complexId);
      _log.LogInformation("A complex with Id: {complexId} was found", complexId);

      var addressId = complex.AddressId;
      ApiAddress address;

      // Try to get an address with an addressId
      // If it doesn't exist, it will create an address object with just the Id property
      // This looks wrong and should be reevaluated; address GUID should normally always be valid
      try
      {
        address = await _addressRequest.GetAddressAsync(addressId);
      }
      catch
      {
        address = new ApiAddress()
        {
          Id = addressId
        };
      }

      // Creates a complex with its associated Amenities
      var apiComplex = new ApiComplex
      {
        ComplexId = complex.Id,
        Address = address,
        ProviderId = complex.ProviderId,
        ComplexName = complex.ComplexName,
        ContactNumber = complex.ContactNumber,
        ComplexAmenities = await _amenityRepository.ReadAmenityListByComplexIdAsync(complex.Id)
      };
      _log.LogInformation("A list of amenities for complex Id {lcomplex.ComplexId} was found!", complex.Id);

      return Ok(apiComplex);
    }

    /// <summary>
    /// Get Complex objects given a ProviderId from database.
    /// GET: api/complex/providerId/{providerId}
    /// </summary>
    /// <param name="providerId"> Indicates needed Complex objects. </param>
    /// <returns> Collection of Complex objects with associated amenities. </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("providerId/{providerId}")]
    [Authorize(Roles = "Coordinator, Provider")] // OktaSetup
    public async Task<ActionResult<IEnumerable<ApiComplex>>> GetComplexesByProviderId([FromRoute]Guid providerId)
    {
      // Gets all existing complexes associated with the given providerId
      var complexes = await _complexRepository.ReadComplexByProviderIdAsync(providerId);
      _log.LogInformation("A list of complexes for provider Id: {providerId} were found", providerId);

      // Initializes an empty list that will hold all complexes and their associated complex amenities
      var apiComplexes = new List<ApiComplex>();

      // For each complex, use the addressId to get address from Address service
      // Create ApiComplex object for each Complex we have, and add them to a list that will be returned
      foreach (var complex in complexes)
      {
        var addressId = complex.AddressId;
        ApiAddress address;

        // This looks wrong and should be reevaluated; address GUID should normally always be valid
        try
        {
          address = await _addressRequest.GetAddressAsync(addressId);
        }
        catch
        {
          address = new ApiAddress()
          {
            Id = addressId
          };
        }

        var apiComplex = new ApiComplex
        {
          ComplexId = complex.Id,
          Address = address,
          ProviderId = complex.ProviderId,
          ComplexName = complex.ComplexName,
          ContactNumber = complex.ContactNumber,
          ComplexAmenities = await _amenityRepository.ReadAmenityListByComplexIdAsync(complex.Id)
        };
        _log.LogInformation("A list of amenities for complex Id {complex.ComplexId} was found!", complex.Id);

        apiComplexes.Add(apiComplex);
      }

      return Ok(apiComplexes);
    }

    /// <summary>
    /// Adds a new Complex object to the database
    /// Sends Complex address to Address service.
    /// POST: api/complex
    /// </summary>
    /// <param name="apiComplex"> Indicates the new Complex object to be added. </param>
    /// <returns>Added Complex object.</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    [Authorize(Roles = "Provider")] // OktaSetup
    public async Task<ActionResult<ApiComplex>> PostComplexAsync([FromBody]ApiComplex apiComplex)
    {
      // Creates an address from address properties in apiComplex object argument
      var complexAddress = new ApiAddress()
      {
        Street = apiComplex.Address.Street,
        City = apiComplex.Address.City,
        State = apiComplex.Address.State,
        ZipCode = apiComplex.Address.ZipCode,
        Country = apiComplex.Address.Country,
      };

      // Retrieves a normalized address using AddressRequest
      var normalizedAddress = await _addressRequest.PostAddressAsync(complexAddress);

      // Normalizes the address in the Complex object that is passed in
      apiComplex.Address.Id = normalizedAddress.Id;
      apiComplex.Address.Street = normalizedAddress.Street;
      apiComplex.Address.City = normalizedAddress.City;
      apiComplex.Address.State = normalizedAddress.State;
      apiComplex.Address.Country = normalizedAddress.Country;
      apiComplex.Address.ZipCode = normalizedAddress.ZipCode;

      // Generates a new Guid for a ComplexId
      var complexId = Guid.NewGuid();

      // Sets the Id of the Complex object that is passed in
      apiComplex.ComplexId = complexId;

      // Creates a Complex object with the information in the apiComplex object argument
      var complex = new Logic.Complex()
      {
        Id = complexId,
        AddressId = normalizedAddress.Id,
        ProviderId = apiComplex.ProviderId,
        ContactNumber = apiComplex.ContactNumber,
        ComplexName = apiComplex.ComplexName
      };

      try
      {
        // Adds created Complex object into the database
        await _complexRepository.CreateComplexAsync(complex);
        _log.LogInformation("new complex in the database is inserted");

        // Gets all existing Amenity objects from database
        var existingAmenities = await _amenityRepository.ReadAmenityListAsync();
        _log.LogInformation("list of Amenity is found");

        // Create ComplexAmenity objects from the list of Amenities passed in with apiComplex
        foreach (var postedAmenity in apiComplex.ComplexAmenities)
        {
          // Instantiates a new ComplexAmenity object
          var complexAmenity = new Logic.ComplexAmenity();

          // If there are any existing amenities with a matching AmenityType, link this existing amenity
          // with the new ComplexAmenity object; otherwise, create a new amenity with the posted AmenityType and
          // link this new Amenity object with the new ComplexAmenity object
          if (existingAmenities.Any(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower()))
          {
            var amenity = existingAmenities.FirstOrDefault(existingAmenity => existingAmenity.AmenityType.ToLower() == postedAmenity.AmenityType.ToLower());

            complexAmenity.Id = Guid.NewGuid();
            complexAmenity.ComplexId = complexId;
            complexAmenity.AmenityId = amenity.Id;
          }
          else
          {
            var amenity = new Logic.Amenity()
            {
              Id = Guid.NewGuid(),
              AmenityType = postedAmenity.AmenityType,
              Description = null
            };
            await _amenityRepository.CreateAmenityAsync(amenity);

            complexAmenity.Id = Guid.NewGuid();
            complexAmenity.ComplexId = complexId;
            complexAmenity.AmenityId = amenity.Id;
          }

          await _amenityRepository.CreateAmenityComplexAsync(complexAmenity);
          _log.LogInformation("a list of amenities for complex id: {complex.Id} was created", complex.Id);
        }

        return Created($"api/Complex/{complex.Id}", apiComplex);
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "unable to create complex");
        throw;
      }
    }

    /// <summary>
    /// Updates an existing Complex object in the database.
    /// PUT: api/complex//
    /// </summary>
    /// <param name="apiComplex"> Indicates the updated Complex object. </param>
    /// <returns> Appropriate status code. </returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut]
    [Authorize(Roles = "Provider")] // OktaSetup
    public async Task<ActionResult> UpdateComplexAsync([FromBody]ApiComplex apiComplex)
    {
      // Maps the apiComplex to a Complex Library model
      var complex = new Logic.Complex()
      {
        Id = apiComplex.ComplexId,
        AddressId = apiComplex.Address.Id,
        ProviderId = apiComplex.ProviderId,
        ContactNumber = apiComplex.ContactNumber,
        ComplexName = apiComplex.ComplexName
      };

      // Deletes all the complex amenities with an associated complexId
      await _amenityRepository.DeleteAmenityComplexAsync(complex.Id);
      _log.LogInformation("old amenities for complex id: {complex.Id} is deleted", complex.Id);

      try
      {
        // Updates the complex with the new information passed in
        await _complexRepository.UpdateComplexAsync(complex);
        _log.LogInformation("complex is updated");

        // Gets all existing amenities from the database
        var existingAmenities = await _amenityRepository.ReadAmenityListAsync();
        _log.LogInformation("list of amenity is read");

        if (apiComplex.ComplexAmenities != null)
        {
          // Create ComplexAmenity objects from the list of Amenities passed in with apiComplex
          foreach (var postedAmenity in apiComplex.ComplexAmenities)
          {
            // Instantiates a new ComplexAmenity object
            var complexAmenity = new Logic.ComplexAmenity();

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
              var amenity = new Logic.Amenity()
              {
                Id = Guid.NewGuid(),
                AmenityType = postedAmenity.AmenityType,
                Description = null
              };
              await _amenityRepository.CreateAmenityAsync(amenity);

              complexAmenity.Id = Guid.NewGuid();
              complexAmenity.ComplexId = complex.Id;
              complexAmenity.AmenityId = amenity.Id;
            }

            await _amenityRepository.CreateAmenityComplexAsync(complexAmenity);
            _log.LogInformation("new list of amenity of complex is created");
          }
        }

        return StatusCode(200);
      }
      catch (Exception ex)
      {
        _log.LogError(ex, "unable to update complex");
        throw;
      }
    }

    /// <summary>
    /// Deletes a Complex object by ComplexId from database.
    /// DELETE: api/complex/{complexId}
    /// </summary>
    /// <param name="complexId"> Indicates the Complex object to be deleted.</param>
    /// <returns>Appropriate status code.</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{complexId}")]
    [Authorize(Roles = "Provider")] // OktaSetup
    public async Task<ActionResult> DeleteComplexByIdAsync([FromRoute]Guid complexId)
    {
      // Deletes an existing complex associated the complexId in the database
      await _complexRepository.DeleteComplexAsync(complexId);
      _log.LogInformation("deleted complex of complex Id: {complexId}", complexId);

      return NoContent();
    }

    /// <summary>
    /// Call Repository and Address service to get specific complex info
    /// by complex name and phone number as parameters
    /// then return single Api Complex model.
    /// GET: api/complex/{complexName}/{complexNumber}
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{complexName}/{complexNumber}")]
    [Authorize(Roles = "Coordinator, Provider")] // OktaSetup
    public async Task<ActionResult<ApiComplex>> GetComplexByNameAndNumberAsync([FromRoute]string complexName, string complexNumber)
    {
      var lcomplex = await _complexRepository.ReadComplexByNameAndNumberAsync(complexName, complexNumber);
      _log.LogInformation("a complex with name: {complexName} and phone: {complexNumber} was found", complexName, complexNumber);

      // var aId = lcomplex.AddressId;
      // var address = await _addressRequest.GetAddressAsync(aId);
      var apiComplex = new ApiComplex
      {
        ComplexId = lcomplex.Id,
        // Address = address,
        ProviderId = lcomplex.ProviderId,
        ComplexName = lcomplex.ComplexName,
        ContactNumber = lcomplex.ContactNumber,
        ComplexAmenities = await _amenityRepository.ReadAmenityListByComplexIdAsync(lcomplex.Id)
      };
      _log.LogInformation("a list of amenities for complex Id {lcomplex.ComplexId} were found!", lcomplex.Id);

      return Ok(apiComplex);
    }
  }
}
