using System;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.Lib.Models
{
  /// <summary>
  /// This model serves to connect the Amenity with the Room that has that amenity. Both are FK's
  /// </summary>
  public class AmenityRoom
  {
    [Required]
    public Guid AmenityRoomId { get; set; }

    [Required]
    public Guid AmenityId { get; set; }

    [Required]
    public Guid RoomId { get; set; }
  }
}
