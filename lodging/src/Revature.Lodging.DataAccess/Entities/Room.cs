using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Revature.Lodging.DataAccess.Entities
{
  public class Room
  {
    public Guid Id { get; set; }

    public string RoomNumber { get; set; }

    [Range(1, int.MaxValue)]
    public int NumberOfBeds { get; set; }

    /// <summary>
    /// Updated by tenant service
    /// </summary>
    public int NumberOfOccupants { get; set; }

    public Gender Gender { get; set; }

    public RoomType RoomType { get; set; }
    public DateTime LeaseStart { get; set; }
    public DateTime LeaseEnd { get; set; }

    public Guid ComplexId { get; set; }
  }
}
