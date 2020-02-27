using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Identity.DataAccess.Entities
{

  public class CoordinatorAccount

  {

    public CoordinatorAccount()

    {

      Notifications = new HashSet<Notification>();
      Providers = new HashSet<ProviderAccount>();

    }



    public Guid Id { get; set; }

    public string TrainingCenterName { get; set; }

    public string TrainingCenterAddress { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public virtual ICollection<Entities.ProviderAccount> Providers { get; set; }

    public virtual ICollection<Entities.Notification> Notifications { get; set; }

  }
}
