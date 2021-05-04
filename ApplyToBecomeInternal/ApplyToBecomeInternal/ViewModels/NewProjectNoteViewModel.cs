using ApplyToBecomeInternal.Models.Navigation;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.ViewModels
{
	public class NewProjectNoteViewModel
	{
		public NewProjectNoteViewModel(ProjectViewModel project)
		{
			Project = project;
			var templateData = new[] {new KeyValuePair<string, string>("id", project.Id)};
			Navigation = new NavigationViewModel(NavigationTarget.ProjectNotes, templateData);
		}
		public ProjectViewModel Project { get; }
		public NavigationViewModel Navigation { get; }

	}
}
