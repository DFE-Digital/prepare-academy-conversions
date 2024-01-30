using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dfe.PrepareConversions.Data.Exceptions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Dfe.PrepareConversions.Pages.TaskList.KeyStagePerformance
{
   public class PerformanceDataViewModel : BaseAcademyConversionProjectPageModel
   {
      private readonly ErrorService _errorService;

      public PerformanceDataViewModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
      {
         _errorService = errorService;
      }

      // key stage performance tables
      [BindProperty(Name = "key-stage-2-additional-information")]
      [DisplayFormat(ConvertEmptyStringToNull = false)]
      public string KeyStage2PerformanceAdditionalInformation { get; set; }

      [BindProperty(Name = "key-stage-4-additional-information")]
      [DisplayFormat(ConvertEmptyStringToNull = false)]
      public string KeyStage4PerformanceAdditionalInformation { get; set; }

      [BindProperty(Name = "key-stage-5-additional-information")]
      [DisplayFormat(ConvertEmptyStringToNull = false)]
      public string KeyStage5PerformanceAdditionalInformation { get; set; }

      [BindProperty(Name = "educational-attendance-additional-information")]
      [DisplayFormat(ConvertEmptyStringToNull = false)]
      public string EducationalAttendanceAdditionalInformation { get; set; }

      public bool ShowError => _errorService.HasErrors();

      public string SuccessPage
      {
         get => TempData["SuccessPage"].ToString();
         set => TempData["SuccessPage"] = value;
      }

      public override async Task<IActionResult> OnGetAsync(int id)
      {
         await base.OnGetAsync(id);

         KeyStage2PerformanceAdditionalInformation = Project.KeyStage2PerformanceAdditionalInformation;
         KeyStage4PerformanceAdditionalInformation = Project.KeyStage4PerformanceAdditionalInformation;
         KeyStage5PerformanceAdditionalInformation = Project.KeyStage5PerformanceAdditionalInformation;
         EducationalAttendanceAdditionalInformation = Project.EducationalAttendanceAdditionalInformation;

         return Page();
      }

      public override async Task<IActionResult> OnPostAsync(int id)
      {
         await SetProject(id);

         if (ModelState.IsValid)
         {
            SetExistingValuesIfNotChanged();

            try
            {
               await _repository.SetPerformanceData(id, new SetPerformanceDataModel(id,
                                                                                 KeyStage2PerformanceAdditionalInformation,
                                                                                 KeyStage4PerformanceAdditionalInformation,
                                                                                 KeyStage5PerformanceAdditionalInformation,
                                                                                 EducationalAttendanceAdditionalInformation));

               (string returnPage, string fragment) = GetReturnPageAndFragment();
               if (!string.IsNullOrWhiteSpace(returnPage))
               {
                  return RedirectToPage(returnPage, null, new { id }, fragment);
               }

               return RedirectToPage(SuccessPage, new { id });

            }
            catch (ApiResponseException ex)
            {

               _errorService.AddApiError();
               return Page();
            }

         }

         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      private void SetExistingValuesIfNotChanged()
      {
         if (KeyStage2PerformanceAdditionalInformation is null)
         {
            KeyStage2PerformanceAdditionalInformation = Project.KeyStage2PerformanceAdditionalInformation;
         }

         if (KeyStage4PerformanceAdditionalInformation is null)
         {
            KeyStage4PerformanceAdditionalInformation = Project.KeyStage4PerformanceAdditionalInformation;
         }

         if (KeyStage5PerformanceAdditionalInformation is null)
         {
            KeyStage5PerformanceAdditionalInformation = Project.KeyStage5PerformanceAdditionalInformation;
         }

         if (EducationalAttendanceAdditionalInformation is null)
         {
            EducationalAttendanceAdditionalInformation = Project.EducationalAttendanceAdditionalInformation;
         }
      }

      private (string, string) GetReturnPageAndFragment()
      {
         Request.Query.TryGetValue("return", out StringValues returnQuery);
         Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
         return (returnQuery, fragmentQuery);
      }
   }
}
