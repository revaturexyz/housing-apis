using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Tenant.Api.Models
{
  public class TenantAccountMessage
  {
    public Guid TenantId;
    public string Name;
    public string Email;
    public int OperationType;
  }
}
