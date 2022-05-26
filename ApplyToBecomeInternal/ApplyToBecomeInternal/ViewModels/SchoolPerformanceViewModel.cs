namespace ApplyToBecomeInternal.ViewModels
{
	public class SchoolPerformanceViewModel
	{
		public string Id { get; set; }
		public string DateOfLatestSection8Inspection { get; set; }
		public string InspectionEndDate { get; set; }
		public string PersonalDevelopment { get; set; }
		public string BehaviourAndAttitudes { get; set; }
		public string EarlyYearsProvision { get; set; }
		public string EffectivenessOfLeadershipAndManagement { get; set; }
		public string OverallEffectiveness { get; set; }
		public string QualityOfEducation { get; set; }
		public string SixthFormProvision { get; set; }
		public bool ShowAdditionalInformation { get; set; }
		public string AdditionalInformation { get; set; }
		public bool LatestInspectionIsSection8 { get; set; }
		public bool EarlyYearsProvisionApplicable { get; internal set; }
		public bool SixthFormProvisionApplicable { get; internal set; }
	}
}
