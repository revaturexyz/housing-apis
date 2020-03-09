using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Tenant.Api.ServiceBus
{
  interface IIdentityService
  {
    /// <summary>
    /// Service that communicates with the Identity service to create and delete tenant accounts
    /// </summary>
    public interface IRoomService
    {
      /// <summary>
      /// Method that deletes tenant accounts
      /// </summary>
      /// <param name="TenantId"></param>
      /// <param name="Email"></param>
      /// <param name="Name"></param>
      /// <returns></returns>
      /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful</exception>
      void DeleteAccount(Guid TenantId, string Email, string Name, int OperationType);
      /// <summary>
      /// Method that creates new tenant accounts
      /// </summary>
      /// <param name="TenantId"></param>
      /// <param name="Email"></param>
      /// <param name="Name"></param>
      /// <returns></returns>
      /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful</exception>
      void CreateAccount(Guid TenantId, string Email, string Name, int OperationType);
      /// <summary>
      /// Method that modifies existing tenant accounts
      /// </summary>
      /// <param name="TenantId"></param>
      /// <param name="Email"></param>
      /// <param name="Name"></param>
      /// <returns></returns>
      /// <exception cref="HttpRequestException">Thrown when the response from the room service isn't successful</exception>
      void UpdateAccount(Guid TenantId, string Email, string Name, int OperationType);
    }
  }
}
