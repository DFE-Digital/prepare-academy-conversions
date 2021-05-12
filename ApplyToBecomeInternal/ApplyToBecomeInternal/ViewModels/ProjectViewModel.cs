using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.ProjectNotes;
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
			Notes = project.Notes;
		}


		private static string FormatDate(DateTime dateTime) => dateTime.ToString("dd MMMM yyyy");

		public string Id { get; }
		public string TrustName { get; }
		public string SchoolName { get; }
		public string SchoolURN { get; }
		public string LocalAuthority { get; }
		public string ApplicationReceivedDate { get; }
		public string AssignedDate { get; }
		public string Phase { get; }
		public List<ProjectNote> Notes { get; set; }
	}
}