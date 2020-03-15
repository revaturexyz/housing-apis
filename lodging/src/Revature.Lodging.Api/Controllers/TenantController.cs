using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Lodging.Lib.Interface;

namespace Revature.Lodging.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TenantController : ControllerBase
  {
    private readonly IRoomRepository _repository;
    private readonly ILogger _logger;

    /// <summary>
    /// Controller in charge of communicating with the tenant service.
    /// </summary>
    public TenantController(IRoomRepository repository, ILogger<TenantController> logger)
    {
      _repository = repository ?? throw new NullReferenceException("Repository cannot be null." + nameof(repository));
      _logger = logger ?? throw new NullReferenceException("Logger cannot be null." + nameof(logger));
    }

    // GET: api/rooms?gender=g&endDate=e
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Coordinator")] // OktaSetup
    public async Task<IActionResult> GetAsync(
          [FromQuery] string gender,
          [FromQuery] DateTime endDate
          )
    {
      _logger.LogInformation("Getting vacant filtered rooms...");
      var result = await _repository.GetVacantFilteredRoomsByGenderandEndDateAsync(gender, endDate);
      _logger.LogInformation("Filtered rooms fetched.");
      return Ok(result);
    }
  }
}
