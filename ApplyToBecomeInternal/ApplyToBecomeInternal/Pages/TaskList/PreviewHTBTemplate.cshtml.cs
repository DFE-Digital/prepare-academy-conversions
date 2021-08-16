using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class PreviewHTBTemplateModel : BaseAcademyConversionProjectPageModel
	{
		private readonly ErrorService _errorService;

		public PreviewHTBTemplateModel(KeyStagePerformanceService keyStagePerformanceService, IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
		{
			_keyStagePerformanceService = keyStagePerformanceService;
			_errorService = errorService;
		}

		public bool ShowGenerateHtbTemplateError;
		public bool ShowKeyStage2PerformanceTables;
		private readonly KeyStagePerformanceService _keyStagePerformanceService;

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

			var keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(Project.SchoolURN);
			ShowKeyStage2PerformanceTables = keyStagePerformance.KeyStage2?.Any(HasKeyStage2PerformanceTables) ?? false;

			return await base.OnGetAsync(id);
		}


		private bool HasKeyStage2PerformanceTables(KeyStage2PerformanceResponse keyStage2Performance)
		{
			return HasValue(keyStage2Performance.PercentageMeetingExpectedStdInRWM)
			       || HasValue(keyStage2Performance.PercentageAchievingHigherStdInRWM)
			       || HasValue(keyStage2Performance.ReadingProgressScore)
			       || HasValue(keyStage2Performance.WritingProgressScore)
			       || HasValue(keyStage2Performance.MathsProgressScore);
		}

		private bool HasValue(DisadvantagedPupilsResponse disadvantagedPupilsResponse)
		{
			return !string.IsNullOrEmpty(disadvantagedPupilsResponse.NotDisadvantaged)
			       || !string.IsNullOrEmpty(disadvantagedPupilsResponse.Disadvantaged);
		}
	}
}
