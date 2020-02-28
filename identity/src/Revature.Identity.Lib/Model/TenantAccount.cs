using System;
using System.ComponentModel.DataAnnotations;

namespace Revature.Account.Lib.Model
{
  public class TenantAccount
  {
    /// <summary>
    /// Tenant Name: Required to be a non-empty string
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    /// <summary>
    /// New Tenant Id created Type (Guid)
    /// </summary>
    public Guid TenantId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Provider Id associated with Tenant type (Guid?)
    /// </summary>
    public Guid? ProviderId { get; set; }

    /// <summary>
    /// Coordinator Id Associated with Tenant type (Guid?)
    /// </summary>
    public Guid? CoordinatorId { get; set; }

    /// <summary>
    /// Tenant Email: DataType.EmailAddress required
    /// </summary>
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    /// <summary>
    /// Status Associated with Tenant type (Status)
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Time Account was created type (DateTime)
    /// </summary>
    public DateTime AccountCreateAt { get; set; }

    /// <summary>
    /// Time Account is expected to Expire type (DateTime)
    /// </summary>
    public DateTime AccountExpiresAt { get; set; }

  }
}
