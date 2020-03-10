using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Tenant.Api.ServiceBus
{
  public class IdentitySBStub : IIdentityService
  {
    public async Task CreateAccount(Guid TenantId, string Email, string Name)
    {
      await Task.Yield();
    }

    public async Task DeleteAccount(Guid TenantId, string Email, string Name)
    {
      await Task.Yield();
    }

    public async Task UpdateAccount(Guid TenantId, string Email, string Name)
    {
      await Task.Yield();
    }
  }
}
