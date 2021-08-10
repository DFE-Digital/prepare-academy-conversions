using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
    {
		private readonly KeyStagePerformanceService _keyStagePerformanceService;
		private readonly ErrorService _errorService;

		public IndexModel(KeyStagePerformanceService keyStagePerformanceService, IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
		{
			_keyStagePerformanceService = keyStagePerformanceService;
			_errorService = errorService;
		}

		public TaskListViewModel TaskList { get; set; }
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

			var keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(Project.SchoolURN);

			// 16 plus = 6, All-through = 7, Middle deemed primary = 3, Middle deemed secondary = 5, Not applicable = 0, Nursery = 1, Primary = 2, Secondary = 4
			TaskList = TaskListViewModel.Build(Project);
			TaskList.HasKeyStage2PerformanceTables = keyStagePerformance.KeyStage2?.Any(HasKeyStage2PerformanceTables) ?? false;
			TaskList.HasKeyStage4PerformanceTables = keyStagePerformance.KeyStage4?.Any(HasKeyStage4PerformanceTables) ?? false;
			TaskList.HasKeyStage5PerformanceTables = keyStagePerformance.KeyStage5?.Any(HasKeyStage5PerformanceTables) ?? false;

			return Page();
		}

		private bool HasKeyStage2PerformanceTables(KeyStage2PerformanceResponse keyStage2Performance)
		{
			return HasValue(keyStage2Performance.PercentageMeetingExpectedStdInRWM)
				|| HasValue(keyStage2Performance.PercentageAchievingHigherStdInRWM)
				|| HasValue(keyStage2Performance.ReadingProgressScore)
				|| HasValue(keyStage2Performance.WritingProgressScore)
				|| HasValue(keyStage2Performance.MathsProgressScore);
		}

		private bool HasKeyStage4PerformanceTables(KeyStage4PerformanceResponse keyStage4Performance)
		{
			return HasValue(keyStage4Performance.SipAttainment8score)
			       || HasValue(keyStage4Performance.SipAttainment8scoreenglish)
			       || HasValue(keyStage4Performance.SipAttainment8scoremaths)
			       || HasValue(keyStage4Performance.SipAttainment8scoreebacc)
			       || HasValue(keyStage4Performance.SipNumberofpupilsprogress8)
			       || HasValue(keyStage4Performance.SipProgress8Score)
			       || HasValue(keyStage4Performance.SipProgress8english)
			       || HasValue(keyStage4Performance.SipProgress8maths)
			       || HasValue(keyStage4Performance.SipProgress8ebacc);
		}

		private bool HasKeyStage5PerformanceTables(KeyStage5PerformanceResponse keyStage5Performance)
		{
			return keyStage5Performance.AcademicQualificationAverage != null
			       || keyStage5Performance.AppliedGeneralQualificationAverage != null
			       || keyStage5Performance.NationalAcademicQualificationAverage != null
			       || keyStage5Performance.NationalAppliedGeneralQualificationAverage != null
			       || keyStage5Performance.LAAcademicQualificationAverage != null
			       || keyStage5Performance.LAAppliedGeneralQualificationAverage != null;
		}

		private bool HasValue(DisadvantagedPupilsResponse disadvantagedPupilsResponse)
		{
			return !string.IsNullOrEmpty(disadvantagedPupilsResponse.NotDisadvantaged)
				|| !string.IsNullOrEmpty(disadvantagedPupilsResponse.Disadvantaged);
		}
    }
}
