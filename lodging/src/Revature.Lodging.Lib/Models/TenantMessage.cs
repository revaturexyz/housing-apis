using System;

namespace Revature.Lodging.Lib.Models
{
  /// <summary>
  /// The message we will receive from the tenant.
  /// </summary>
  public class TenantMessage
  {
    public Guid RoomId { get; set; }

    public string Gender { get; set; }

    /// <summary>
    /// Gets or sets the operation type.
    /// </summary>
    /// <remarks>
    /// Based on the operation type ( 0 = Create, 1 = Delete ), we will react accordingly in the ServiceBusConsumer
    /// If 0 then we will add a occupant to the room, if 1 then we will remove a occupant from a room.
    /// </remarks>
    public OperationType OperationType { get; set; }
  }
}
