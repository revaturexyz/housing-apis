using System;
using System.Collections.Generic;

namespace Revature.Tenant.Lib.Models
{
  /// <summary>
  /// Class that contains necessary information about the room and its residents.
  /// </summary>
  /// <exception cref="ArgumentException">Thrown when number of beds is not positive or if room id is empty.</exception>
  public class RoomInfo
  {
    /// <summary>
    /// RoomId of room from Room Service.
    /// </summary>
    private Guid _roomId;

    /// <summary>
    /// Full capacity of room.
    /// </summary>
    private int _numberOfBeds;

    /// <summary>
    /// Gets or sets room ID.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when room ID is empty.</exception>
    public Guid RoomId
    {
      get => _roomId;
      set
      {
        if (value == Guid.Empty) throw new ArgumentException("Room ID can't be empty");
        _roomId = value;
      }
    }

    /// <summary>
    /// Gets or sets full capacity of room, checks if the full capacity is positive.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the number of beds is not positive.</exception>
    public int NumberOfBeds
    {
      get => _numberOfBeds;
      set
      {
        if (value < 1) throw new ArgumentException("Number of beds must be greater than 0");
        _numberOfBeds = value;
      }
    }

    /// <summary>
    /// Gets or sets list of tenants and associated information of room occupants.
    /// </summary>
    public List<Tenant> Tenants { get; set; }
  }
}
