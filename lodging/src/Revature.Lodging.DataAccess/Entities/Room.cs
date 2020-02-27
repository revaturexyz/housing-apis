using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.DataAccess.Entities
{
  /// <summary>
  /// Entity for Room table, most attributes are assigned from the complex service except the number of occupants
  /// </summary>
  public class Room
  {
    public Guid RoomId { get; set; }

    public string RoomNumber { get; set; }

    [Range(1, int.MaxValue)]
    public int NumberOfBeds { get; set; }

    /// <summary>
    /// Updated by tenant service
    /// </summary>
    public int NumberOfOccupants { get; set; }

    public int GenderId { get; set; }
    public Gender Gender { get; set; }
    public int RoomTypeId { get; set; }
    public RoomType RoomType { get; set; }
    public Guid ComplexId { get; set; }
    public Complex Complex { get; set; }
    public int FloorPlanID { get; set; }
    public FloorPlan FloorPlan { get; set; }

    public DateTime LeaseStart { get; set; }
    public DateTime LeaseEnd { get; set; }

    public ICollection<AmenityRoom> AmenityRoom { get; set; }
  }
}
