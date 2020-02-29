using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Models
{
  public class FloorPlan
  {
    public Guid FloorPlanId { get; set; }

    public Guid ComplexId { get; set; }

    public string FloorPlanName { get; set; }

    public int NumberBeds { get; set; }

    public int RoomType { get; set; }

    public IEnumerable<Amenity> Amenities { get; set; }
  }
}
