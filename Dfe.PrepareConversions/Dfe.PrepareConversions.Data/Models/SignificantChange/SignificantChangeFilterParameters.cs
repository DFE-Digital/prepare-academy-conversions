
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeFilterParameters
{
   public List<FilterValueDisplay> Statuses { get; set; } = new();
   public List<FilterValueDisplay> AssignedUsers { get; set; } = new();
   public List<FilterValueDisplay> Tiers { get; set; } = new();
   public List<FilterValueDisplay> Routes { get; set; } = new();
}