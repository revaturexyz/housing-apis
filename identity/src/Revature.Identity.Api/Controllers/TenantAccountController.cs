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
  }
}
