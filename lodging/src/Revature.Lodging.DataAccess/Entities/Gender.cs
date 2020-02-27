using System.Collections.Generic;
namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity for the Gender Types i.e Male, Female
  /// </summary>
  public class Gender
  {
    public int GenderID { get; set; }
    public string Type { get; set; }
    public ICollection<Room> Room { get; set; }
  }
}
