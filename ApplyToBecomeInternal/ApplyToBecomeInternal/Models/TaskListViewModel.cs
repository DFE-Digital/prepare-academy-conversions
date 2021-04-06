using ApplyToBecome.Data.Models;

namespace ApplyToBecomeInternal.Models
{
	public class TaskListViewModel
	{
		public TaskListViewModel(Project project)
		{
			SchoolName = project.School.Name;
			SchoolURN = project.School.URN;
		}

		public string SchoolName { get; }
		public string SchoolURN { get; }
	}
}
