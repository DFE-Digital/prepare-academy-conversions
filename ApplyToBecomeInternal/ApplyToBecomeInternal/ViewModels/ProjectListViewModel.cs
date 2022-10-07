using ApplyToBecome.Data.Models;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ProjectListViewModel
	{
		public string Id { get; set; }
		public string NameOfTrust { get; set; }
		public string SchoolName { get; set; }
		public string SchoolURN { get; set; }
		public string LocalAuthority { get; set; }
		public string ApplicationReceivedDate { get; set; }
		public string AssignedDate { get; set; }
		public string HeadTeacherBoardDate { get; set; }
		public string ProposedAcademyOpeningDate { get; set; }
		public ProjectStatus Status { get; set; }
		public string AssignedUserFullName { get; set; }
	}

	public class ProjectStatus
	{				
		public ProjectStatus(string value, string colour)
		{
			Value = value;
			Colour = colour;	
		}
		public string Value { get; set; }
		public string Colour { get; set; }
	}
}
