using System;

namespace Revature.Tenant.Lib.Models
{
  /// <summary>
  /// Model for the response recieved from room service of available rooms, needed for the buggy json deserialization.
  /// </summary>
  /// <remarks>The room service sends a tuple and there were some issues with deserializing a tuple thus creating this model was necessary.</remarks>
  public class AvailRoom
  {
    /// <summary>
    /// Gets or sets room Id.
    /// </summary>
    public Guid item1 { get; set; }

    /// <summary>
    /// Gets or sets number of beds.
    /// </summary>
    public int item2 { get; set; }
  }
}
