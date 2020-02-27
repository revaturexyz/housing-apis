using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Identity.DataAccess.Entities
{

  public class UpdateAction

  {
    public UpdateAction()
    {
      Notifications = new HashSet<Notification>();
    }

    public Guid Id { get; set; }

   // public Guid NotificationId { get; set; }



    public string UpdateType { get; set; }



    public string SerializedTarget { get; set; }



    public virtual ICollection<Notification> Notifications { get; set; }



  }
}
