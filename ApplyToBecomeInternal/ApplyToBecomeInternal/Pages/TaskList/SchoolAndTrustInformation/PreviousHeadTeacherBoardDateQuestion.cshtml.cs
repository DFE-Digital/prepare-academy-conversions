using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.SchoolAndTrustInformation
{
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

			var response = await _repository.UpdateProject(id, base.Build());

			if (!response.Success)
			{
				_errorService.AddTramsError();
				await SetProject(id);
				return Page();
			}

			var (returnPage, fragment, back) = GetReturnPageAndFragment();
			if (!string.IsNullOrWhiteSpace(returnPage) && IsNo())
			{
				return RedirectToPage(Links.TaskList.PreviewHTBTemplate.Page, new {id});
			}
			if (IsNo())
			{
				return RedirectToPage(Links.SchoolAndTrustInformationSection.ConfirmSchoolAndTrustInformation.Page, new {id});
			}
			if (!string.IsNullOrWhiteSpace(returnPage) && !string.IsNullOrEmpty(back))
			{
				return RedirectToPage(returnPage, null, new { id, @return = back, back = Links.SchoolAndTrustInformationSection.PreviousHeadTeacherBoardDateQuestion.Page }, fragment);
			}
			if (!string.IsNullOrWhiteSpace(returnPage))
			{
				return RedirectToPage(returnPage, null, new { id }, fragment);
			}

			return RedirectToPage(SuccessPage, new { id });
		}

		private (string, string, string) GetReturnPageAndFragment()
		{
			Request.Query.TryGetValue("return", out var returnQuery);
			Request.Query.TryGetValue("fragment", out var fragmentQuery);
			Request.Query.TryGetValue("back", out var backQuery);
			return (returnQuery, fragmentQuery, backQuery);
		}
	}
}
