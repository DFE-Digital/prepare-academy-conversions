﻿namespace Dfe.PrepareConversions.ViewModels;

public class SchoolOverviewViewModel
{
   public string Id { get; set; }
   public string SchoolPhase { get; set; }
   public string AgeRange { get; set; }
   public string SchoolType { get; set; }
   public string NumberOnRoll { get; set; }
   public string PercentageSchoolFull { get; set; }
   public string SchoolCapacity { get; set; }
   public string PublishedAdmissionNumber { get; set; }
   public decimal? NumberOfPlacesFundedFor { get; set; }
   public decimal? NumberOfResidentialPlaces { get; set; }
   public decimal? NumberOfFundedResidentialPlaces { get; set; }
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
   public string MemberOfParliamentNameAndParty { get; set; }
   public bool? SchoolOverviewSectionComplete { get; set; }
   public string PupilsAttendingGroup { get; set; }
   public bool IsSpecial { get; set; }
   public bool IsPRU { get; set; }
   public int? NumberOfAlternativeProvisionPlaces { get; set; }
   public int? NumberOfMedicalPlaces { get; set; }
   public int? NumberOfPost16Places { get; set; }
   public int? NumberOfSENUnitPlaces { get; set; }
   public bool IsReadOnly {  get; set; }
}
