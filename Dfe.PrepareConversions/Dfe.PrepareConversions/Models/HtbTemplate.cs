﻿using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models;

public class HtbTemplate
{
   [DocumentText("SchoolUrn")]
   public string SchoolUrn { get; set; }

   [DocumentText("SchoolName")]
   public string SchoolName { get; set; }

   [DocumentText("SchoolNameAndUrn")]
   public string SchoolNameAndUrn { get; set; }

   [DocumentText("LocalAuthority")]
   public string LocalAuthority { get; set; }

   public string ApplicationReferenceNumber { get; set; }
   public string ProjectStatus { get; set; }
   public string ApplicationReceivedDate { get; set; }
   public string AssignedDate { get; set; }

   [DocumentText("HeadTeacherBoardDate")]
   public string HeadTeacherBoardDate { get; set; }

   public string BaselineDate { get; set; }

   //school/trust info
   [DocumentText("RecommendationForProject")]
   public string RecommendationForProject { get; set; }

   [DocumentText("Author")]
   public string Author { get; set; }

   [DocumentText("Version")]
   public string Version { get; set; }

   [DocumentText("ClearedBy")]
   public string ClearedBy { get; set; }

   [DocumentText("PreviousHeadTeacherBoardDate")]
   public string PreviousHeadTeacherBoardDate { get; set; }

   public string PreviousHeadTeacherBoardLink { get; set; }

   [DocumentText("TrustReferenceNumber")]
   public string TrustReferenceNumber { get; set; }

   [DocumentText("TrustName")]
   public string NameOfTrust { get; set; }

   [DocumentText("TrustNameAndReferenceNumber")]
   public string TrustNameAndReferenceNumber { get; set; }

   [DocumentText("SponsorReferenceNumber")]
   public string SponsorReferenceNumber { get; set; }

   [DocumentText("SponsorName")]
   public string SponsorName { get; set; }

   [DocumentText("AcademyTypeRouteAndConversionGrant")]
   public string AcademyTypeRouteAndConversionGrant { get; set; }

   [DocumentText("ConversionSupportGrantChangeReason")]
   public string ConversionSupportGrantChangeReason { get; set; }

   [DocumentText("ProposedAcademyOpeningDate")]
   public string ProposedAcademyOpeningDate { get; set; }

   // School Overview
   [DocumentText("SchoolPhase")]
   public string SchoolPhase { get; set; }

   [DocumentText("AgeRange")]
   public string AgeRange { get; set; }

   [DocumentText("SchoolType")]
   public string SchoolType { get; set; }

   [DocumentText("NumberOnRoll")]
   public string NumberOnRoll { get; set; }

   [DocumentText("PercentageSchoolFull")]
   public string PercentageSchoolFull { get; set; }

   [DocumentText("SchoolCapacity")]
   public string SchoolCapacity { get; set; }

   [DocumentText("PublishedAdmissionNumber")]
   public string PublishedAdmissionNumber { get; set; }

   [DocumentText("PercentageFreeSchoolMeals")]
   public string PercentageFreeSchoolMeals { get; set; }

   [DocumentText("PartOfPfiScheme")]
   public string PartOfPfiScheme { get; set; }

   [DocumentText("ViabilityIssues")]
   public string ViabilityIssues { get; set; }
   [DocumentText("NumberOfPlacesFundedFor")]
   public string NumberOfPlacesFundedFor { get; set; }
   [DocumentText("NumberOfResidentialPlaces")]
   public string NumberOfResidentialPlaces { get; set; }
   [DocumentText("NumberOfFundedResidentialPlaces")]
   public string NumberOfFundedResidentialPlaces { get; set; }

   [DocumentText("FinancialDeficit")]
   public string FinancialDeficit { get; set; }

   [DocumentText("IsSchoolLinkedToADiocese")]
   public string IsSchoolLinkedToADiocese { get; set; }

   [DocumentText("PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust")]
   public string PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; set; }

   [DocumentText("DistanceFromSchoolToTrustHeadquarters")]
   public string DistanceFromSchoolToTrustHeadquarters { get; set; }

   [DocumentText("DistanceFromSchoolToTrustHeadquartersAdditionalInformation")]
   public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }

   [DocumentText("ParliamentaryConstituency")]
   public string ParliamentaryConstituency { get; set; }

   [DocumentText("MemberOfParliamentNameAndParty")]
   public string MemberOfParliamentNameAndParty { get; set; }

   [DocumentText("PupilsAttendingGroup")]
   public string PupilsAttendingGroup { get; set; }

   [DocumentText("NumberOfAlternativeProvisionPlaces")]
   public string NumberOfAlternativeProvisionPlaces { get; set; }

   [DocumentText("NumberOfMedicalPlaces")]
   public string NumberOfMedicalPlaces { get; set; }

   [DocumentText("NumberOfPost16Places")]
   public string NumberOfPost16Places { get; set; }

   [DocumentText("NumberOfSENUnitPlaces")]
   public string NumberOfSENUnitPlaces { get; set; }

   [DocumentText("ConversionSupportGrantNumberOfSites")]
   public string ConversionSupportGrantNumberOfSites { get; set; }

   // rationale
   [DocumentText("RationaleForProject")]
   public string RationaleForProject { get; set; }

   [DocumentText("RationaleForTrust")]
   public string RationaleForTrust { get; set; }

   // risk and issues
   [DocumentText("RisksAndIssues")]
   public string RisksAndIssues { get; set; }

   // legal requirements
   [DocumentText("GoverningBodyResolution")]
   public string GoverningBodyResolution { get; set; }

   [DocumentText("Consultation")]
   public string Consultation { get; set; }

   [DocumentText("DiocesanConsent")]
   public string DiocesanConsent { get; set; }

   [DocumentText("FoundationConsent")]
   public string FoundationConsent { get; set; }

   // school budget info
   [DocumentText("EndOfCurrentFinancialYear")]
   public string EndOfCurrentFinancialYear { get; set; }

   [DocumentText("RevenueCarryForwardAtEndMarchCurrentYear")]
   public string RevenueCarryForwardAtEndMarchCurrentYear { get; set; }

   [DocumentText("ProjectedRevenueBalanceAtEndMarchNextYear")]
   public string ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }

   [DocumentText("EndOfNextFinancialYear")]
   public string EndOfNextFinancialYear { get; set; }

   [DocumentText("CapitalCarryForwardAtEndMarchCurrentYear")]
   public string CapitalCarryForwardAtEndMarchCurrentYear { get; set; }

   [DocumentText("CapitalCarryForwardAtEndMarchNextYear")]
   public string CapitalCarryForwardAtEndMarchNextYear { get; set; }

   [DocumentText("SchoolBudgetInformationAdditionalInformation")]
   public string SchoolBudgetInformationAdditionalInformation { get; set; }

   // school pupil forecasts
   [DocumentText("YearOneProjectedCapacity")]
   public string YearOneProjectedCapacity { get; set; }

   [DocumentText("YearOneProjectedPupilNumbers")]
   public string YearOneProjectedPupilNumbers { get; set; }

   [DocumentText("YearOnePercentageSchoolFull")]
   public string YearOnePercentageSchoolFull { get; set; }

   [DocumentText("YearTwoProjectedCapacity")]
   public string YearTwoProjectedCapacity { get; set; }

   [DocumentText("YearTwoProjectedPupilNumbers")]
   public string YearTwoProjectedPupilNumbers { get; set; }

   [DocumentText("YearTwoPercentageSchoolFull")]
   public string YearTwoPercentageSchoolFull { get; set; }

   [DocumentText("YearThreeProjectedCapacity")]
   public string YearThreeProjectedCapacity { get; set; }

   [DocumentText("YearThreeProjectedPupilNumbers")]
   public string YearThreeProjectedPupilNumbers { get; set; }

   [DocumentText("YearThreePercentageSchoolFull")]
   public string YearThreePercentageSchoolFull { get; set; }

   [DocumentText("SchoolPupilForecastsAdditionalInformation")]
   public string SchoolPupilForecastsAdditionalInformation { get; set; }
   public IEnumerable<EducationalAttendanceViewModel> EducationalAttendance { get; set; }

   // Public Sector Equality Duty
   [DocumentText("PublicEqualityDutyImpact")]
   public string PublicEqualityDutyImpact { get; set; }

   [DocumentText("PublicEqualityDutyReduceImpactReason")]
   public string PublicEqualityDutyReduceImpactReason { get; set; }

   public static HtbTemplate Build(AcademyConversionProject project,
                                   SchoolOverview schoolOverview, KeyStagePerformance keyStagePerformance)
   {
      HtbTemplate htbTemplate = new()
      {
         SchoolName = project.SchoolName,
         SchoolUrn = project.Urn.ToString(),
         SchoolNameAndUrn = $"{project.SchoolName} - URN {project.Urn.ToString()}",
         LocalAuthority = project.LocalAuthority,
         ApplicationReceivedDate = project.ApplicationReceivedDate.ToDateString(),
         AssignedDate = project.AssignedDate.ToDateString(),
         HeadTeacherBoardDate = project.HeadTeacherBoardDate.ToDateString(),
         RecommendationForProject = project.RecommendationForProject,
         Author = $"Author: {project.Author}",
         Version = $"Version: {DateTime.Today.ToDateString()}",
         ClearedBy = $"Cleared by: {project.ClearedBy}",
         PreviousHeadTeacherBoardDate = project.PreviousHeadTeacherBoardDate.HasValue ? project.PreviousHeadTeacherBoardDate.ToDateString() : "No",
         PreviousHeadTeacherBoardLink = project.PreviousHeadTeacherBoardLink,
         TrustReferenceNumber = project.TrustReferenceNumber,
         NameOfTrust = project.NameOfTrust,
         TrustNameAndReferenceNumber = $"{project.NameOfTrust} - {project.TrustReferenceNumber}",
         SponsorReferenceNumber = project.SponsorReferenceNumber ?? "Not applicable",
         SponsorName = project.SponsorName ?? "Not applicable",
         AcademyTypeRouteAndConversionGrant = $"{project.AcademyTypeAndRoute} - {project.ConversionSupportGrantAmount?.ToMoneyString(true)}",
         ConversionSupportGrantChangeReason = project.ConversionSupportGrantChangeReason,
         ProposedAcademyOpeningDate = project.ProposedConversionDate.ToDateString(),
         SchoolPhase = schoolOverview.SchoolPhase,
         AgeRange = !string.IsNullOrEmpty(schoolOverview.AgeRangeLower) && !string.IsNullOrEmpty(schoolOverview.AgeRangeUpper)
            ? $"{schoolOverview.AgeRangeLower} to {schoolOverview.AgeRangeUpper}"
            : "",
         SchoolType = schoolOverview.SchoolType,
         NumberOnRoll = schoolOverview.NumberOnRoll?.ToString(),
         PercentageSchoolFull = schoolOverview.NumberOnRoll.AsPercentageOf(schoolOverview.SchoolCapacity),
         SchoolCapacity = schoolOverview.SchoolCapacity?.ToString(),
         PublishedAdmissionNumber = project.PublishedAdmissionNumber,
         PercentageFreeSchoolMeals = !string.IsNullOrEmpty(schoolOverview.PercentageFreeSchoolMeals) ? $"{schoolOverview.PercentageFreeSchoolMeals}%" : "",
         PartOfPfiScheme = project.PartOfPfiScheme,
         ViabilityIssues = project.ViabilityIssues,
         NumberOfPlacesFundedFor = project.NumberOfPlacesFundedFor.ToStringOrDefault(),
         NumberOfResidentialPlaces = project.NumberOfResidentialPlaces.ToStringOrDefault(),
         NumberOfFundedResidentialPlaces = project.NumberOfFundedResidentialPlaces.ToStringOrDefault(),
         FinancialDeficit = project.FinancialDeficit,
         IsSchoolLinkedToADiocese = schoolOverview.IsSchoolLinkedToADiocese,
         DistanceFromSchoolToTrustHeadquarters = project.DistanceFromSchoolToTrustHeadquarters != null
            ? $"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()} miles"
            : null,
         DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
         ParliamentaryConstituency = schoolOverview.ParliamentaryConstituency,
         MemberOfParliamentNameAndParty = project.MemberOfParliamentNameAndParty,
         RationaleForProject = project.RationaleForProject,
         RationaleForTrust = project.RationaleForTrust,
         RisksAndIssues = project.RisksAndIssues,
         GoverningBodyResolution = project.GoverningBodyResolution.SplitPascalCase(),
         Consultation = project.Consultation.SplitPascalCase(),
         DiocesanConsent = project.DiocesanConsent.SplitPascalCase(),
         FoundationConsent = project.FoundationConsent.SplitPascalCase(),
         EndOfCurrentFinancialYear = project.EndOfCurrentFinancialYear?.ToDateString(),
         EndOfNextFinancialYear = project.EndOfNextFinancialYear?.ToDateString(),
         RevenueCarryForwardAtEndMarchCurrentYear = project.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString(true),
         ProjectedRevenueBalanceAtEndMarchNextYear = project.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString(true),
         CapitalCarryForwardAtEndMarchCurrentYear = project.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString(true),
         CapitalCarryForwardAtEndMarchNextYear = project.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString(true),
         SchoolBudgetInformationAdditionalInformation = project.SchoolBudgetInformationAdditionalInformation,
         YearOneProjectedCapacity = project.YearOneProjectedCapacity.ToString(),
         YearOneProjectedPupilNumbers = project.YearOneProjectedPupilNumbers.ToStringOrDefault(),
         YearOnePercentageSchoolFull = project.YearOneProjectedPupilNumbers.AsPercentageOf(project.YearOneProjectedCapacity),
         YearTwoProjectedCapacity = project.YearTwoProjectedCapacity.ToString(),
         YearTwoProjectedPupilNumbers = project.YearTwoProjectedPupilNumbers.ToString(),
         YearTwoPercentageSchoolFull = project.YearTwoProjectedPupilNumbers.AsPercentageOf(project.YearTwoProjectedCapacity),
         YearThreeProjectedCapacity = project.YearThreeProjectedCapacity.ToString(),
         YearThreeProjectedPupilNumbers = project.YearThreeProjectedPupilNumbers.ToString(),
         YearThreePercentageSchoolFull = project.YearThreeProjectedPupilNumbers.AsPercentageOf(project.YearThreeProjectedCapacity),
         SchoolPupilForecastsAdditionalInformation = project.SchoolPupilForecastsAdditionalInformation,
         PupilsAttendingGroup = SchoolOverviewHelper.MapPupilsAttendingGroup(project.PupilsAttendingGroupPermanentlyExcluded, project.PupilsAttendingGroupMedicalAndHealthNeeds, project.PupilsAttendingGroupTeenageMums),
         ConversionSupportGrantNumberOfSites = project.ConversionSupportGrantNumberOfSites,
         NumberOfAlternativeProvisionPlaces = project.NumberOfAlternativeProvisionPlaces?.ToString(),
         NumberOfSENUnitPlaces = project.NumberOfSENUnitPlaces?.ToString(),
         NumberOfMedicalPlaces = project.NumberOfMedicalPlaces?.ToString(),
         NumberOfPost16Places = project.NumberOfPost16Places?.ToString(),
         // Public Sector Equality Duty
         PublicEqualityDutyImpact = project.PublicEqualityDutyImpact,
         PublicEqualityDutyReduceImpactReason = project.PublicEqualityDutyReduceImpactReason
      };

      if (keyStagePerformance.HasSchoolAbsenceData)
      {
         htbTemplate.EducationalAttendance = keyStagePerformance.SchoolAbsenceData.Select(EducationalAttendanceViewModel.Build).OrderByDescending(ks => ks.Year);
      }
      return htbTemplate;
   }
}
