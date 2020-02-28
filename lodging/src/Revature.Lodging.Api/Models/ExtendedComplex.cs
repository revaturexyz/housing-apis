using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Models
{
  public class ExtendedComplex
  {
    public Guid Id { get; set; }

    public string ComplexName { get; set; }

    public string ContactName { get; set; }

    public Provider Provider { get; set; }

    public Address Address { get; set; }

    public IEnumerable<Amenity> Amenities { get; set; }
  }
}
