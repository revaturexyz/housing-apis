using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Revature.Lodging.Lib.Models
{
  public class Gender
  {
    [Required]
    public int GenderId { get; set; }
    [Required]
    public string Type { get; set; }
  }
}
