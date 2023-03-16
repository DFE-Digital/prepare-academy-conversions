using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolAndTrustInformation;

public class PreviousHeadTeacherBoardDateQuestion : UpdateAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public PreviousHeadTeacherBoardDateQuestion(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository, errorService)
   {
      _errorService = errorService;
   }

   private bool IsNo()
   {
      return AcademyConversionProject.PreviousHeadTeacherBoardDateQuestion != "Yes";
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      _errorService.AddErrors(Request.Form.Keys, ModelState);
      if (_errorService.HasErrors())
      {
         await SetProject(id);
         return Page();
      }

      ApiResponse<AcademyConversionProject> response = await _repository.UpdateProject(id, Build());

      if (!response.Success)
      {
         _errorService.AddApiError();
         await SetProject(id);
         return Page();
      }

      (string returnPage, string fragment, string back) = GetReturnPageAndFragment();

      if (IsNo())
      {
         return ReturnPage(returnPage)
            ? RedirectToPage(Links.TaskList.PreviewHTBTemplate.Page, null, new { id }, "previous-head-teacher-board")
            : RedirectToPage(Links.SchoolAndTrustInformationSection.ConfirmSchoolAndTrustInformation.Page, new { id });
      }

      if (ReturnPage(returnPage))
      {
         fragment ??= "previous-head-teacher-board";
         return !string.IsNullOrEmpty(back)
            ? RedirectToPage(returnPage, null, new { id, @return = back, back = Links.SchoolAndTrustInformationSection.PreviousHeadTeacherBoardDateQuestion.Page }, fragment)
            : RedirectToPage(returnPage, null, new { id }, fragment);
      }

      return RedirectToPage(SuccessPage, new { id });
   }

   private static bool ReturnPage(string returnPage)
   {
      return !string.IsNullOrWhiteSpace(returnPage);
   }

   private (string, string, string) GetReturnPageAndFragment()
   {
      Request.Query.TryGetValue("return", out StringValues returnQuery);
      Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
      Request.Query.TryGetValue("back", out StringValues backQuery);
      return (returnQuery, fragmentQuery, backQuery);
   }
}
