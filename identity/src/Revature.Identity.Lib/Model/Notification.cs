using System;

namespace Revature.Identity.Lib.Model
{
  /// <summary>
  /// Encapsulates a notification a coordinator might receive regarding
  /// an action made by a provider.
  /// </summary>
  public class Notification
  {
    /// <summary>
    /// Guid based Id for the Notification.
    /// </summary>
    public Guid NotificationId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Guid based Id for a housing complex Provider.
    /// </summary>
    public Guid ProviderId { get; set; }

    /// <summary>
    /// Guid based Id for a training center's Coordinator.
    /// </summary>
    public Guid CoordinatorId { get; set; }

    public Status Status { get; set; }

    public UpdateAction UpdateAction { get; set; }

    /// <summary>
    /// Date and time the associated provider account expires at, if any, in the format 11:59:59.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
  }
}
