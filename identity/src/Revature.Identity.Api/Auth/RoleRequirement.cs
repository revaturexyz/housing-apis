using Microsoft.AspNetCore.Authorization;

namespace Revature.Identity.Api
{
  /// <summary>
  /// Simple requirement which takes a role.
  /// </summary>
  public class RoleRequirement : IAuthorizationRequirement
  {
    public RoleRequirement(string role)
    {
      Role = role;
    }

    public string Role { get; }
  }
}
