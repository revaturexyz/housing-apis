using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Models
{
  public class Amenity
  {
    public Guid Id { get; set; }

    public string AmenityType { get; set; }

    public string Description { get; set; }
  }
}
