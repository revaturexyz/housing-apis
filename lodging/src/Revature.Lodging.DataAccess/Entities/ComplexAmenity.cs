using System;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity AmenityComplex model. Repository use it to CRUD amenity of complex data from database.
  /// </summary>
  public class ComplexAmenity
  {
    public Guid Id { get; set; }

    public Guid AmenityId { get; set; }

    public Guid ComplexId { get; set; }

    public virtual Complex Complex { get; set; }

    public virtual Amenity Amenity { get; set; }
  }
}
