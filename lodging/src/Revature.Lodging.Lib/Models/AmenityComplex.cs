using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.Lib.Models
{
  public class AmenityComplex
  {
    [Required]
    public Guid AmenityComplexId { get; set; }
    [Required]
    public Guid AmentityID { get; set; }
    [Required]
    public Guid ComplexId { get; set; }
  }
}
