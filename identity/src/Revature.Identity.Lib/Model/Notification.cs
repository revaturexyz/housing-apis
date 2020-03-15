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
    /// Gets or sets GUID-based ID for the Notification.
    /// </summary>
    public Guid NotificationId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets GUID-based ID for a housing complex Provider.
    /// </summary>
    public Guid ProviderId { get; set; }

    /// <summary>
    /// Gets or sets GUID-based ID for a training center's Coordinator.
    /// </summary>
    public Guid CoordinatorId { get; set; }

    public Status Status { get; set; }

    public UpdateAction UpdateAction { get; set; }

    /// <summary>
    /// Gets or sets date and time the associated provider account expires at, if any.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
  }
}
