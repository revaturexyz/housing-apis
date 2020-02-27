using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.Lib.Models
{
  public class Complex
  {
    [Required]
    public Guid ComplexId { get; set; }
    [Required]
    public Guid AddressId { get; set; }
    [Required]
    public Guid ProviderId { get; set; }
    [Required, MaxLength(50)]
    public string ComplexName { get; set; }
    [MaxLength(20)]
    public string ContactNumber { get; set; }
  }
}
