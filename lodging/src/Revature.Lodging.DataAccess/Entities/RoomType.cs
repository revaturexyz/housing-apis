using System.Collections.Generic;

namespace Revature.Lodging.DataAccess.Entities
{
  public class RoomType
  {
    public int Id { get; set; }

    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the collection of all the rooms with the specified room type.
    /// </summary>
    public virtual ICollection<Room> Room { get; set; }
  }
}
