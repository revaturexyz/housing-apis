using System;

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
