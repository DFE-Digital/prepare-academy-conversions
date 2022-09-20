using ApplyToBecome.Data.Services;
using ApplyToBecome.Data;
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
	public class PreviewHTBTemplateModel : BaseAcademyConversionProjectPageModel
	{
		private readonly KeyStagePerformanceService _keyStagePerformanceService;
		private readonly ILegalRequirementsRepository _legalRequirementsRepository;
		private readonly ErrorService _errorService;

		public PreviewHTBTemplateModel(KeyStagePerformanceService keyStagePerformanceService, 
			IAcademyConversionProjectRepository repository, ErrorService errorService,
			ILegalRequirementsRepository legalRequirementsRepository) : base(repository)
		{
			_keyStagePerformanceService = keyStagePerformanceService;
			_legalRequirementsRepository = legalRequirementsRepository;
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
				var returnPage = WebUtility.UrlEncode(Links.TaskList.PreviewHTBTemplate.Page);
				// this sets the return location for the 'Confirm' button on the HeadTeacherBoardDate page
				_errorService.AddError($"/task-list/{id}/confirm-school-trust-information-project-dates/advisory-board-date?return={returnPage}&fragment=advisory-board-date",
					"Set an Advisory board date before you generate your project template");
			}

			var keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(Project.SchoolURN);
			ApiResponse<ApplyToBecome.Data.Models.AcademyConversion.LegalRequirements> legalRequirements =
				await _legalRequirementsRepository.GetRequirementsByProjectId(id);
			if (Project != null)
			{
				Project.GoverningBodyResolution = legalRequirements.Body.GoverningBodyApproved.ToDescription();
				Project.Consultation = legalRequirements.Body.ConsultationDone.ToDescription();
				Project.DiocesanConsent = legalRequirements.Body.DiocesanConsent.ToDescription();
				Project.FoundationConsent = legalRequirements.Body.FoundationConsent.ToDescription();
			}
			// 16 plus = 6, All-through = 7, Middle deemed primary = 3, Middle deemed secondary = 5, Not applicable = 0, Nursery = 1, Primary = 2, Secondary = 4
			TaskList = TaskListViewModel.Build(Project);
			TaskList.HasKeyStage2PerformanceTables = keyStagePerformance.HasKeyStage2PerformanceTables;
			TaskList.HasKeyStage4PerformanceTables = keyStagePerformance.HasKeyStage4PerformanceTables;
			TaskList.HasKeyStage5PerformanceTables = keyStagePerformance.HasKeyStage5PerformanceTables;

			return Page();
		}
	}
}
