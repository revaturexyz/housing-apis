using System;
using System.ComponentModel.DataAnnotations;

namespace Revature.Lodging.Lib.Models
{
  public class Room
  {
    /// <summary>
    /// Room Number of the room.
    /// </summary>
    private string _roomNumber;

    /// <summary>
    /// Number of beds in a Room, can also be interpreted as Room's full capacity.
    /// </summary>
    private int _numberOfBeds;

    /// <summary>
    /// Number of occupants in a room.
    /// </summary>
    private int _numberOfOccupants;

    /// <summary>
    /// room type.
    /// </summary>
    private string _roomType;

    /// <summary>
    /// Gets or sets unique identifier for each Room, assigned by this service.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the room number, another way to uniquely identify a Room.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when room number has null or no value.</exception>
    public string RoomNumber
    {
      get => _roomNumber;
      set
      {
        if (string.IsNullOrWhiteSpace(value))
        {
          throw new ArgumentException("Room Number should have a value");
        }

        _roomNumber = value;
      }
    }

    /// <summary>
    /// Gets or sets the number of beds in the room; must be greater than zero.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when given an invalid number of beds.</exception>
    [Required]
    public int NumberOfBeds
    {
      get => _numberOfBeds;
      set
      {
        if (value > 0) _numberOfBeds = value;
        else throw new ArgumentException("Number of beds must be greater than zero");
      }
    }

    /// <summary>
    /// Gets or sets number of occupants per Room, used to check for Room vacancy, updated whenever a tenant is assigned or leaves a Room.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when occupants are greater than number number of beds.</exception>
    [Required]
    public int NumberOfOccupants
    {
      get => _numberOfOccupants;
      set
      {
        if (value > _numberOfBeds || value < 0) throw new ArgumentException("Number of Occupants must be less than number of beds");
        else _numberOfOccupants = value;
      }
    }

    /// <summary>
    /// Gets date for the start of the lease.
    /// </summary>
    public DateTime LeaseStart { get; private set; }

    /// <summary>
    /// Gets date for end of lease.
    /// </summary>
    public DateTime LeaseEnd { get; private set; }

    /// <summary>
    /// Gets or sets complex where Room belongs in.
    /// </summary>
    [Required]
    public Guid ComplexId { get; set; }

    /// <summary>
    /// Gets or sets gender of the Room, when assigning a tenant to a Room, their roommates should be of the same gender.
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    /// Gets or sets type of Room, for example: apartment, dorm, townhouse, hotel/motel.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the room type is null or has no value just whitespace.</exception>
    public string RoomType
    {
      get => _roomType;
      set
      {
        if (string.IsNullOrWhiteSpace(value))
        {
          throw new ArgumentException("Room Type must have a value");
        }

        _roomType = value;
      }
    }

    /// <summary>
    /// Method that sets the lease of the room.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when lease period is invalid, i.e the lease ends before it even begins.</exception>
    public void SetLease(DateTime start, DateTime end)
    {
      if (start == null || end == null)
      {
        return;
      }

      if (start.CompareTo(end) >= 0)
      {
        throw new ArgumentException("Lease should start before it ends");
      }

      LeaseEnd = end;
      LeaseStart = start;
    }
  }
}
