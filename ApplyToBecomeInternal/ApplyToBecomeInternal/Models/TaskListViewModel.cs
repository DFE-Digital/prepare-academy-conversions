using ApplyToBecomeInternal.Models.Shared;

namespace ApplyToBecomeInternal.Models
{
	public class TaskListViewModel
	{
		public TaskListViewModel(ProjectViewModel project)
		{
			Project = project;
			SubMenu = new SubMenuViewModel(project.Id, SubMenuPage.TaskList);
		}
		
		public ProjectViewModel Project { get; }
		public SubMenuViewModel SubMenu { get; }
	}
}