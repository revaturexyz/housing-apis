using System;

namespace Revature.Lodging.Api.Models
{
  /// <summary>
  /// Api Address model. Use it as parameter from front-end (inside Api Complex model)
  /// and/or as return type to front-end and Address service
  /// </summary>
  public class ApiComplexAddress
  {
    public Guid AddressId { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
  }
}
