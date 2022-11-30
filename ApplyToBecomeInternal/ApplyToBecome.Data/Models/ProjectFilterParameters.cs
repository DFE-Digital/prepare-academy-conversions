using System.Collections.Generic;

namespace ApplyToBecome.Data.Models
{
	public class ProjectFilterParameters
	{
		public List<string> Statuses { get; set; }
		public List<string> AssignedUsers { get; set; }
		public List<string> Regions { get; set; }
	}
}
