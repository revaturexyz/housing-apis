using System;
using System.Threading.Tasks;

namespace Revature.Tenant.Api.ServiceBus
{
  public interface IIdentityService
  {
    /// <summary>
    /// Service that communicates with the Identity service to create and delete tenant accounts
    /// </summary>
    /// <summary>
    /// Method that deletes tenant accounts
    /// </summary>
    /// <param name="TenantId"></param>
    /// <param name="Email"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful</exception>
    Task DeleteAccount(Guid TenantId, string Email, string Name);
    /// <summary>
    /// Method that creates new tenant accounts
    /// </summary>
    /// <param name="TenantId"></param>
    /// <param name="Email"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful</exception>
    Task CreateAccount(Guid TenantId, string Email, string Name);
    /// <summary>
    /// Method that modifies existing tenant accounts
    /// </summary>
    /// <param name="TenantId"></param>
    /// <param name="Email"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful</exception>
    Task UpdateAccount(Guid TenantId, string Email, string Name);
  }
}

