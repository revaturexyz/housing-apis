using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Identity.DataAccess.Entities
{
  public class ProviderAccount

  {

    public ProviderAccount()
    {
      Notifications = new HashSet<Notification>();
    }

    public Guid Id { get; set; }

   // public Guid? CoordinatorId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string StatusText { get; set; }

    public DateTime AccountCreatedAt { get; set; }

    public DateTime AccountExpiresAt { get; set; }

    public virtual CoordinatorAccount Coordinator { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; }

  }
}
