using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.KeyStagePerformance
{
   public class PerformanceDataViewModel: BaseAcademyConversionProjectPageModel
   {
      private readonly ErrorService _errorService;

      public PerformanceDataViewModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
      {
         _errorService = errorService;
      }

      [BindProperty]
      public SetPerformanceDataModel Model { get; set; }

      public bool ShowError => _errorService.HasErrors();

      public string SuccessPage
      {
         get => TempData["SuccessPage"].ToString();
         set => TempData["SuccessPage"] = value;
      }

      public override async Task<IActionResult> OnPostAsync(int id)
      {
         //if (YesChecked is true && string.IsNullOrWhiteSpace(ExternalApplicationFormUrl))
         //{
         //   ModelState.AddModelError(nameof(ExternalApplicationFormUrl), "You must enter valid link for the schools application form");
         //}

         if (ModelState.IsValid)
         {

            await _repository.SetProjectExternalApplicationForm(id, YesChecked.Value, YesChecked is true ? ExternalApplicationFormUrl : default);


            return RedirectToPage(Links.ExternalApplicationForm.Index.Page, new { id });
         }

         _errorService.AddErrors(ModelState.Keys, ModelState);
         return await base.OnGetAsync(id);


         bool schoolAndTrustInformationSectionComplete = AcademyConversionProject.SchoolAndTrustInformationSectionComplete != null &&
                                                AcademyConversionProject.SchoolAndTrustInformationSectionComplete.Value;
         if (schoolAndTrustInformationSectionComplete && !Project.HeadTeacherBoardDate.HasValue)
         {
            _errorService.AddError(
               $"/task-list/{id}/confirm-school-trust-information-project-dates/advisory-board-date?return=%2FTaskList%2FSchoolAndTrustInformation/ConfirmSchoolAndTrustInformation&fragment=advisory-board-date",
               "Set an Advisory board date before you generate your project template");
         }

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

      private (string, string) GetReturnPageAndFragment()
      {
         Request.Query.TryGetValue("return", out StringValues returnQuery);
         Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
         return (returnQuery, fragmentQuery);
      }
   }
}
