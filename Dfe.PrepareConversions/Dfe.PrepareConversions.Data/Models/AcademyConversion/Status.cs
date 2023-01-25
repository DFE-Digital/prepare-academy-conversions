using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.AcademyConversion
{
	public enum Status
	{
		[Description("Not Started")] NotStarted = 0,
		[Description("In Progress")] InProgress = 1,
		Completed = 2
	}
}
