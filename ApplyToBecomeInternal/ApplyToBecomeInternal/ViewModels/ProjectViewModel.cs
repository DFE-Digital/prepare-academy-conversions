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

		public ProjectViewModel(AcademyConversionProject project)
		{
			Id = project.Id.ToString();
			//TrustName = project.Trust.Name;
			SchoolName = project.SchoolName;
			SchoolURN = project.Urn.ToString();
			LocalAuthority = project.LocalAuthority;
			ApplicationReceivedDate = FormatDate(project.ApplicationReceivedDate);
			AssignedDate = FormatDate(project.AssignedDate);
			Phase = project.ProjectStatus;
			//ProjectDocuments = project.ProjectDocuments;
			RationaleForProject = project.RationaleForProject;
			RationaleForTrust = project.RationaleForTrust;
			RationaleMarkAsComplete = project.RationaleMarkAsComplete ?? false;
			SetRationaleTaskListStatus();
		}

		private void SetRationaleTaskListStatus()
		{
			if (RationaleMarkAsComplete)
			{
				RationaleTaskListStatus = TaskListItemViewModel.Completed;
			}
			else if (string.IsNullOrWhiteSpace(RationaleForProject) && string.IsNullOrWhiteSpace(RationaleForTrust))
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
