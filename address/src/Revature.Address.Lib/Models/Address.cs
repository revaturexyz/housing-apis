using System;
using System.Text.RegularExpressions;

namespace Revature.Address.Lib
{
  /// <summary>
  /// This is an address object for business logic purposes,
  /// specifies a GUID as well as a street,
  /// city, state, country, and ZIP code.
  /// </summary>
  public class Address
  {
    private string _street;

    private string _city;

    private string _state;

    private string _country;

    private string _zipCode;

    /// <summary>
    /// Gets or sets GUID for identifying addresses.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Gets or sets street. Specifies that a street must not be null and
    /// not whitespace or a series of special characters.
    /// </summary>
    public string Street
    {
      get => _street;
      set
      {
        if (value is null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        var trimmed = value.Trim('.', '+', '*', '\'', ' ', '%', '^', '&', '!', '@', '#', '$', '(', ')');
        if (trimmed.Length > 0)
        {
          _street = trimmed;
        }
        else
        {
          throw new ArgumentException($"\"{value}\" is not a valid street address", nameof(value));
        }
      }
    }

    /// <summary>
    /// Gets or sets city. Specifies that a city must not be null or whitespace.
    /// </summary>
    public string City
    {
      get => _city;
      set
      {
        if (!string.IsNullOrWhiteSpace(value))
        {
          _city = value.Trim();
        }
        else if (value is null)
        {
          throw new ArgumentNullException(nameof(value));
        }
        else
        {
          throw new ArgumentException($"Invalid value \"{value}\" City name cannot be whitespace.", nameof(value));
        }
      }
    }

    /// <summary>
    /// Gets or sets state. Specifies that a state must not be null and
    /// can only be a series of letters and spaces.
    /// </summary>
    public string State
    {
      get => _state;
      set
      {
        if (value != null && Regex.IsMatch(value, @"^[a-zA-Z\s]+$"))
        {
          _state = value;
        }
        else if (value is null)
        {
          throw new ArgumentNullException(nameof(value));
        }
        else
        {
          throw new ArgumentException(
            $"Invalid value \"{value}\": State name must be a string of only letters and spaces.", nameof(value));
        }
      }
    }

    /// <summary>
    /// Gets or sets country. Specifies that a country must not be null or whitespace.
    /// </summary>
    public string Country
    {
      get => _country;
      set
      {
        if (!string.IsNullOrWhiteSpace(value))
        {
          _country = value.Trim();
        }
        else if (value is null)
        {
          throw new ArgumentNullException(nameof(value));
        }
        else
        {
          throw new ArgumentException($"Invalid value \"{value}\" Country name cannot be whitespace.", nameof(value));
        }
      }
    }

    /// <summary>
    /// Gets or sets ZIP code. Specifies that a ZIP code must be a string of 5 integers.
    /// </summary>
    public string ZipCode
    {
      get => _zipCode;
      set
      {
        if (Regex.IsMatch(value, @"^[0-9]+$") && value.Length == 5)
        {
          _zipCode = value;
        }
        else
        {
          throw new ArgumentException($"\"{value}\" is not a string of 5 integers.", nameof(value));
        }
      }
    }
  }
}
