using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models;

public class ProjectFilterParameters
{
   public List<string> Statuses { get; set; } = new();
   public List<string> AssignedUsers { get; set; } = new();
   public List<string> Regions { get; set; } = new();
}
