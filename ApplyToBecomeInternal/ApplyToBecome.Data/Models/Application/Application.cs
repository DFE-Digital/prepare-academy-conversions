namespace ApplyToBecome.Data.Models.Application
{
	/// <remarks>
	/// Whole model and related models can probably be done away with once this is built e2e
	/// </remarks>
	public class Application
	{
		public string SchoolName { get; set; }
		public string TrustName { get; set; }
		public string LeadApplicant { get; set; }
		public ApplicationDetails Details { get; set; }
		public ConversionInformation ConversionInformation { get; set; }
		public FurtherInformation FurtherInformation { get; set; }
		public bool HasGovernmentConsultedStakeholders { get; set; }
		public Finances Finances { get; set; }
		public LandAndBuildings LandAndBuildings { get; set; }
		public string FundsPaidToSchoolOrTrust { get; set; }
		public FuturePupilNumbers FuturePupilNumbers { get; set; }
		public Declaration Declaration { get; set; }
	}
}