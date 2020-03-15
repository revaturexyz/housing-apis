using System;

namespace Revature.Identity.Lib.Model
{
  /// <summary>
  /// Contains information pertaining to a particular provider which
  /// owns one or more complexes that are providing housing to
  /// a single training center.
  /// </summary>
  public class ProviderAccount
  {
    private string _name;
    private string _email;

    public Guid ProviderId { get; set; } = Guid.NewGuid();

    public Guid? CoordinatorId { get; set; }

    public string Name
    {
      get => _name;
      set
      {
        NotNullOrEmptyorWhitespaces(value);
        _name = value;
      }
    }

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
    /// Gets or sets the current status of a provider's account.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets or sets date and time the account was created at, expressed in the format 11:59:59.
    /// </summary>
    public DateTime AccountCreatedAt { get; set; }

    /// <summary>
    /// Gets or sets date and time the account expires at.
    /// </summary>
    public DateTime AccountExpiresAt { get; set; }

    /// <summary>
    /// Checks to see if a string is either null (does not exist) or empty ( "" ) or whitespaces ("  ").
    /// </summary>
    private static void NotNullOrEmptyorWhitespaces(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        if (value == null)
        {
          throw new ArgumentNullException(nameof(value), "Your Input cannot be null");
        }
        else
        {
          throw new ArgumentException("Your Input cannot be empty string.", nameof(value));
        }
      }
    }
  }
}
