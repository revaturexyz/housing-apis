using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Identity.Lib.Model
{
  public class TenantAccount
  {

    private string _email;
    private string _name;


    /// <summary>
    /// Tenant Name: Required to be a non-empty string
    /// </summary>
    public string Name
    {
      get { return _name; }
      set
      {
        NotNullOrEmpty(value);
        _name = value;
      }
    }

    /// <summary>
    /// New Tenant Id created Type (Guid)
    /// </summary>
    public Guid TenantId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Tenant Email: DataType.EmailAddress required
    /// </summary>
    public string Email
    {
      get { return _email; }
      set
      {
        // This line simply uses the instantiation of the MailAddress object
        // to check if the email is valid. Object is thrown away.
        _ = new System.Net.Mail.MailAddress(value);
        _email = value;
      }
    }


    /// <summary>
    /// Checks to see if a string is either null (does not exist) or empty ( "" )
    /// </summary>
    /// <param name="value"></param>
    private static void NotNullOrEmpty(string value)
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(value), "Your Input cannot be null");
      }
      if (value.Length == 0)
      {
        throw new ArgumentException("Your Input cannot be empty string.", nameof(value));
      }
    }
  }
}

