using System;

namespace ApplyToBecome.Data.Models
{
	public class SchoolPerformance
	{
		public DateTime? DateOfLatestSection8Inspection { get; set; }
		// InspectionEndDate always refers to a full inspection.
		public DateTime? InspectionEndDate { get; set; }
		public string OverallEffectiveness { get; set; }

		public string QualityOfEducation { get; set; }
		public string BehaviourAndAttitudes { get; set; }
		public string PersonalDevelopment { get; set; }
		public string EffectivenessOfLeadershipAndManagement { get; set; }

		public string EarlyYearsProvision { get; set; }
		public string SixthFormProvision { get; set; }
	}
}