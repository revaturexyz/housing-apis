using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Lodging.DataAccess.Entities
{
  public class Gender
  {
    public int Id { get; set; }
    public string Type { get; set; }

    /// <summary>
    /// Gender model has a collection of Room that stores the rooms assigned to the specified gender
    /// </summary>
    public virtual ICollection<Room> Room { get; set; }
  }
}
