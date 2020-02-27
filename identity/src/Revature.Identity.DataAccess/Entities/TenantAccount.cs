using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Identity.DataAccess.Entities
{
  public class TenantAccount
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
  }
}
