using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages;

public class UpdateAcademyConversionProjectPageModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public UpdateAcademyConversionProjectPageModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
   {
      _errorService = errorService;
   }


   [BindProperty]
   public AcademyConversionProjectPostModel AcademyConversionProject { get; set; }

   public bool ShowError => _errorService.HasErrors();

   public string SuccessPage
   {
      get => TempData["SuccessPage"].ToString();
      set => TempData["SuccessPage"] = value;
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await SetProject(id);

      bool schoolAndTrustInformationSectionComplete = AcademyConversionProject.SchoolAndTrustInformationSectionComplete != null &&
                                                      AcademyConversionProject.SchoolAndTrustInformationSectionComplete.Value;

      if (AcademyConversionProject.LocalAuthorityInformationTemplateSentDate.HasValue &&
         AcademyConversionProject.LocalAuthorityInformationTemplateReturnedDate.HasValue &&
         AcademyConversionProject.LocalAuthorityInformationTemplateSentDate > AcademyConversionProject.LocalAuthorityInformationTemplateReturnedDate)
      {
         _errorService.AddError("returnedDateBeforeSentDateError", "The returned template date be must on or after sent date");
      }

      if (AcademyConversionProject.EndOfCurrentFinancialYear.HasValue &&
          AcademyConversionProject.EndOfNextFinancialYear.HasValue &&
          AcademyConversionProject.EndOfCurrentFinancialYear != DateTime.MinValue &&
          AcademyConversionProject.EndOfNextFinancialYear != DateTime.MinValue &&
          AcademyConversionProject.EndOfCurrentFinancialYear.Value.AddYears(1).AddDays(-1) > AcademyConversionProject.EndOfNextFinancialYear)
      {
         _errorService.AddError(
            $"/task-list/{id}/confirm-school-budget-information/update-school-budget-information?return=%2FTaskList%2FSchoolBudgetInformation/ConfirmSchoolBudgetInformation&fragment=financial-year",
            "The next financial year cannot be before or within a year of the current financial year");
      }

      _errorService.AddErrors(Request.Form.Keys, ModelState);
      if (_errorService.HasErrors())
      {
         RePopDatePickerModelsAfterValidationFail();
         return Page();
      }

      ApiResponse<AcademyConversionProject> response = await _repository.UpdateProject(id, Build());

      if (!response.Success)
      {
         _errorService.AddApiError();
         return Page();
      }

      (string returnPage, string fragment) = GetReturnPageAndFragment();
      if (!string.IsNullOrWhiteSpace(returnPage))
      {
         return RedirectToPage(returnPage, null, new { id }, fragment);
      }

      return RedirectToPage(SuccessPage, new { id });
   }

   private void RePopDatePickerModelsAfterValidationFail()
   {
      Project.LocalAuthorityInformationTemplateSentDate = AcademyConversionProject.LocalAuthorityInformationTemplateSentDate;
      Project.LocalAuthorityInformationTemplateReturnedDate = AcademyConversionProject.LocalAuthorityInformationTemplateReturnedDate;
   }
   public static decimal? CalculateGrantAmount(string type, string phase)
   {
      int defaultAmount = 25000;
      if (phase is null) return defaultAmount;
      switch (phase.ToLower())
      {
         case "primary":
            return type switch
            {
               SponsoredGrantType.FastTrack => 70000,
               SponsoredGrantType.Full => 110000,
               SponsoredGrantType.Intermediate => 90000,
               _ => defaultAmount
            };
         case "secondary":
            return type switch
            {
               SponsoredGrantType.FastTrack => 80000,
               SponsoredGrantType.Full => 150000,
               SponsoredGrantType.Intermediate => 115000,
               _ => defaultAmount
            };
      }

      // Else return default £25k
      return defaultAmount;
   }

   public static decimal? CalculatePRUGrantAmount(string type, string numberOfSites = "1")
   {
      int defaultAmount = 25000;
      int supplementAmount = CalculateGrantSupplement(numberOfSites);

      return type switch
      {
         SponsoredGrantType.FastTrack => 70000 + supplementAmount,
         SponsoredGrantType.Full => 110000 + supplementAmount,
         SponsoredGrantType.Intermediate => 90000 + supplementAmount,
         _ => defaultAmount + supplementAmount
      };
   }

   protected UpdateAcademyConversionProject Build()
   {
      return new UpdateAcademyConversionProject
      {
         ProjectStatus = AcademyConversionProject.ProjectStatus,
         ConversionSupportGrantAmount = AcademyConversionProject.ConversionSupportGrantAmount,
         ConversionSupportGrantChangeReason = AcademyConversionProject.ConversionSupportGrantChangeReason,
         ConversionSupportGrantType = AcademyConversionProject.ConversionSupportGrantType,
         ConversionSupportGrantNumberOfSites = AcademyConversionProject.ConversionSupportNumberOfSites,
         ConversionSupportGrantEnvironmentalImprovementGrant = AcademyConversionProject.ConversionSupportGrantEnvironmentalImprovementGrant,
         ConversionSupportGrantAmountChanged = ConversionSupportGrantAmountChanged(Project?.AcademyTypeAndRoute ?? string.Empty),
         ApplicationReceivedDate = AcademyConversionProject.ApplicationReceivedDate,
         AssignedDate = AcademyConversionProject.AssignedDate,
         HeadTeacherBoardDate = AcademyConversionProject.HeadTeacherBoardDate,
         BaselineDate = AcademyConversionProject.BaselineDate,
         LocalAuthorityInformationTemplateSentDate = AcademyConversionProject.LocalAuthorityInformationTemplateSentDate,
         LocalAuthorityInformationTemplateReturnedDate = AcademyConversionProject.LocalAuthorityInformationTemplateReturnedDate,
         LocalAuthorityInformationTemplateComments = AcademyConversionProject.LocalAuthorityInformationTemplateComments,
         LocalAuthorityInformationTemplateLink = AcademyConversionProject.LocalAuthorityInformationTemplateLink,
         LocalAuthorityInformationTemplateSectionComplete = AcademyConversionProject.LocalAuthorityInformationTemplateSectionComplete,
         RecommendationForProject = AcademyConversionProject.RecommendationForProject,
         Author = AcademyConversionProject.Author,
         ClearedBy = AcademyConversionProject.ClearedBy,
         Form7Received = AcademyConversionProject.Form7Received,
         Form7ReceivedDate = AcademyConversionProject.Form7ReceivedDate == default(DateTime) ? null : AcademyConversionProject.Form7ReceivedDate,
         PreviousHeadTeacherBoardDateQuestion = AcademyConversionProject.PreviousHeadTeacherBoardDateQuestion,
         PreviousHeadTeacherBoardDate =
            AcademyConversionProject.PreviousHeadTeacherBoardDateQuestion == "No" ? default(DateTime) : AcademyConversionProject.PreviousHeadTeacherBoardDate,
         SchoolAndTrustInformationSectionComplete = AcademyConversionProject.SchoolAndTrustInformationSectionComplete,
         PublishedAdmissionNumber = AcademyConversionProject.PublishedAdmissionNumber,
         ViabilityIssues = AcademyConversionProject.ViabilityIssues,
         NumberOfPlacesFundedFor = AcademyConversionProject.NumberOfPlacesFundedFor,
         NumberOfResidentialPlaces = AcademyConversionProject.NumberOResidentialPlaces,
         NumberOfFundedResidentialPlaces = AcademyConversionProject.NumberOfFundedResidentialPlaces,
         FinancialDeficit = AcademyConversionProject.FinancialDeficit,
         IsThisADiocesanTrust = AcademyConversionProject.IsThisADiocesanTrust,
         DistanceFromSchoolToTrustHeadquarters = AcademyConversionProject.DistanceFromSchoolToTrustHeadquarters,
         DistanceFromSchoolToTrustHeadquartersAdditionalInformation = AcademyConversionProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
         MemberOfParliamentNameAndParty = AcademyConversionProject.MemberOfParliamentNameAndParty,
         SchoolOverviewSectionComplete = AcademyConversionProject.SchoolOverviewSectionComplete,
         SchoolPerformanceAdditionalInformation = AcademyConversionProject.SchoolPerformanceAdditionalInformation,
         RationaleForProject = AcademyConversionProject.RationaleForProject,
         RationaleForTrust = AcademyConversionProject.RationaleForTrust,
         RationaleSectionComplete = AcademyConversionProject.RationaleSectionComplete,
         RisksAndIssues = AcademyConversionProject.RisksAndIssues,
         RisksAndIssuesSectionComplete = AcademyConversionProject.RisksAndIssuesSectionComplete,
         LegalRequirementsSectionComplete = AcademyConversionProject.LegalRequirementsSectionComplete,
         GoverningBodyResolution = AcademyConversionProject.GoverningBodyResolution,
         Consultation = AcademyConversionProject.Consultation,
         DiocesanConsent = AcademyConversionProject.DiocesanConsent,
         FoundationConsent = AcademyConversionProject.FoundationConsent,
         EndOfCurrentFinancialYear = AcademyConversionProject.EndOfCurrentFinancialYear,
         EndOfNextFinancialYear = AcademyConversionProject.EndOfNextFinancialYear,
         RevenueCarryForwardAtEndMarchCurrentYear = AcademyConversionProject.RevenueCarryForwardAtEndMarchCurrentYear,
         ProjectedRevenueBalanceAtEndMarchNextYear = AcademyConversionProject.ProjectedRevenueBalanceAtEndMarchNextYear,
         CapitalCarryForwardAtEndMarchCurrentYear = AcademyConversionProject.CapitalCarryForwardAtEndMarchCurrentYear,
         CapitalCarryForwardAtEndMarchNextYear = AcademyConversionProject.CapitalCarryForwardAtEndMarchNextYear,
         SchoolBudgetInformationAdditionalInformation = AcademyConversionProject.SchoolBudgetInformationAdditionalInformation,
         SchoolBudgetInformationSectionComplete = AcademyConversionProject.SchoolBudgetInformationSectionComplete,
         SchoolPupilForecastsAdditionalInformation = AcademyConversionProject.SchoolPupilForecastsAdditionalInformation,
         KeyStage2PerformanceAdditionalInformation = AcademyConversionProject.KeyStage2PerformanceAdditionalInformation,
         KeyStage4PerformanceAdditionalInformation = AcademyConversionProject.KeyStage4PerformanceAdditionalInformation,
         KeyStage5PerformanceAdditionalInformation = AcademyConversionProject.KeyStage5PerformanceAdditionalInformation,
         DaoPackSentDate = AcademyConversionProject.DaoPackSentDate == default(DateTime) ? null : AcademyConversionProject.DaoPackSentDate,
         NumberOfAlternativeProvisionPlaces = AcademyConversionProject.NumberOfAlternativeProvisionPlaces,
         NumberOfMedicalPlaces = AcademyConversionProject.NumberOfMedicalPlaces,
         NumberOfSENUnitPlaces = AcademyConversionProject.NumberOfSENUnitPlaces,
         NumberOfPost16Places = AcademyConversionProject.NumberOfPost16Places,
      };
   }

   private bool? ConversionSupportGrantAmountChanged(string academyRoute)
   {
      if (academyRoute == AcademyTypeAndRoutes.Sponsored || academyRoute == AcademyTypeAndRoutes.Voluntary)
      {
         return AcademyConversionProject.ConversionSupportGrantAmountChanged;
      }

      return null;
   }

   private static int CalculateGrantSupplement(string numberOfSites)
   {
      return numberOfSites switch
      {
         "2" => 5000,
         "3" => 10000,
         "4" => 10000,
         "5 or more" => 20000,
         _ => 0
      };
   }

   private (string, string) GetReturnPageAndFragment()
   {
      Request.Query.TryGetValue("return", out StringValues returnQuery);
      Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
      return (returnQuery, fragmentQuery);
   }
}
