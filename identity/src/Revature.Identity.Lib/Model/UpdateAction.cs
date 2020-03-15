using System;

namespace Revature.Identity.Lib.Model
{
  public class UpdateAction
  {
    public Guid UpdateActionId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets string title of intended action, i.e. "UpdateOccupiedRoom".
    /// </summary>
    public string UpdateType { get; set; }

    /// <summary>
    /// Gets or sets foreign key of the notification this action belongs to.
    /// </summary>
    public Guid NotificationId { get; set; }

    /// <summary>
    /// Gets or sets the serialized object to be used by the action.
    /// </summary>
    public string SerializedTarget { get; set; }
  }
}
