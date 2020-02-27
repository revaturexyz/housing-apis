using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Revature.Lodging.Lib.Models
{
  public class AmenityFloorPlan
  {
    [Required]
    public Guid AmenityFloorPlanId { get; set; }
    [Required]
    public Guid AmenityId { get; set; }
    [Required]
    public Guid FloorPlanId { get; set; }
  }
}
