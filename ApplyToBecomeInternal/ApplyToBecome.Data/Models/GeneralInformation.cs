namespace ApplyToBecome.Data.Models
{
	public class GeneralInformation
	{
		public string SchoolPhase { get; set; }
		public string AgeRangeLower { get; set; }
		public string AgeRangeUpper { get; set; }
		public string SchoolType { get; set; }
		public int? NumberOnRoll { get; set; }
		public int? SchoolCapacity { get; set; }
		public string PublishedAdmissionNumber { get; set; }
		public string PercentageFreeSchoolMeals { get; set; }
		public string PartOfPfiScheme { get; set; }
		public string ViabilityIssues { get; set; }
		public string FinancialDeficit { get; set; }
		public string IsSchoolLinkedToADiocese { get; set; }
		public decimal? PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		public string ParliamentaryConstituency { get; set; }
		public bool? GeneralInformationSectionComplete { get; set; }
	}
}
