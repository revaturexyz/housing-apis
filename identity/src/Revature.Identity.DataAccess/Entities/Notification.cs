using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Identity.DataAccess.Entities
{
  public class Notification

  {

    public Guid Id { get; set; }

    /*
    public Guid ProviderId { get; set; }

    public Guid CoordinatorId { get; set; }

    public Guid UpdateActionId { get; set; }
    */

    public string StatusText { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CoordinatorAccount Coordinator { get; set; }

    public virtual ProviderAccount Provider { get; set; }

    public virtual UpdateAction UpdateAction { get; set; }

  }
}
