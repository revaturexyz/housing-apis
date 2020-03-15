using System;

namespace Revature.Identity.Lib.Model
{
  public class TenantMessage
  {
    /// <summary>
    /// Gets or sets the message we will receive from the tenant, which will be the TenantId, Email, and Name.
    /// </summary>
    public Guid TenantId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// Gets or sets operation type.
    /// Based on the operation type (0 = Create, 1 = Delete, 2 = Put), we will react accordingly in the ServiceBusConsumer.
    /// If 0 then we will create a tenant account with the given GUID; if 1 then we will delete the given tenant account.
    /// </summary>
    public OperationType OperationType { get; set; }
  }
}
