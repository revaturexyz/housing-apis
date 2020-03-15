using System;
using System.Threading.Tasks;

namespace Revature.Tenant.Api.ServiceBus
{
  /// <summary>
  /// Service that communicates with the Identity service to create and delete tenant accounts.
  /// </summary>
  public interface IIdentityService
  {
    /// <summary>
    /// Method that deletes tenant accounts.
    /// </summary>
    /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAccount(Guid tenantId, string email, string name);

    /// <summary>
    /// Method that creates new tenant accounts.
    /// </summary>
    /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CreateAccount(Guid tenantId, string email, string name);

    /// <summary>
    /// Method that modifies existing tenant accounts.
    /// </summary>
    /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAccount(Guid tenantId, string email, string name);
  }
}
