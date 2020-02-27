using System.Collections.Generic;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity for available Room types: Apartment, Dorm, House, etc
  /// </summary>
  public class RoomType
  {
    public int RoomTypeId { get; set; }
    public string Type { get; set; }
    public ICollection<FloorPlan> FloorPlan { get; set; }
    public ICollection<Room> Room { get; set; }
  }
}
