using System;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity AmenityRoom model. Repository use it to CRUD complex data from database
  /// </summary>
  public class AmenityRoom
  {
    public Guid Id { get; set; }

    public Guid AmenityId { get; set; }

    public Guid RoomId { get; set; }


    /// <summary>
    /// for FK: amenity Id
    /// </summary>
    public Amenity Amenity { get; set; }
  }
}
