using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models;

public class FormAMATProject
{
   public int Id { get; set; }
   public int? Urn { get; set; }
   public DateTime CreatedOn { get; set; }
   public string SchoolName { get; set; }
   public string SchoolPhase { get; set; }
   public string SchoolType { get; set; }
   public string LocalAuthority { get; set; }
   public string ApplicationReferenceNumber { get; set; }
   public string ProjectStatus { get; set; }
   public DateTime? ApplicationReceivedDate { get; set; }
   public DateTime? AssignedDate { get; set; }
   public DateTime? HeadTeacherBoardDate { get; set; }
   public DateTime? BaselineDate { get; set; }

   //la summary page
   public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }
   public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; set; }
   public string LocalAuthorityInformationTemplateComments { get; set; }
   public string LocalAuthorityInformationTemplateLink { get; set; }
   public bool? LocalAuthorityInformationTemplateSectionComplete { get; set; }

   //school/trust info
   public string RecommendationForProject { get; set; }
   public string Author { get; set; }
   public string Version { get; set; }
   public string ClearedBy { get; set; }
   public string AcademyOrderRequired { get; set; }
   public string PreviousHeadTeacherBoardDateQuestion { get; set; }
   public DateTime? PreviousHeadTeacherBoardDate { get; set; }
   public string PreviousHeadTeacherBoardLink { get; set; }
   public string TrustReferenceNumber { get; set; }
   public string NameOfTrust { get; set; }
   public string SponsorReferenceNumber { get; set; }
   public string SponsorName { get; set; }
   public string AcademyTypeAndRoute { get; set; }
   public string Form7Received { get; set; }
   public DateTime? Form7ReceivedDate { get; set; }
   public DateTime? ProposedAcademyOpeningDate { get; set; }
   public bool? SchoolAndTrustInformationSectionComplete { get; set; }
   public decimal? ConversionSupportGrantAmount { get; set; }
   public string ConversionSupportGrantChangeReason { get; set; }
   public string ConversionSupportGrantType { get; set; }
   public string ConversionSupportGrantEnvironmentalImprovementGrant { get; set; }
   public bool? ConversionSupportGrantAmountChanged { get; set; }
   public string ConversionSupportGrantNumberOfSites { get; set; }
   public DateTime? DaoPackSentDate { get; set; }
   public string Region { get; set; }

   // Annex B
   public bool? AnnexBFormReceived { get; set; }
   public string AnnexBFormUrl { get; set; }

   // External Application Form
   public bool? ExternalApplicationFormSaved { get; set; }
   public string? ExternalApplicationFormUrl { get; set; }

   // School Overview
   public string PublishedAdmissionNumber { get; set; }
   public string PartOfPfiScheme { get; set; }
   public string PfiSchemeDetails { get; set; }
   public string ViabilityIssues { get; set; }
   public decimal? NumberOfPlacesFundedFor { get; set; }
   public decimal? NumberOfResidentialPlaces { get; set; }
   public decimal? NumberOfFundedResidentialPlaces { get; set; }
   public string FinancialDeficit { get; set; }
   public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
   public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
   public string MemberOfParliamentNameAndParty { get; set; }
   public bool? SchoolOverviewSectionComplete { get; set; }
   public bool? PupilsAttendingGroupPermanentlyExcluded { get; set; }
   public bool? PupilsAttendingGroupMedicalAndHealthNeeds { get; set; }
   public bool? PupilsAttendingGroupTeenageMums { get; set; }
   public int? NumberOfAlternativeProvisionPlaces { get; set; }
   public int? NumberOfMedicalPlaces { get; set; }
   public int? NumberOfPost16Places { get; set; }
   public int? NumberOfSENUnitPlaces { get; set; }

   //school performance ofsted information
   public string SchoolPerformanceAdditionalInformation { get; set; }

   // rationale
   public string RationaleForProject { get; set; }
   public string RationaleForTrust { get; set; }
   public bool? RationaleSectionComplete { get; set; }

   // risk and issues
   public string RisksAndIssues { get; set; }
   public bool? RisksAndIssuesSectionComplete { get; set; }

   // legal requirements
   public string GoverningBodyResolution { get; set; }
   public string Consultation { get; set; }
   public string DiocesanConsent { get; set; }
   public string FoundationConsent { get; set; }
   public bool? LegalRequirementsSectionComplete { get; set; }

   // school budget info
   public DateTime? EndOfCurrentFinancialYear { get; set; }
   public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
   public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
   public DateTime? EndOfNextFinancialYear { get; set; }
   public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
   public decimal? CapitalCarryForwardAtEndMarchNextYear { get; set; }
   public string SchoolBudgetInformationAdditionalInformation { get; set; }
   public bool? SchoolBudgetInformationSectionComplete { get; set; }

   // pupil schools forecast
   public int? YearOneProjectedCapacity { get; set; }
   public int? YearOneProjectedPupilNumbers { get; set; }
   public int? YearTwoProjectedCapacity { get; set; }
   public int? YearTwoProjectedPupilNumbers { get; set; }
   public int? YearThreeProjectedCapacity { get; set; }
   public int? YearThreeProjectedPupilNumbers { get; set; }
   public int? YearFourProjectedCapacity { get; set; }
   public int? YearFourProjectedPupilNumbers { get; set; }
   public string SchoolPupilForecastsAdditionalInformation { get; set; }

   // key stage performance tables
   public string KeyStage2PerformanceAdditionalInformation { get; set; }
   public string KeyStage4PerformanceAdditionalInformation { get; set; }
   public string KeyStage5PerformanceAdditionalInformation { get; set; }
   public string EducationalAttendanceAdditionalInformation { get; set; }

   // assigned user
   public User AssignedUser { get; set; }

   // notes
   public ICollection<ProjectNote> Notes { get; set; } = new List<ProjectNote>();

}
