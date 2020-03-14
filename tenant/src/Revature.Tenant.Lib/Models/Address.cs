using System;

namespace Revature.Tenant.Lib.Models
{
  public class Address
  {
    public Guid AddressId { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }

    public string Country { get; set; }
  }
}
