using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.AcademyConversion;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		private readonly ErrorService _errorService;
		private readonly KeyStagePerformanceService _keyStagePerformanceService;		

		public IndexModel(KeyStagePerformanceService keyStagePerformanceService,
			IAcademyConversionProjectRepository repository,
			ErrorService errorService) : base(repository)
		{
			_keyStagePerformanceService = keyStagePerformanceService;
			_errorService = errorService;
		}

		public bool ShowGenerateHtbTemplateError { get; set; }
		public Status LegalRequirementsStatus { get; set; } = Status.NotStarted;
		public TaskListViewModel TaskList { get; set; }

		public void SetErrorPage(string errorPage)
		{
			TempData["ErrorPage"] = errorPage;
		}

		public override async Task<IActionResult> OnGetAsync(int id)
		{
			IActionResult result = await SetProject(id);

			if ((result as StatusCodeResult)?.StatusCode == (int)HttpStatusCode.NotFound)
			{
				return NotFound();
			}

			ShowGenerateHtbTemplateError = (bool)(TempData["ShowGenerateHtbTemplateError"] ?? false);
			if (ShowGenerateHtbTemplateError)
			{
				string returnPage = WebUtility.UrlEncode(Links.TaskList.Index.Page);
				// this sets the return location for the 'Confirm' button on the HeadTeacherBoardDate page
				_errorService.AddError($"/task-list/{id}/confirm-school-trust-information-project-dates/advisory-board-date?return={returnPage}",
					"Set an Advisory board date before you generate your project template");
			}

			KeyStagePerformance keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(Project?.SchoolURN);
			// 16 plus = 6, All-through = 7, Middle deemed primary = 3, Middle deemed secondary = 5, Not applicable = 0, Nursery = 1, Primary = 2, Secondary = 4
			if (Project != null) TaskList = TaskListViewModel.Build(Project);
			if (TaskList != null)
			{
				TaskList.HasKeyStage2PerformanceTables = keyStagePerformance.HasKeyStage2PerformanceTables;
				TaskList.HasKeyStage4PerformanceTables = keyStagePerformance.HasKeyStage4PerformanceTables;
				TaskList.HasKeyStage5PerformanceTables = keyStagePerformance.HasKeyStage5PerformanceTables;
			}

			return Page();
		}
	}
}
