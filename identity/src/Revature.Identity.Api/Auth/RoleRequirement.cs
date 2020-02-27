using Microsoft.AspNetCore.Authorization;

namespace Revature.Identity.Api.Auth
{
  /// <summary>
  /// Simple requirement which takes a role.
  /// </summary>
  public class RoleRequirement : IAuthorizationRequirement
  {
    public string Role { get; }

    public RoleRequirement(string role)
    {
      Role = role;
    }
  }
}
