using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans
{
	public enum SchoolImprovementPlanExpectedEndDate
	{
		[Description("To Advisory Board")]
		ToAdvisoryBoard = 1,
		[Description("To Conversion")]
		ToConversion = 2,
		[Description("Unknown")]
		Unknown = 3,
		[Description("Other")]
		Other = 99
	}
}
