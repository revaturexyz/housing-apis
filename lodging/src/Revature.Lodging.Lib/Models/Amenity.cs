using System;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.Lib.Models
{
  /// <summary>
  /// This model represents an amenity.
  /// AmenityRoom and AmenityComplex show the sets of amenities that rooms and complexes have.
  /// It only has a type and description.
  /// </summary>
  public class Amenity
  {
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string AmenityType { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }
  }
}
