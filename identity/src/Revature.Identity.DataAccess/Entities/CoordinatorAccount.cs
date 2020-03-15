using System;
using System.Collections.Generic;

namespace Revature.Identity.DataAccess.Entities
{
  public class CoordinatorAccount
  {
    public CoordinatorAccount()
    {
      Notifications = new HashSet<Notification>();
    }

    public Guid CoordinatorId { get; set; }

    public string TrainingCenterName { get; set; }

    public string TrainingCenterAddress { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public virtual ICollection<ProviderAccount> Providers { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; }
  }
}
