using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Models
{
  public class ExtendedRoom
  {
    public Guid Id { get; set; }

    public string RoomNumber { get; set; }

    public int NumberBeds { get; set; }

    public Guid FloorPlanId { get; set; }

    public string Gender { get; set; }

    public string RoomType { get; set; }

    public IEnumerable<Amenity> Amenities { get; set; }
  }
}
