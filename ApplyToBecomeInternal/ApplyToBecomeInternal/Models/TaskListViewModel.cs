using ApplyToBecomeInternal.Models.Shared;

namespace ApplyToBecomeInternal.Models
{
	public class TaskListViewModel
	{
		public TaskListViewModel(ProjectViewModel project)
		{
			Project = project;
			SubMenu = new SubMenuViewModel(project.Id, SubMenuPage.TaskList);
			Navigation = new NavigationViewModel(NavigationContent.ProjectsList);
		}

		public ProjectViewModel Project { get; }
		public SubMenuViewModel SubMenu { get; }
		public NavigationViewModel Navigation { get; set; }

	}
}