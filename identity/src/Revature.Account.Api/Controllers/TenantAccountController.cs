using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Account.Api;
using Revature.Account.Lib.Interface;
using Revature.Account.Lib.Model;
//using Revature.Identity.Api.Auth;
//using Revature.Identity.Lib.Interface;
//using Revature.Identity.Lib.Model;

namespace Revature.Identity.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TenantController : ControllerBase
  {

    private readonly IGenericRepository _repo;
    private readonly ILogger<TenantController> _logger;
    private readonly IOktaHelperFactory _oktaHelperFactory;

    public TenantController(IGenericRepository repo, ILogger<TenantController> logger, IOktaHelperFactory authHelperFactory)
    {
      _repo = repo ?? throw new ArgumentNullException(nameof(repo));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _oktaHelperFactory = authHelperFactory;
    }





    // GET: api/tenant-accounts/5
    /// <summary>
    /// Return the All Tenants attatched to a Provider
    /// </summary>
    /// <param name="ProviderId"></param>
    /// <returns></returns>
    [HttpGet("{tenantId}", Name = "GetTenantByProvider")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> Get(Guid ProviderId)
    {
      _logger.LogInformation($"GET - Getting tenant account by Provider: {ProviderId}");
      var tenant = await _repo.GetAllTenantsByProviderAsync(ProviderId);
      if (tenant == null)
      {
        _logger.LogWarning($"No tenant account found for {ProviderId}");
        return NotFound();
      }
      return Ok(tenant);
    }





    // GET: api/tenant-accounts/5
    /// <summary>
    /// return all tenants attatched to a Coordinator
    /// </summary>
    /// <param name="CoordinatorId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{tenantId}", Name = "GetTenantByCoordinator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> Get(Guid CoordinatorId, int? id)
    {
      _logger.LogInformation($"GET - Getting tenant account by Coordinator ID: {CoordinatorId}");
      var tenant = await _repo.GetAllTenantsByCoordinatorAsync(CoordinatorId);
      if (tenant == null)
      {
        _logger.LogWarning($"No tenant account found for {CoordinatorId}");
        return NotFound();
      }
      return Ok(tenant);
    }





    // GET: api/tenant-accounts/5
    /// <summary>
    /// return a tenant id based on a tenant email.
    /// </summary>
    /// <param name="Email"></param>
    /// <returns></returns>
    [HttpGet("{tenantId}", Name = "GetTenantByEmail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> Get(string Email)
    {
      _logger.LogInformation($"GET - Getting tenant id by Email: {Email}");
      var tenant = await _repo.GetTenantIdByEmailAsync(Email);
      if (tenant == null)
      {
        _logger.LogWarning($"No tenant account found for {Email}");
        return NotFound();
      }
      return Ok(tenant);
    }





    // PUT: api/tenant-accounts/5
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    [HttpPut("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Put(Guid tenantId, [FromBody, Bind("TenantId, Name, Email")] TenantAccount tenant)
    {
      _logger.LogInformation($"PUT - Put request for tenant ID: {tenantId}");
      var existingTenant = await _repo.GetTenantAccountByIdAsync(tenantId);
      if (existingTenant != null)
      {
        existingTenant.TenantId = tenant.TenantId;
        existingTenant.Name = tenant.Name;
        existingTenant.Email = tenant.Email;

        await _repo.UpdateTenantAccountAsync(existingTenant);
        await _repo.SaveAsync();
        _logger.LogInformation($"Put request persisted for {tenantId}");
        return NoContent();
      }
      _logger.LogWarning($"Put request failed for {tenantId}");
      return NotFound();
    }





    // DELETE: api/provider-accounts/5
    [HttpDelete("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> Delete(Guid tenantId)
    {
      _logger.LogInformation($"DELETE - Delete request for tenant ID: {tenantId}");
      var existingProvider = await _repo.GetTenantAccountByIdAsync(tenantId);
      if (existingProvider != null)
      {
        await _repo.DeleteTenantAccountAsync(tenantId);
        await _repo.SaveAsync();
        _logger.LogInformation($"Delete request persisted for {tenantId}");
        return NoContent();
      }
      _logger.LogWarning($"Delete request failed for {tenantId}");
      return NotFound();
    }





    // POST: api/tenant-accounts/5
    /// <summary>
    /// Create a new tenant based on a tenantId 
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    [HttpPut("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Post(Guid tenantId, [FromBody, Bind("TenantId, Name, Email")] TenantAccount tenant)
    {
      _logger.LogInformation($"PUT - Put request for provider ID: {tenantId}");
      var existingProvider = await _repo.AddTenantAccount(tenantId);
      if (existingProvider != null)
      {
        existingProvider.CoordinatorId = tenant.CoordinatorId;
        existingProvider.Name = tenant.Name;
        existingProvider.Email = tenant.Email;

        await _repo.UpdateTenantAccountAsync(existingProvider);
        await _repo.SaveAsync();
        _logger.LogInformation($"Put request persisted for {tenantId}");
        return NoContent();
      }
      _logger.LogWarning($"Put request failed for {tenantId}");
      return NotFound();
    }
  }
}
