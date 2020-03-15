using System;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity AmenityRoom model. Repository use it to CRUD complex data from database.
  /// </summary>
  public class RoomAmenity
  {
    public Guid Id { get; set; }

    public Guid AmenityId { get; set; }

    public Guid RoomId { get; set; }

    public virtual Room Room { get; set; }

    public virtual Amenity Amenity { get; set; }
  }
}
