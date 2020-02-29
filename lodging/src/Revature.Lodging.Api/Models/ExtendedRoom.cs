using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Models
{
  public class ExtendedRoom
  {
    public Guid ComplexId { get; set; }

    public string RoomNumber { get; set; }

    public int NumberBeds { get; set; }

    public int CurrentOccupants { get; set; }

    public Guid FloorPlanId { get; set; }

    public int Gender { get; set; }

    public int RoomType { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public IEnumerable<Amenity> Amenities { get; set; }
  }
}