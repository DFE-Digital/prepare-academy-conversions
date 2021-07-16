using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class PreviewHTBTemplateModel : BaseAcademyConversionProjectPageModel
	{
		private readonly ErrorService _errorService;

		public PreviewHTBTemplateModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
		{
			_errorService = errorService;
		}

		public bool ShowGenerateHtbTemplateError;
		public string ErrorPage
		{
			set => TempData[nameof(ErrorPage)] = value;
		}

		public override async Task<IActionResult> OnGetAsync(int id)
		{
			await SetProject(id);

			ShowGenerateHtbTemplateError = (bool)(TempData["ShowGenerateHtbTemplateError"] ?? false);
			if (ShowGenerateHtbTemplateError)
			{
				_errorService.AddError($"/task-list/{id}/confirm-school-trust-information-project-dates#head-teacher-board-date",
					"Set an HTB date before you generate your document");
			}

			return await base.OnGetAsync(id);
		}
	}
}
