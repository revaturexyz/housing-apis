using System;
using System.Collections.Generic;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity Complex model. Repository use it to CRUD complex data from database.
  /// </summary>
  public partial class Complex
  {
    public Guid Id { get; set; }

    public Guid AddressId { get; set; }

    public Guid ProviderId { get; set; }

    public string ComplexName { get; set; }

    public string ContactNumber { get; set; }

    /// <summary>
    /// The Complex model has a collection of AmenityComplex models that
    /// represent the amenities offered by the complex.
    /// </summary>
    public virtual ICollection<ComplexAmenity> ComplexAmenity { get; set; }

    /// <summary>
    /// The complex model has a collection of Room models that represent the rooms
    /// that a single complex contains.
    /// </summary>
    public virtual ICollection<Room> Room { get; set; }
  }
}
