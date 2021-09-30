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
					"Set an Advisory Board date before you generate your project template");
			}

			var keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(Project.SchoolURN);

			// 16 plus = 6, All-through = 7, Middle deemed primary = 3, Middle deemed secondary = 5, Not applicable = 0, Nursery = 1, Primary = 2, Secondary = 4
			TaskList = TaskListViewModel.Build(Project);
			TaskList.HasKeyStage2PerformanceTables = keyStagePerformance.HasKeyStage2PerformanceTables;
			TaskList.HasKeyStage4PerformanceTables = keyStagePerformance.HasKeyStage4PerformanceTables;
			TaskList.HasKeyStage5PerformanceTables = keyStagePerformance.HasKeyStage5PerformanceTables;

			return Page();
		}
    }
}
