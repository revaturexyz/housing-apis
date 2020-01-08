using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Account.Lib.Interface;
using Revature.Account.Lib.Model;

namespace Revature.Account.Api.Controllers
{
  /// <summary>
  /// RESTful API Controllers for the Coordinator account.
  /// </summary>
  [Route("api/coordinator-accounts")]
  [ApiController]
  public class CoordinatorAccountController : ControllerBase
  {
    private readonly IGenericRepository _repo;
    private readonly ILogger<CoordinatorAccountController> _logger;
    private readonly IOktaHelperFactory _oktaHelperFactory;

    public CoordinatorAccountController(IGenericRepository repo, ILogger<CoordinatorAccountController> logger, IOktaHelperFactory oktaHelperFactory)
    {
      _repo = repo ?? throw new ArgumentNullException(nameof(repo));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _oktaHelperFactory = oktaHelperFactory;
    }

    /* This method is called every time the user first shows up to the site in order to
     * check that the token has all of the information it needs and that the info
     * it has is correct.
     */
    // NOTE: You literally call ...accounts/id, not with any particular id
    // GET: api/coordinator-accounts/id
    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult> Get()
    {
      _logger.LogInformation($"GET - Retrieving user ID and verifying correct metadata is in token.");

      try
      {
        var okta = _oktaHelperFactory.Create(Request);
        var oktaUser = await okta.Client.Users.GetUserAsync(okta.Email);
        var oktaRoles = await okta.Client.Groups.ToList();

        var id = await _repo.GetCoordinatorIdByEmailAsync(okta.Email);
        if (id != Guid.Empty)
        {
          // If their roles arent set properly, set them
          if (!okta.Roles.Contains("coordinator"))
          {
            await okta.AddRoleAsync(oktaUser.Id, oktaRoles.First(r => r.Profile.Name == "coordinator").Id);
          }
        }
        else
        {
          // Check the provider db
          id = await _repo.GetProviderIdByEmailAsync(okta.Email);

          if (id != Guid.Empty
            && !okta.Roles.Contains("approved_provider"))
          {
            // They have no role, so set them as tenant
            await okta.AddRoleAsync(oktaUser.Id, oktaRoles.First(r => r.Profile.Name == "Tenants").Id);
          }
        }

        if (id == Guid.Empty)
        {
          // They have no account anywhere - check roles for coordinator role
          if (okta.Roles.Contains("coordinator"))
          {
            // They have been set as a coordinator on the okta site, so make a new account
            var coordinator = new CoordinatorAccount
            {
              Name = oktaUser.Profile.FirstName != null && oktaUser.Profile.LastName != null
                ? oktaUser.Profile.FirstName + " " + oktaUser.Profile.LastName
                : "No Name",
              Email = okta.Email,
              TrainingCenterName = "No Name",
              TrainingCenterAddress = "No Address"
            };
            // Add them
            _repo.AddCoordinatorAccount(coordinator);
          }
          else
          {
            // Make a new provider
            var provider = new ProviderAccount
            {
              Name = oktaUser.Profile.FirstName != null && oktaUser.Profile.LastName != null
                ? oktaUser.Profile.FirstName + " " + oktaUser.Profile.LastName
                : "No Name",
              Email = okta.Email,
              Status = new Status(Status.Pending),
              AccountCreatedAt = DateTime.Now,
              AccountExpiresAt = DateTime.Now.AddDays(7)
            };
            // Add them
            _repo.AddProviderAccountAsync(provider);

            // No notification is made. This is handled in the frontend when
            // they select a training center and click 'request approval'

            // They have no role, so set them as unapproved
            await okta.AddRoleAsync(oktaUser.Id, oktaRoles.First(r => r.Profile.Name == "Tenants").Id);
          }
          // Db was modified either way, save changes
          await _repo.SaveAsync();

          // Get their id
          id = await _repo.GetProviderIdByEmailAsync(okta.Email);
        }

        // Update the app_metadata if it doesnt contain the correct id
        await okta.UpdateMetadataWithIdAsync(oktaUser.Id, id);

        return Ok(id);
      }
      catch (Exception e)
      {
        _logger.LogError(e, "Error occurred in token setup");
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
      }
    }

    // GET: api/coordinator-accounts/5
    [HttpGet("{coordinatorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> Get(Guid coordinatorId)
    {
      try
      {
        _logger.LogInformation("GET - Getting coordinator with ID: {coordinatorId}", coordinatorId);
        var coordinator = await _repo.GetCoordinatorAccountByIdAsync(coordinatorId);

        if (coordinator == null)
        {
          _logger.LogWarning("No coordinator found for given ID: {coordinatorId} on GET call.", coordinatorId);
          return NotFound();
        }

        return Ok(coordinator);
      }
      catch (Exception e)
      {
        _logger.LogError("Exception getting coordinator: {exceptionMessage}, {exception}", e.Message, e);
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
      }
    }

    // GET: api/coordinator-accounts/all
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> GetAll()
    {
      try
      {
        _logger.LogInformation("GET - Retreiving all coordinators.");
        var coordinators = await _repo.GetAllCoordinatorAccountsAsync();

        return Ok(coordinators);
      }
      catch (Exception e)
      {
        _logger.LogError("Exception occurred in GetAll for coordinator controller: {exceptionMessage}, {exception}", e.Message, e);
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
      }
    }
  }
}
