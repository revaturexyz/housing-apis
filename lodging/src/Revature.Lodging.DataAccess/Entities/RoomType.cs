using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Lodging.DataAccess.Entities
{
  public class RoomType
  {
    public int Id { get; set; }
    public string Type { get; set; }

    /// <summary>
    /// Model has a collection of Room that defines all the rooms with the specified room type.
    /// </summary>
    public virtual ICollection<Room> Room  { get; set; }
  }
}
