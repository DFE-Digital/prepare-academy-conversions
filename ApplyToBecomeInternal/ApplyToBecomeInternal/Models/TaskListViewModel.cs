using ApplyToBecome.Data.Models;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models
{
	public class TaskListViewModel
	{
		private readonly Dictionary<ProjectPhase, string> _projectPhaseText = new Dictionary<ProjectPhase, string> 
		{
			{ProjectPhase.PreHTB, "Pre HTB"},
			{ProjectPhase.PostHTB, "Post HTB"}
		};
		
		public TaskListViewModel(Project project)
		{
			SchoolName = project.School.Name;
			SchoolURN = project.School.URN;
			Phase = _projectPhaseText[project.Phase];
		}

		public string SchoolName { get; }
		public string SchoolURN { get; }
		public string Phase { get; }
	}
}
