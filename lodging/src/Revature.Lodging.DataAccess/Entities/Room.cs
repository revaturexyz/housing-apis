using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.DataAccess.Entities
{
  public class Room
  {
    public Guid Id { get; set; }

    public string RoomNumber { get; set; }

    [Range(1, int.MaxValue)]
    public int NumberOfBeds { get; set; }

    // Updated by tenant service.
    public int NumberOfOccupants { get; set; }

    public DateTime LeaseStart { get; set; }

    public DateTime LeaseEnd { get; set; }

    public Guid ComplexId { get; set; }

    public int? GenderId { get; set; }

    public int RoomTypeId { get; set; }

    public virtual Complex Complex { get; set; }

    public virtual Gender Gender { get; set; }

    public virtual RoomType RoomType { get; set; }

    /// <summary>
    /// Gets or sets the collection of the room's amenities.
    /// </summary>
    public IEnumerable<RoomAmenity> RoomAmenity { get; set; }
  }
}
