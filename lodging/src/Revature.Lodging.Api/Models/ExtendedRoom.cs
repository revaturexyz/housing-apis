using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revature.Lodging.Api.Models
{
  public class ExtendedRoom
  {
    public Guid RoomId { get; set; }

    public string RoomNumber { get; set; }

    public int NumberBeds { get; set; }

    public int NumberOccupants { get; set; }

    public long FloorPlanId { get; set; }

    public Gender Gender { get; set; }

    public RoomType RoomType { get; set; }

    public DateTime LeaseStart { get; set; }

    public DateTime LeaseEnd { get; set; }

    public IEnumerable<Amenity> Amenities { get; set; }
  }
}
