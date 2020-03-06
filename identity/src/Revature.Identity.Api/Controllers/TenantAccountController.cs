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
  [Route("api/tenant-accounts")]
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

    //This logic should not be exposed, and is instead implememted in the service bus
    //POST: api/tenant-accounts/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="Email"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    //public async Task<ActionResult> Post([FromBody, Bind("TenantId, Name, Email")] TenantAccount tenant)
    //{
    //    try
    //  {
    //    _logger.LogInformation("Post- Creating tenant with ID: {tenant.tenantI}, Name: {tenant.Name}, Email: {tenant.Email}", tenant.TenantId, tenant.Name, tenant.Email);
    //    _repo.AddTenantAccount(tenant);
    //    await _repo.SaveAsync();
    //
    //    return Ok(tenant);
    //  }
    //  catch (Exception e)
    //  {
    //    _logger.LogError("Exception getting tenant: {exceptionMessage}, {exception}", e.Message, e);
    //    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    //  }
    //}


    // GET: api/tenant-accounts/1a5bae53-cffa-472f-af41-fae9904b9db0
    [HttpGet("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator,Tenant")]
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

    // GET: api/tenant-accounts/john@gmail.com
    /// <summary>
    /// return a tenant based on an email.
    /// </summary>
    /// <param name="tenantEmail"></param>
    /// <returns></returns>
    [HttpGet("{tentantEmail}", Name = "GetTenantByEmail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator,Tenant")]
    public async Task<ActionResult> Get(string tenantEmail)
    {
      _logger.LogInformation($"GET - Getting tenant id by Email: {tenantEmail}");
      TenantAccount tenant;
      var id = await _repo.GetTenantIdByEmailAsync(tenantEmail);
      if (id == null)
      {
        _logger.LogWarning($"No tenant account found for {tenantEmail}");
        return NotFound();
      }
      else
      {
        tenant = await _repo.GetTenantAccountByIdAsync(id);
      }
      return Ok(tenant);
    }

    // PUT: api/tenant-accounts/1a5bae53-cffa-472f-af41-fae9904b9db0
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    [HttpPut("{tenantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Coordinator")]
    public async Task<IActionResult> Put(Guid tenantId, [FromBody, Bind("Name, Email")] TenantAccount tenant)
    {
      _logger.LogInformation($"PUT - Put request for tenant ID: {tenantId}");
      var existingTenant = await _repo.GetTenantAccountByIdAsync(tenantId);
      if (existingTenant != null)
      {
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

    //This logic should not be exposed, and is instead implememted in the service bus
    //// DELETE: api/tenant-accounts/1a5bae53-cffa-472f-af41-fae9904b9db0
    //[HttpDelete("{tenantId}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[Authorize]
    //public async Task<ActionResult> Delete(Guid tenantId)
    //{
    //  _logger.LogInformation($"DELETE - Delete request for tenant ID: {tenantId}");
    //  var existingProvider = await _repo.GetTenantAccountByIdAsync(tenantId);
    //  if (existingProvider != null)
    //  {
    //
    //    await _repo.DeleteTenantAccountAsync(tenantId);
    //    await _repo.SaveAsync();
    //    _logger.LogInformation($"Delete request persisted for {tenantId}");
    //    return NoContent();
    //  }
    //  _logger.LogWarning($"Delete request failed for {tenantId}");
    //  return NotFound();
    //}
  }
}
