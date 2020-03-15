using System.Collections.Generic;

namespace Revature.Lodging.DataAccess.Entities
{
  public class Gender
  {
    public int Id { get; set; }

    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the collection of the rooms assigned to this gender.
    /// </summary>
    public virtual ICollection<Room> Room { get; set; }
  }
}
