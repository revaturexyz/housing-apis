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
    /// Gets or sets the amenities offered by the complex.
    /// </summary>
    public virtual ICollection<ComplexAmenity> ComplexAmenity { get; set; }

    /// <summary>
    /// Gets or sets the collection of rooms that the complex contains.
    /// </summary>
    public virtual ICollection<Room> Room { get; set; }
  }
}
