using System;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.Lib.Models
{
  /// <summary>
  /// This model serves to connect the Amenity with the Complex that has that amenity. Both FK's
  /// </summary>
  public class ComplexAmenity
  {
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid AmenityId { get; set; }

    [Required]
    public Guid ComplexId { get; set; }
  }
}
