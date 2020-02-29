using System.Collections.Generic;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity for available Room types:
  /// 1 = Dorm, 2 = Apartment, 3 = Townhouse, 4= Hotel/Motel
  /// </summary>
  public class RoomType
  {
    public int RoomTypeId { get; set; }
    public string Type { get; set; }
    public ICollection<FloorPlan> FloorPlan { get; set; }
    public ICollection<Room> Room { get; set; }
  }
}
