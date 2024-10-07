using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dfe.PrepareConversions.Data.Exceptions;

namespace Dfe.PrepareConversions.Pages.TaskList.KeyStagePerformance
{
   public class PerformanceDataViewModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : BaseAcademyConversionProjectPageModel(repository)
   {
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

      public bool ShowError => errorService.HasErrors();

      public string SuccessPage
      {
         get => TempData["SuccessPage"].ToString();
         set => TempData["SuccessPage"] = value;
      }

      public override async Task<IActionResult> OnGetAsync(int id)
      {
         await base.OnGetAsync(id);

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

               errorService.AddApiError();
               return Page();
            }

         }

         errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      private void SetExistingValuesIfNotChanged()
      {
         KeyStage2PerformanceAdditionalInformation ??= Project.KeyStage2PerformanceAdditionalInformation;

         KeyStage4PerformanceAdditionalInformation ??= Project.KeyStage4PerformanceAdditionalInformation;

         KeyStage5PerformanceAdditionalInformation ??= Project.KeyStage5PerformanceAdditionalInformation;

         EducationalAttendanceAdditionalInformation ??= Project.EducationalAttendanceAdditionalInformation;
      }

      private (string, string) GetReturnPageAndFragment()
      {
         Request.Query.TryGetValue("return", out StringValues returnQuery);
         Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
         return (returnQuery, fragmentQuery);
      }
   }
}
