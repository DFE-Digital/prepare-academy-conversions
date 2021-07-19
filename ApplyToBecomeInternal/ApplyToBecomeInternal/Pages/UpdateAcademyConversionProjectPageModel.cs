using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages
{
	public class UpdateAcademyConversionProjectPageModel : BaseAcademyConversionProjectPageModel
	{
		private readonly ErrorService _errorService;

		public UpdateAcademyConversionProjectPageModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
		{
			_errorService = errorService;
		}

		[BindProperty]
		public AcademyConversionProjectPostModel AcademyConversionProject { get; set; }

		public bool ShowError => _errorService.HasErrors();

		public string SuccessPage
		{
			get
			{
				return TempData[nameof(SuccessPage)].ToString();
			}
			set
			{
				TempData[nameof(SuccessPage)] = value;
			}
		}

		public virtual async Task<IActionResult> OnPostAsync(int id)
		{
			_errorService.AddErrors(Request.Form.Keys, ModelState);
			if (_errorService.HasErrors())
			{
				await SetProject(id);
				return Page();
			}

			var response = await _repository.UpdateProject(id, Build());

			if (!response.Success)
			{
				_errorService.AddTramsError();
				await SetProject(id);
				return Page();
			}

			var (returnPage, fragment) = GetReturnPageAndFragment();
			if (!string.IsNullOrWhiteSpace(returnPage))
			{
				return RedirectToPage(returnPage, null, new { id }, fragment);
			}

			return RedirectToPage(SuccessPage, new { id });
		}

		private UpdateAcademyConversionProject Build()
		{
			return new UpdateAcademyConversionProject
			{
				ProjectStatus = AcademyConversionProject.ProjectStatus,
				ApplicationReceivedDate = AcademyConversionProject.ApplicationReceivedDate,
				AssignedDate = AcademyConversionProject.AssignedDate,
				HeadTeacherBoardDate = AcademyConversionProject.HeadTeacherBoardDate,
				BaselineDate = AcademyConversionProject.BaselineDate,
				LocalAuthorityInformationTemplateSentDate = AcademyConversionProject.LocalAuthorityInformationTemplateSentDate,
				LocalAuthorityInformationTemplateReturnedDate = AcademyConversionProject.LocalAuthorityInformationTemplateReturnedDate,
				LocalAuthorityInformationTemplateComments = AcademyConversionProject.LocalAuthorityInformationTemplateComments,
				LocalAuthorityInformationTemplateLink = AcademyConversionProject.LocalAuthorityInformationTemplateLink,
				LocalAuthorityInformationTemplateSectionComplete = AcademyConversionProject.LocalAuthorityInformationTemplateSectionComplete,
				RecommendationForProject = AcademyConversionProject.RecommendationForProject,
				Author = AcademyConversionProject.Author,
				ClearedBy = AcademyConversionProject.ClearedBy,
				AcademyOrderRequired = AcademyConversionProject.AcademyOrderRequired,
				ProposedAcademyOpeningDate = AcademyConversionProject.ProposedAcademyOpeningDate,
				SchoolAndTrustInformationSectionComplete = AcademyConversionProject.SchoolAndTrustInformationSectionComplete,
				PublishedAdmissionNumber = AcademyConversionProject.PublishedAdmissionNumber,
				ViabilityIssues = AcademyConversionProject.ViabilityIssues,
				FinancialDeficit = AcademyConversionProject.FinancialDeficit,
				IsThisADiocesanTrust = AcademyConversionProject.IsThisADiocesanTrust,
				DistanceFromSchoolToTrustHeadquarters = AcademyConversionProject.DistanceFromSchoolToTrustHeadquarters,
				DistanceFromSchoolToTrustHeadquartersAdditionalInformation = AcademyConversionProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
				GeneralInformationSectionComplete = AcademyConversionProject.GeneralInformationSectionComplete,
				SchoolPerformanceAdditionalInformation = AcademyConversionProject.SchoolPerformanceAdditionalInformation,
				RationaleForProject = AcademyConversionProject.RationaleForProject,
				RationaleForTrust = AcademyConversionProject.RationaleForTrust,
				RationaleSectionComplete = AcademyConversionProject.RationaleSectionComplete,
				RisksAndIssues = AcademyConversionProject.RisksAndIssues,
				RisksAndIssuesSectionComplete = AcademyConversionProject.RisksAndIssuesSectionComplete,
				RevenueCarryForwardAtEndMarchCurrentYear = AcademyConversionProject.RevenueCarryForwardAtEndMarchCurrentYear,
				ProjectedRevenueBalanceAtEndMarchNextYear = AcademyConversionProject.ProjectedRevenueBalanceAtEndMarchNextYear,
				CapitalCarryForwardAtEndMarchCurrentYear = AcademyConversionProject.CapitalCarryForwardAtEndMarchCurrentYear,
				CapitalCarryForwardAtEndMarchNextYear = AcademyConversionProject.CapitalCarryForwardAtEndMarchNextYear,
				SchoolBudgetInformationAdditionalInformation = AcademyConversionProject.SchoolBudgetInformationAdditionalInformation,
				SchoolBudgetInformationSectionComplete = AcademyConversionProject.SchoolBudgetInformationSectionComplete,
				SchoolPupilForecastsAdditionalInformation = AcademyConversionProject.SchoolPupilForecastsAdditionalInformation,
				KeyStagePerformanceTablesAdditionalInformation = AcademyConversionProject.KeyStagePerformanceTablesAdditionalInformation,
			};
		}

		private (string, string) GetReturnPageAndFragment()
		{
			Request.Query.TryGetValue("return", out var returnQuery);
			Request.Query.TryGetValue("fragment", out var fragmentQuery);
			return (returnQuery, fragmentQuery);
		}
	}
}
