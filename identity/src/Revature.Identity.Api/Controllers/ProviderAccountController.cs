using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Identity.Lib.Interface;
using Revature.Identity.Lib.Model;

namespace Revature.Identity.Api.Controllers
{
  /// <summary>
  /// RESTful API Controllers for the Provider account.
  /// </summary>
  [Route("api/provider-accounts")]
  [ApiController]
  public class ProviderAccountController : ControllerBase
  {
    private readonly IGenericRepository _repo;
    private readonly ILogger<ProviderAccountController> _logger;
    private readonly IOktaHelperFactory _oktaHelperFactory;

    public ProviderAccountController(IGenericRepository repo, ILogger<ProviderAccountController> logger, IOktaHelperFactory authHelperFactory)
    {
      _repo = repo ?? throw new ArgumentNullException(nameof(repo));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _oktaHelperFactory = authHelperFactory;
    }

    // GET: api/provider-accounts/5
    [HttpGet("{providerId}", Name = "GetProviderAccountById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> Get(Guid providerId)
    {
      _logger.LogInformation($"GET - Getting provider account by ID: {providerId}");
      var provider = await _repo.GetProviderAccountByIdAsync(providerId);
      if (provider == null)
      {
        _logger.LogWarning($"No provider account found for {providerId}");
        return NotFound();
      }

      return Ok(provider);
    }

    // POST: api/provider-accounts
    /* POST was removed because the flow is handled automatically by the
     * coordinator controller upon calling coordinator-accounts/id and
     * the connection between provider and coordinator and the sending
     * of notifications is handled on the frontend.
     */

    // PUT: api/provider-accounts/5
    [HttpPut("{providerId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator")]
    public async Task<IActionResult> Put(Guid providerId, [FromBody, Bind("CoordinatorId, Name, Email")] ProviderAccount provider)
    {
      _logger.LogInformation($"PUT - Put request for provider ID: {providerId}");
      var existingProvider = await _repo.GetProviderAccountByIdAsync(providerId);
      if (existingProvider != null)
      {
        existingProvider.CoordinatorId = provider.CoordinatorId;
        existingProvider.Name = provider.Name;
        existingProvider.Email = provider.Email;

        await _repo.UpdateProviderAccountAsync(existingProvider);
        await _repo.SaveAsync();
        _logger.LogInformation($"Put request persisted for {providerId}");
        return NoContent();
      }

      _logger.LogWarning($"Put request failed for {providerId}");
      return NotFound();
    }

    // PUT: api/provider-accounts/{id}/approve
    [HttpPut("{providerId}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator")]
    public async Task<IActionResult> Put(Guid providerId)
    {
      _logger.LogInformation($"PUT - Approval request for provider: {providerId}");
      var existingProvider = await _repo.GetProviderAccountByIdAsync(providerId);
      if (existingProvider != null && existingProvider.Status.StatusText != Status.Approved)
      {
        // This section would effect the coordinator, not the provider.
        // The Get() methodin CoordinatorAccountController will update Okta permissions on next login.
        // OktaHelper okta = _oktaHelperFactory.Create(Request);
        // var oktaUser = await okta.Client.Users.FirstOrDefault(user => user.Profile.Email == okta.Email);
        //// Add approved_provider
        // await okta.AddRoleAsync(oktaUser.Id, OktaHelper.ApprovedProviderRole);
        existingProvider.Status.StatusText = Status.Approved;
        await _repo.UpdateProviderAccountAsync(existingProvider);
        await _repo.SaveAsync();

        _logger.LogInformation($"Approval set to true for id: {providerId}");
        return NoContent();
      }

      _logger.LogWarning($"Account already is approved, no change for id: {providerId}");
      return NotFound();
    }

    // DELETE: api/provider-accounts/5
    [HttpDelete("{providerId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator")]
    public async Task<ActionResult> Delete(Guid providerId)
    {
      _logger.LogInformation($"DELETE - Delete request for provider ID: {providerId}");
      var existingProvider = await _repo.GetProviderAccountByIdAsync(providerId);
      if (existingProvider != null)
      {
        await _repo.DeleteProviderAccountAsync(providerId);
        await _repo.SaveAsync();

        _logger.LogInformation($"Delete request persisted for {providerId}");
        return NoContent();
      }

      _logger.LogWarning($"Delete request failed for {providerId}");
      return NotFound();
    }
  }
}
