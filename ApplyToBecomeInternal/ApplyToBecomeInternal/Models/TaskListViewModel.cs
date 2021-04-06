using ApplyToBecome.Data.Models;

namespace ApplyToBecomeInternal.Models
{
	public class TaskListViewModel
	{
		public TaskListViewModel(Project project)
		{
			SchoolName = project.School.Name;
		}

		public string SchoolName { get; }
	}
}
