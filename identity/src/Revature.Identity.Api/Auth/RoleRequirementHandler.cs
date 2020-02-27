using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Revature.Identity.Api.Auth
{
  public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
      throw new System.NotImplementedException();
    }
  }
}
