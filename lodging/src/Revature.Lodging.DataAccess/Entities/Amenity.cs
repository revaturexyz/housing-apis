using System;
using System.Collections.Generic;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity Amenity model. Repository use it to CRUD amenity data from database
  /// </summary>
  public class Amenity
  {

    public Guid Id { get; set; }
    public string AmenityType { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// it is for amenity Id to behave as FK in AmenityRoom table
    /// </summary>
    public virtual ICollection<RoomAmenity> RoomAmenity { get; set; }

    /// <summary>
    /// It is for complex Id behave as FK in AmenityRoom table
    /// </summary>
    public virtual ICollection<ComplexAmenity> ComplexAmenity { get; set; }
  }
}
