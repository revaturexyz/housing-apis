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

namespace Revature.Account.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TenantAccountController : ControllerBase
  {

    private readonly IGenericRepository _repo;
    private readonly ILogger<TenantAccountController> _logger;

    public TenantAccountController(IGenericRepository repo, ILogger<TenantAccountController> logger)
    {
      _repo = repo ?? throw new ArgumentNullException(nameof(repo));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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



    // GET: api/tenant-accounts/5
    [HttpGet("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> Get(Guid tenantId)
    {
      try
      {
        _logger.LogInformation("GET - Getting tenant with ID: {tenantId}", tenantId);
        var tenant = await _repo.GetTenantAccountByIdAsync(tenantId);

        if (tenant == null)
        {
          _logger.LogWarning("No tenant found for given ID: {tenantId} on GET call.", tenantId);
          return NotFound();
        }

        return Ok(tenant);
      }
      catch (Exception e)
      {
        _logger.LogError("Exception getting tenant: {exceptionMessage}, {exception}", e.Message, e);
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
      }
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


  }
}
