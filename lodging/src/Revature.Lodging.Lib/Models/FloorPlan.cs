using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace Revature.Lodging.Lib.Models
{
  public class FloorPlan
  {
    private int _numberBeds;

    [Required]
    public Guid FloorPlanId { get; set; }

    [Required]
    public string FloorPlanName { get; set; }

    [Required]
    public int NumberOfBeds
    {
       get => _numberBeds;
       set{
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
   [Required]
    public int RoomTypeId { get; set; }

    [Required]
    public Guid ComplexId { get; set; }

  }
}
