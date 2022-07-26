using System.ComponentModel;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision.Models
{
	public enum DecisionMadeBy
	{
		[Description("Regional Director for the region")]
		RegionalDirectorForRegion = 0,
		[Description("A different Regional Director")]
		OtherRegionalDirector = 1,
		[Description("Minister")]
		Minister = 2,
		[Description("Director General")]
		DirectorGeneral = 3,
		[Description("Other")]
		None = 4
	}
}
