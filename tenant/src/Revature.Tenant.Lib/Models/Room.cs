using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Tenant.Lib.Models
{
  public class Room
  {
    public Guid RoomId { get; set; }
    public string RoomNumber { get; set; }
    public Guid ComplexId { get; set; }
    public int NumberOfBeds { get; set; }
    public int NumberOfOccupants { get; set; }
    public string Gender { get; set; }
    public List<Amenity> Amenities { get; set; }
    public string ApiRoomType { get; set; }
    public DateTime LeaseStart { get; set; }
    public DateTime LeaseEnd { get; set; }

    public void SetLease(DateTime start, DateTime end)
    {
      if (start == null || end == null)
      {
        return;
      }

      if (start.CompareTo(end) >= 0)
      {
        throw new ArgumentException("Lease should start before it ends");
      }
      LeaseEnd = end;
      LeaseStart = start;
    }
  }
}
