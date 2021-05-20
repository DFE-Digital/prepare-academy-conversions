using ApplyToBecome.Data.Models.ProjectNotes;
using ApplyToBecomeInternal.Models.Navigation;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ProjectNotesViewModel
	{

		public ProjectNotesViewModel(IEnumerable<ProjectNote> notes, ProjectViewModel project, bool newNote)
		{
			Notes = notes;
			Project = project;
			NewNote = newNote;
			SubMenu = new SubMenuViewModel(project.Id, SubMenuPage.ProjectNotes);
			Navigation = new NavigationViewModel(NavigationTarget.ProjectsList);
		}

		public IEnumerable<ProjectNote> Notes { get; }
		public ProjectViewModel Project { get; }
		public bool NewNote { get; }
		public SubMenuViewModel SubMenu { get; }
		public NavigationViewModel Navigation { get; }
	}
}