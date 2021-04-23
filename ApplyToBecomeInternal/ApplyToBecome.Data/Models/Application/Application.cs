namespace ApplyToBecome.Data.Models.Application
{
	public class Application
	{
		public School School { get; set; }
		public Trust Trust { get; set; }
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