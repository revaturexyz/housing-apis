using System;

namespace Revature.Identity.DataAccess.Entities
{
  public class TenantAccount
  {
    public Guid TenantId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
  }
}
