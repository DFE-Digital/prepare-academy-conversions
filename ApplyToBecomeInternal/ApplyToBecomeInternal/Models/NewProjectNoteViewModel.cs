using ApplyToBecomeInternal.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Models
{
	public class NewProjectNoteViewModel
	{
		public NewProjectNoteViewModel(ProjectViewModel project)
		{
			Project = project;
			Navigation = new NavigationViewModel(NavigationTarget.ProjectNotes, project.Id);
		}
		public ProjectViewModel Project { get; }
		public NavigationViewModel Navigation { get; set; }

	}
}
