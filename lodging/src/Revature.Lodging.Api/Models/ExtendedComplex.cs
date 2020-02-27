using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Models
{
  public class ExtendedComplex
  {
    public int ComplexId { get; set; }

    public string ComplexName { get; set; }

    public string ContactNumber { get; set; }

    public Provider Provider { get; set; }

    public Address Address { get; set; }

    public IEnumerable<Amenity> Amenities { get; set; }
  }
}
