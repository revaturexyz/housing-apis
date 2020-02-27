using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Revature.Lodging.Lib.Models
{
  public class RoomType
  {
    [Required]
    public int RoomTypeId { get; set; }
    [Required]
    public string Type { get; set; }
  }
}
