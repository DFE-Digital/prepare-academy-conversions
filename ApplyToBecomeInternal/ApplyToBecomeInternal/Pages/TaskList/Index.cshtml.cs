using ApplyToBecome.Data;
using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		private readonly ErrorService _errorService;
		private readonly KeyStagePerformanceService _keyStagePerformanceService;
		private readonly ILegalRequirementsRepository _legalRequirementsRepository;

		public bool ShowGenerateHtbTemplateError;
		public Status LegalRequirementsStatus = Status.NotStarted;

		public IndexModel(KeyStagePerformanceService keyStagePerformanceService,
			IAcademyConversionProjectRepository repository,
			ILegalRequirementsRepository legalRequirementsRepository,
			ErrorService errorService) : base(repository)
		{
			_keyStagePerformanceService = keyStagePerformanceService;
			_legalRequirementsRepository = legalRequirementsRepository;
			_errorService = errorService;
		}

		public TaskListViewModel TaskList { get; set; }

		public string ErrorPage
		{
			set => TempData[nameof(ErrorPage)] = value;
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
			await AttachLegalRequirements(id);
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

		private async Task AttachLegalRequirements(int id)
		{
			ApiResponse<ApplyToBecome.Data.Models.AcademyConversion.LegalRequirements> legalRequirements =
				await _legalRequirementsRepository.GetRequirementsByProjectId(id);
			if (Project != null)
			{
				Project.GoverningBodyResolution = legalRequirements.Body.GoverningBodyApproved.ToString();
				Project.Consultation = legalRequirements.Body.ConsultationDone.ToString();
				Project.DiocesanConsent = legalRequirements.Body.DiocesanConsent.ToString();
				Project.FoundationConsent = legalRequirements.Body.FoundationConsent.ToString();
				Project.LegalRequirementsSectionComplete = legalRequirements.Body.IsComplete;
			}

			if (legalRequirements.Success)
			{
				LegalRequirementsStatus = legalRequirements.Body.Status;
			}
		}
	}
}
