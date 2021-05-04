using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ProjectListViewModel
	{
		public ProjectListViewModel(IEnumerable<Project> projects)
		{
			Projects = projects.Select(project => new ProjectViewModel(project));
		}

		public IEnumerable<ProjectViewModel> Projects { get; }
		public int ProjectCount => Projects.Count();
	}
}
