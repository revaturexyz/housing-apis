using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity AmenityRoom model. Repository use it to CRUD complex data from database
  /// </summary>
  public class RoomAmenity
  {
    public Guid Id { get; set; }

    public Guid AmenityId { get; set; }

    public Guid RoomId { get; set; }

    /// <summary>
    /// for FK: room Id
    /// </summary>
    public virtual Room Room { get; set; }

    /// <summary>
    /// for FK: amenity Id
    /// </summary>
    public virtual Amenity Amenity { get; set; }
  }
}
