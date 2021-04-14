using ApplyToBecomeInternal.Models.Shared;

namespace ApplyToBecomeInternal.Models
{
	public class ProjectNotesViewModel
	{

		public ProjectNotesViewModel(ProjectViewModel project)
		{
			Project = project;
			SubMenu = new SubMenuViewModel(project.Id, SubMenuPage.ProjectNotes);
			Navigation = new NavigationViewModel(NavigationContent.ProjectsList);
		}

		public ProjectViewModel Project { get; }
		public SubMenuViewModel SubMenu { get; }
		public NavigationViewModel Navigation { get; set; }
	}
}