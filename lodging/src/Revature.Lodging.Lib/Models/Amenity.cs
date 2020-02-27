using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.Lib.Models
{
  public class Amenity
  {
    [Required]
    public Guid AmenityId { get; set; }
    [Required, MaxLength(50)]
    public string AmenityType { get; set; }
    [MaxLength(50)]
    public string Description { get; set; }
  }
}
