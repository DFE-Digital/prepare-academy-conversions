using ApplyToBecomeInternal.Models.Shared;

namespace ApplyToBecomeInternal.Models
{
	public class ApplicationFormViewModel
	{

		public ApplicationFormViewModel(ProjectViewModel project)
		{
			Project = project;
			SubMenu = new SubMenuViewModel(project.Id, SubMenuPage.ApplicationForm);
			Navigation = new NavigationViewModel(NavigationContent.ProjectsList);
		}

		public ProjectViewModel Project { get; }
		public SubMenuViewModel SubMenu { get; }
		public NavigationViewModel Navigation { get; set; }

	}
}