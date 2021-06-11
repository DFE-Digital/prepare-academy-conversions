using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.GenerateHTBTemplate;
using System;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ProjectViewModel
	{
		private readonly Dictionary<ProjectPhase, string> _projectPhaseText = new Dictionary<ProjectPhase, string>
		{
			{ProjectPhase.PreHTB, "Pre HTB"},
			{ProjectPhase.PostHTB, "Post HTB"}
		};

		public ProjectViewModel(Project project)
		{
			Id = project.Id.ToString();
			TrustName = project.Trust.Name;
			SchoolName = project.School.Name;
			SchoolURN = project.School.URN;
			LocalAuthority = project.School.LocalAuthority;
			ApplicationReceivedDate = FormatDate(project.ApplicationReceivedDate);
			AssignedDate = FormatDate(project.AssignedDate);
			Phase = _projectPhaseText[project.Phase];
			ProjectDocuments = project.ProjectDocuments;
			RationaleForProject = project.Rationale.RationaleForProject;
			RationaleForTrust = project.Rationale.RationaleForTrust;
			RationaleMarkAsComplete = project.Rationale.RationaleMarkAsComplete;
			SetRationaleTaskListStatus();
		}

		private void SetRationaleTaskListStatus()
		{
			if (RationaleMarkAsComplete)
			{
				RationaleTaskListStatus = TaskListItemViewModel.Completed;
			}
			else if (string.IsNullOrWhiteSpace(RationaleForProject) && string.IsNullOrWhiteSpace(RationaleForProject))
			{
				RationaleTaskListStatus = TaskListItemViewModel.NotStarted;
			}
			else
			{
				RationaleTaskListStatus = TaskListItemViewModel.InProgress;
			}
		}


		private static string FormatDate(DateTime? dateTime) => dateTime.HasValue ? dateTime.Value.ToString("dd MMMM yyyy") : "";

		public string Id { get; }
		public string TrustName { get; }
		public string SchoolName { get; }
		public string SchoolURN { get; }
		public string LocalAuthority { get; }
		public string ApplicationReceivedDate { get; }
		public string AssignedDate { get; }
		public string Phase { get; }
		public IEnumerable<DocumentDetails> ProjectDocuments { get; set; }
		public string RationaleForProject { get; set; }
		public string RationaleForTrust { get; set; }
		public bool RationaleMarkAsComplete { get; set; }
		public TaskListItemViewModel RationaleTaskListStatus { get; set; }
	}
}
