using System;
using System.Collections.Generic;
using System.Text;

namespace Revature.Lodging.Lib.Models
{
  public class Room
  {

    //private Stuff
    private string _roomNumber;
    private int _numberOccupants;
    private int _numberBeds;
    private RoomType _roomTypeId;
    //public stuff
    public Guid RoomId { get; set; }
    public string RoomNumber {
      get => _roomNumber;
      set
      {
        if (string.IsNullOrWhiteSpace(value))
        {
          throw new Exception("Room number must contain a value");
        }
        else
        {
          _roomNumber = value;
        }
      }
    }
    public int NumberBeds {
      get => _numberBeds;
      set
      {
        if (value > 0)
        {
          _numberBeds = value;
        }
        else
        {
          throw new ArgumentException("Number of beds must be greater than 0.");
        }
      }
    }
    public int NumberOccupants
    {
      get => _numberOccupants;
      set
      {
        if (value < 0 || value > _numberBeds)
        {
          throw new ArgumentException("Number of occupants must be greater than or equal to 0, as well as less than or equal to the number of beds.");
        }
        else
        {
          _numberOccupants = value;
        }
      }
    }
    public Gender Gender { get; set; }
    public DateTime LeaseStart { get; set; }
    public DateTime LeaseEnd { get; set; }

    public void SetLease (DateTime start, DateTime end)
    {
      if(start.CompareTo(end)>-0)
      {
        throw new ArgumentException("Invalid lease timeframe. The lease start time should begin before the lease end time.");
      }
      else
      {
        LeaseStart = start;
        LeaseEnd = end;
      }
      
    }

    public RoomType RoomTypeId {
      get => _roomTypeId;
      set
      {
        if (value != null)
        {
          _roomTypeId = value;
        }
        else
        {
          throw new ArgumentException("Room id should represent a valid room type.");
        }
      }
    }
    public Guid ComplexId{ get; set; }
    public int FloorPlanId { get; set; }
  }
}
