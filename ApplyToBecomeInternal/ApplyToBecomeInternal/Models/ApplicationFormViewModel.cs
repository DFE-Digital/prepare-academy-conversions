using ApplyToBecomeInternal.Models.Shared;

namespace ApplyToBecomeInternal.Models
{
	public class ApplicationFormViewModel
	{

		public ApplicationFormViewModel(ProjectViewModel project)
		{
			Project = project;
			SubMenu = new SubMenuViewModel(project.Id, SubMenuPage.ApplicationForm);
		}

		public ProjectViewModel Project { get; }
		public SubMenuViewModel SubMenu { get; }
	}
}