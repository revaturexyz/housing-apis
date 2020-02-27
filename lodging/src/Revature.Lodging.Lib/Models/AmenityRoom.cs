using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.Lib.Models
{
  public class AmenityRoom
  {
    [Required]
    public Guid AmenityRoomId { get; set; }
    [Required]
    public Guid AmenityId{ get; set; }
    [Required]
    public Guid RoomId { get; set; }
  }
}
