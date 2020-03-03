using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public DateTime LeaseStart { get; set; }
    public DateTime LeaseEnd { get; set; }

    public Guid ComplexId { get; set; }

    public int? GenderId { get; set; }

    public int RoomTypeId { get; set; }

    /// <summary>
    /// for FK: complex Id
    /// </summary>
    public virtual Complex Complex { get; set; }

    /// <summary>
    /// for FK: gender Id
    /// </summary>
    public virtual Gender Gender { get; set; }

    /// <summary>
    /// for FK: room type Id
    /// </summary>
    public virtual RoomType RoomType { get; set; }

    /// <summary>
    /// The Room model has a collection of Amenity Rooms
    /// </summary>
    public IEnumerable<AmenityRoom> AmenityRoom { get; set; }

  }
}
