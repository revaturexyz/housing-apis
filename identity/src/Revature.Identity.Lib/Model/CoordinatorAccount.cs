using System;
using System.Collections.Generic;

namespace Revature.Identity.Lib.Model
{
  /// <summary>
  /// Contains individual information for a coordinator tied to a single training center.
  /// </summary>
  public class CoordinatorAccount
  {
    private string _email;
    private string _name;
    private string _trainingCenterName;
    private string _trainingCenterAddress;

    /// <summary>
    /// Gets or sets gUID based ID for the managing coordinator who manages this message.
    /// </summary>
    public Guid CoordinatorId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets a list of notifications associated with a given coordinator.
    /// </summary>
    public virtual List<Notification> Notifications { get; set; } = new List<Notification>();

    /// <summary>
    /// Gets or sets coordinator's full name.
    /// </summary>
    public string Name
    {
      get => _name;
      set
      {
        NotNullOrEmpty(value);
        _name = value;
      }
    }

    /// <summary>
    /// Gets or sets coordinator's valid email.
    /// </summary>
    public string Email
    {
      get => _email;
      set
      {
        // This line simply uses the instantiation of the MailAddress object
        // to check if the email is valid. Object is thrown away.
        _ = new System.Net.Mail.MailAddress(value);
        _email = value;
      }
    }

    /// <summary>
    /// Gets or sets name of the training center associated with the coordinator.
    /// </summary>
    public string TrainingCenterName
    {
      get => _trainingCenterName;
      set
      {
        NotNullOrEmpty(value);
        _trainingCenterName = value;
      }
    }

    /// <summary>
    /// Gets or sets address of the training center associated with the coordinator.
    /// </summary>
    public string TrainingCenterAddress
    {
      get => _trainingCenterAddress;
      set
      {
        NotNullOrEmpty(value);
        _trainingCenterAddress = value;
      }
    }

    /// <summary>
    /// Checks to see if a string is either null (does not exist) or empty ( "" ).
    /// </summary>
    private static void NotNullOrEmpty(string value)
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(value), "Your input cannot be null");
      }

      if (value.Length == 0)
      {
        throw new ArgumentException("Your input cannot be empty string.", nameof(value));
      }
    }
  }
}
