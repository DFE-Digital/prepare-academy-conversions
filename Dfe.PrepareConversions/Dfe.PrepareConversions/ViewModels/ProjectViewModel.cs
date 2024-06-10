using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Utils;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.ViewModels;

public class ProjectViewModel : ProjectTypeBase
{
   public ProjectViewModel(AcademyConversionProject project)
   {
      Id = project.Id.ToString();
      FormAMatProjectId = project.FormAMatProjectId;
      ApplicationSharePointId = project.ApplicationSharePointId;
      SchoolSharePointId = project.SchoolSharePointId;
      IsFormAMat = project.IsFormAMat.HasValue && project.IsFormAMat.Value;
      ProjectStatus = ProjectListHelper.MapProjectStatus(project.ProjectStatus).Value;
      ProjectStatusColour = ProjectListHelper.MapProjectStatus(project.ProjectStatus).Colour;
      ApplicationReferenceNumber = project.ApplicationReferenceNumber;
      SchoolName = project.SchoolName;
      SchoolURN = project.Urn.ToString();
      SchoolType = project.SchoolType;
      LocalAuthority = project.LocalAuthority;
      ApplicationReceivedDate = project.ApplicationReceivedDate.ToDateString();
      AssignedDate = project.AssignedDate.ToDateString();
      SchoolPhase = project.SchoolPhase;
      SchoolType = project.SchoolType;
      HeadTeacherBoardDate = project.HeadTeacherBoardDate;

      LocalAuthorityInformationTemplateSentDate = project.LocalAuthorityInformationTemplateSentDate;
      LocalAuthorityInformationTemplateReturnedDate = project.LocalAuthorityInformationTemplateReturnedDate;
      LocalAuthorityInformationTemplateComments = project.LocalAuthorityInformationTemplateComments;
      LocalAuthorityInformationTemplateLink = project.LocalAuthorityInformationTemplateLink;
      LocalAuthorityInformationTemplateSectionComplete = project.LocalAuthorityInformationTemplateSectionComplete ?? false;

      RecommendationForProject = project.RecommendationForProject;
      Author = project.Author;
      Version = project.Version;
      ClearedBy = project.ClearedBy;
      PreviousHeadTeacherBoardDateQuestion = project.PreviousHeadTeacherBoardDateQuestion;
      PreviousHeadTeacherBoardDate = project.PreviousHeadTeacherBoardDate;
      PreviousHeadTeacherBoardLink = project.PreviousHeadTeacherBoardLink;
      TrustReferenceNumber = project.TrustReferenceNumber;
      NameOfTrust = project.NameOfTrust;
      SponsorReferenceNumber = project.SponsorReferenceNumber;
      SponsorName = project.SponsorName;
      AcademyTypeAndRoute = project.AcademyTypeAndRoute;
      Form7Received = project.Form7Received;
      Form7ReceivedDate = project.Form7ReceivedDate;
      ProposedAcademyOpeningDate = project.ProposedAcademyOpeningDate;
      SchoolAndTrustInformationSectionComplete = project.SchoolAndTrustInformationSectionComplete ?? false;
      ConversionSupportGrantAmount = project.ConversionSupportGrantAmount ?? 0;
      ConversionSupportGrantChangeReason = project.ConversionSupportGrantChangeReason;
      ConversionSupportGrantType = project.ConversionSupportGrantType;
      ConversionSupportGrantEnvironmentalImprovementGrant = project.ConversionSupportGrantEnvironmentalImprovementGrant;
      ConversionSupportGrantAmountChanged = project.ConversionSupportGrantAmountChanged;
      NumberOfSites = project.ConversionSupportGrantNumberOfSites;
      DaoPackSentDate = project.DaoPackSentDate;

      AnnexBFormReceived = project.AnnexBFormReceived;
      AnnexBFormUrl = project.AnnexBFormUrl;

      ExternalApplicationFormSaved = project.ExternalApplicationFormSaved;
      ExternalApplicationFormUrl = project.ExternalApplicationFormUrl;

      PartOfPfiScheme = project.PartOfPfiScheme;
      PfiSchemeDetails = project.PfiSchemeDetails;
      PublishedAdmissionNumber = project.PublishedAdmissionNumber;
      ViabilityIssues = project.ViabilityIssues;
      NumberOfPlacesFundedFor = project.NumberOfPlacesFundedFor;
      NumberOfResidentialPlaces = project.NumberOfResidentialPlaces;
      NumberOfFundedResidentialPlaces = project.NumberOfFundedResidentialPlaces;
      FinancialDeficit = project.FinancialDeficit;
      DistanceFromSchoolToTrustHeadquarters = project.DistanceFromSchoolToTrustHeadquarters;
      DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation;
      MemberOfParliamentNameAndParty = project.MemberOfParliamentNameAndParty;
      SchoolOverviewSectionComplete = project.SchoolOverviewSectionComplete ?? false;

      SchoolPerformanceAdditionalInformation = project.SchoolPerformanceAdditionalInformation;

      RationaleForProject = project.RationaleForProject;
      RationaleForTrust = project.RationaleForTrust;
      RationaleSectionComplete = project.RationaleSectionComplete ?? false;

      RisksAndIssues = project.RisksAndIssues;
      RisksAndIssuesSectionComplete = project.RisksAndIssuesSectionComplete ?? false;

      GoverningBodyResolution = project.GoverningBodyResolution;
      Consultation = project.Consultation;
      DiocesanConsent = project.DiocesanConsent;
      FoundationConsent = project.FoundationConsent;
      LegalRequirementsSectionComplete = project.LegalRequirementsSectionComplete ?? false;

      YearOneProjectedCapacity = project.YearOneProjectedCapacity;
      YearOneProjectedPupilNumbers = project.YearOneProjectedPupilNumbers;
      YearTwoProjectedCapacity = project.YearTwoProjectedCapacity;
      YearTwoProjectedPupilNumbers = project.YearTwoProjectedPupilNumbers;
      YearThreeProjectedCapacity = project.YearThreeProjectedCapacity;
      YearThreeProjectedPupilNumbers = project.YearThreeProjectedPupilNumbers;
      YearFourProjectedCapacity = project.YearFourProjectedCapacity;
      YearFourProjectedPupilNumbers = project.YearFourProjectedPupilNumbers;
      SchoolPupilForecastsAdditionalInformation = project.SchoolPupilForecastsAdditionalInformation;

      EndOfCurrentFinancialYear = project.EndOfCurrentFinancialYear;
      RevenueCarryForwardAtEndMarchCurrentYear = project.RevenueCarryForwardAtEndMarchCurrentYear;
      CapitalCarryForwardAtEndMarchCurrentYear = project.CapitalCarryForwardAtEndMarchCurrentYear;
      EndOfNextFinancialYear = project.EndOfNextFinancialYear;
      ProjectedRevenueBalanceAtEndMarchNextYear = project.ProjectedRevenueBalanceAtEndMarchNextYear;
      CapitalCarryForwardAtEndMarchNextYear = project.CapitalCarryForwardAtEndMarchNextYear;
      SchoolBudgetInformationAdditionalInformation = project.SchoolBudgetInformationAdditionalInformation;
      SchoolBudgetInformationSectionComplete = project.SchoolBudgetInformationSectionComplete ?? false;

      KeyStage2PerformanceAdditionalInformation = project.KeyStage2PerformanceAdditionalInformation;
      KeyStage4PerformanceAdditionalInformation = project.KeyStage4PerformanceAdditionalInformation;
      KeyStage5PerformanceAdditionalInformation = project.KeyStage5PerformanceAdditionalInformation;
      EducationalAttendanceAdditionalInformation = project.EducationalAttendanceAdditionalInformation;

      AssignedUser = project.AssignedUser;

      Notes = project.Notes;

      PupilsAttendingGroupPermanentlyExcluded = project.PupilsAttendingGroupPermanentlyExcluded;
      PupilsAttendingGroupMedicalAndHealthNeeds = project.PupilsAttendingGroupMedicalAndHealthNeeds;
      PupilsAttendingGroupTeenageMums = project.PupilsAttendingGroupTeenageMums;

      NumberOfAlternativeProvisionPlaces = project.NumberOfAlternativeProvisionPlaces;
      NumberOfMedicalPlaces = project.NumberOfMedicalPlaces;
      NumberOfPost16Places = project.NumberOfPost16Places;
      NumberOfSENUnitPlaces = project.NumberOfSENUnitPlaces;
   }

   public string Id { get; }
   public int? FormAMatProjectId { get; }
   public Guid? SchoolSharePointId { get; }
   public Guid? ApplicationSharePointId { get; }
   public string ProjectStatus { get; }
   public string ProjectStatusColour { get; }
   public string ApplicationReferenceNumber { get; set; }
   public string SchoolName { get; }
   public string SchoolURN { get; }
   public string LocalAuthority { get; }
   public string ApplicationReceivedDate { get; }
   public string AssignedDate { get; }
   public string SchoolPhase { get; }
   public string SchoolType { get; }

   public bool IsPRU { get { return SchoolType?.ToLower() == "pupil referal unit"; } }
   public bool IsSEN { get { return SchoolType?.ToLower().Contains("special") ?? false; } }
   public DateTime? HeadTeacherBoardDate { get; set; }

   public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }
   public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; set; }
   public string LocalAuthorityInformationTemplateComments { get; set; }
   public string LocalAuthorityInformationTemplateLink { get; set; }
   public bool LocalAuthorityInformationTemplateSectionComplete { get; set; }

   //school/trust info
   public string RecommendationForProject { get; set; }
   public string Author { get; set; }
   public string Version { get; set; }
   public string ClearedBy { get; set; }
   public string Form7Received { get; set; }
   public DateTime? Form7ReceivedDate { get; set; }
   public string PreviousHeadTeacherBoardDateQuestion { get; set; }
   public DateTime? PreviousHeadTeacherBoardDate { get; set; }
   public string PreviousHeadTeacherBoardLink { get; set; }
   public string TrustReferenceNumber { get; set; }
   public string NameOfTrust { get; set; }
   public string SponsorReferenceNumber { get; set; }
   public string SponsorName { get; set; }
   public string AcademyTypeAndRoute { get; set; }
   public DateTime? ProposedAcademyOpeningDate { get; set; }
   public bool SchoolAndTrustInformationSectionComplete { get; set; }
   public decimal ConversionSupportGrantAmount { get; set; }
   public string ConversionSupportGrantChangeReason { get; set; }
   public string ConversionSupportGrantType { get; set; }
   public string ConversionSupportGrantEnvironmentalImprovementGrant { get; set; }
   public bool? ConversionSupportGrantAmountChanged { get; set; }
   public DateTime? DaoPackSentDate { get; set; }
   public string NumberOfSites { get; set; }

   // Annex B
   public bool? AnnexBFormReceived { get; set; }
   public string AnnexBFormUrl { get; set; }

   // External Application Form
   public bool? ExternalApplicationFormSaved { get; set; }
   public string ExternalApplicationFormUrl { get; set; }

   // School Overview
   public string PublishedAdmissionNumber { get; set; }
   public string PartOfPfiScheme { get; set; }
   public string PfiSchemeDetails { get; set; }
   public string ViabilityIssues { get; set; }
   public string FinancialDeficit { get; set; }
   public decimal? NumberOfPlacesFundedFor { get; set; }
   public decimal? NumberOfResidentialPlaces { get; set; }
   public decimal? NumberOfFundedResidentialPlaces { get; set; }
   public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
   public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
   public string MemberOfParliamentNameAndParty { get; set; }
   public bool SchoolOverviewSectionComplete { get; set; }
   public bool? PupilsAttendingGroupPermanentlyExcluded { get; set; }
   public bool? PupilsAttendingGroupMedicalAndHealthNeeds { get; set; }
   public bool? PupilsAttendingGroupTeenageMums { get; set; }
   public int? NumberOfAlternativeProvisionPlaces { get; set; }
   public int? NumberOfMedicalPlaces { get; set; }
   public int? NumberOfPost16Places { get; set; }
   public int? NumberOfSENUnitPlaces { get; set; }

   //school performance ofsted information
   public string SchoolPerformanceAdditionalInformation { get; set; }

   public string RationaleForProject { get; set; }
   public string RationaleForTrust { get; set; }
   public bool RationaleSectionComplete { get; set; }

   // risk and issues
   public string RisksAndIssues { get; set; }
   public bool RisksAndIssuesSectionComplete { get; set; }

   // legal requirements
   public string GoverningBodyResolution { get; set; }
   public string Consultation { get; set; }
   public string DiocesanConsent { get; set; }
   public string FoundationConsent { get; set; }
   public bool LegalRequirementsSectionComplete { get; set; }

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

   //school budget info

   public DateTime? EndOfCurrentFinancialYear { get; set; }
   public decimal? RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
   public decimal? CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
   public DateTime? EndOfNextFinancialYear { get; set; }
   public decimal? ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
   public decimal? CapitalCarryForwardAtEndMarchNextYear { get; set; }
   public string SchoolBudgetInformationAdditionalInformation { get; set; }
   public bool SchoolBudgetInformationSectionComplete { get; set; }

   public string KeyStage2PerformanceAdditionalInformation { get; set; }
   public string KeyStage4PerformanceAdditionalInformation { get; set; }
   public string KeyStage5PerformanceAdditionalInformation { get; set; }
   public string EducationalAttendanceAdditionalInformation { get; set; }

   public User AssignedUser { get; set; }

   public ICollection<ProjectNote> Notes { get; }

   protected override string TypeAndRouteValue => AcademyTypeAndRoute;

   public override bool IsExternalSchoolApplication => string.IsNullOrEmpty(this.ApplicationReferenceNumber);

}
