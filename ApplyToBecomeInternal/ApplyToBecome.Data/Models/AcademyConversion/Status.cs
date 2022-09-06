using System.ComponentModel;

namespace ApplyToBecome.Data.Models.AcademyConversion
{
	public enum Status
	{
		[Description("Not Started")] NotStarted = 0,
		[Description("In Progress")] InProgress = 1,
		Completed = 2
	}
}
