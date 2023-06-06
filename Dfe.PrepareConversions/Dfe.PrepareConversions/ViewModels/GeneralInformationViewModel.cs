namespace Dfe.PrepareConversions.ViewModels;

public class GeneralInformationViewModel
{
   public string Id { get; set; }
   public string SchoolPhase { get; set; }
   public string AgeRange { get; set; }
   public string SchoolType { get; set; }
   public string NumberOnRoll { get; set; }
   public string PercentageSchoolFull { get; set; }
   public string SchoolCapacity { get; set; }
   public string PublishedAdmissionNumber { get; set; }
   public string PercentageFreeSchoolMeals { get; set; }
   public string PartOfPfiScheme { get; set; }
   public string PfiSchemeDetails { get; set; }
   public string ViabilityIssues { get; set; }
   public string FinancialDeficit { get; set; }
   public string IsSchoolLinkedToADiocese { get; set; }
   public decimal? PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; set; }
   public string DistanceFromSchoolToTrustHeadquarters { get; set; }
   public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
   public string ParliamentaryConstituency { get; set; }
   public string MemberOfParliamentName { get; set; }
   public string MemberOfParliamentParty { get; set; }
   public bool? GeneralInformationSectionComplete { get; set; }
}
