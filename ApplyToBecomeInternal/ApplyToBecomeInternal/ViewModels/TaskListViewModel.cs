using ApplyToBecomeInternal.Models.Navigation;

namespace ApplyToBecomeInternal.ViewModels
{
	public class TaskListViewModel
	{
		public TaskListViewModel(ProjectViewModel project)
		{
			Project = project;
			SubMenu = new SubMenuViewModel(project.Id, SubMenuPage.TaskList);
			Navigation = new NavigationViewModel(NavigationTarget.ProjectsList);
		}

		public ProjectViewModel Project { get; }
		public SubMenuViewModel SubMenu { get; }
		public NavigationViewModel Navigation { get; set; }

	}
}