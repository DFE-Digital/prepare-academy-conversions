using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.Extensions;
using Dfe.Academisation.ExtensionMethods;
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

   [DocumentText("AcademyOrderRequired")]
   public string AcademyOrderRequired { get; set; }

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

   public string MPName { get; set; }
   public string MPParty { get; set; }

   [DocumentText("MPNameAndParty")]
   public string MPNameAndParty
   {
      get
      {
         string delimiter = string.Empty;
         if (!string.IsNullOrEmpty(MPName) && !string.IsNullOrEmpty(MPParty))
         {
            delimiter = ", ";
         }

         return $"{MPName}{delimiter}{MPParty}";
      }
   }

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

   public SchoolPerformance SchoolPerformance { get; set; }
   public IEnumerable<KeyStage2PerformanceTableViewModel> KeyStage2 { get; set; }
   public KeyStage4PerformanceTableViewModel KeyStage4 { get; set; }
   public IEnumerable<KeyStage5PerformanceTableViewModel> KeyStage5 { get; set; }

   public static HtbTemplate Build(AcademyConversionProject project,
                                   SchoolPerformance schoolPerformance,
                                   SchoolOverview schoolOverview,
                                   KeyStagePerformance keyStagePerformance)
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
         AcademyOrderRequired = project.AcademyOrderRequired,
         PreviousHeadTeacherBoardDate = project.PreviousHeadTeacherBoardDate.HasValue ? project.PreviousHeadTeacherBoardDate.ToDateString() : "No",
         PreviousHeadTeacherBoardLink = project.PreviousHeadTeacherBoardLink,
         TrustReferenceNumber = project.TrustReferenceNumber,
         NameOfTrust = project.NameOfTrust,
         TrustNameAndReferenceNumber = $"{project.NameOfTrust} - {project.TrustReferenceNumber}",
         SponsorReferenceNumber = project.SponsorReferenceNumber ?? "Not applicable",
         SponsorName = project.SponsorName ?? "Not applicable",
         AcademyTypeRouteAndConversionGrant = $"{project.AcademyTypeAndRoute} - {project.ConversionSupportGrantAmount?.ToMoneyString(true)}",
         ConversionSupportGrantChangeReason = project.ConversionSupportGrantChangeReason,
         ProposedAcademyOpeningDate = project.ProposedAcademyOpeningDate.ToDateString(),
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
         FinancialDeficit = project.FinancialDeficit,
         IsSchoolLinkedToADiocese = schoolOverview.IsSchoolLinkedToADiocese,
         DistanceFromSchoolToTrustHeadquarters = project.DistanceFromSchoolToTrustHeadquarters != null
            ? $"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()} miles"
            : null,
         DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
         ParliamentaryConstituency = schoolOverview.ParliamentaryConstituency,
         MPName = project.MemberOfParliamentName,
         MPParty = project.MemberOfParliamentParty,
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
         SchoolPerformance = schoolPerformance
      };

      if (keyStagePerformance.HasKeyStage2PerformanceTables)
      {
         htbTemplate.KeyStage2 = keyStagePerformance.KeyStage2.Select(KeyStage2PerformanceTableViewModel.Build).OrderByDescending(ks => ks.Year).ToList();
      }

      if (keyStagePerformance.HasKeyStage4PerformanceTables)
      {
         htbTemplate.KeyStage4 = KeyStage4PerformanceTableViewModel.Build(keyStagePerformance.KeyStage4);
      }

      if (keyStagePerformance.HasKeyStage5PerformanceTables)
      {
         htbTemplate.KeyStage5 = keyStagePerformance.KeyStage5.Select(KeyStage5PerformanceTableViewModel.Build).OrderByDescending(ks => ks.Year);
      }

      return htbTemplate;
   }
}
