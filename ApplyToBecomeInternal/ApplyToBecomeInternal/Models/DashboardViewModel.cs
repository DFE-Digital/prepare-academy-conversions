using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Models
{
	public class DashboardViewModel
	{
		public DashboardViewModel(IEnumerable<Project> projects)
		{
			Projects = projects;
		}

		public IEnumerable<Project> Projects { get; }
		public int ProjectCount => Projects.Count();
	}
}
