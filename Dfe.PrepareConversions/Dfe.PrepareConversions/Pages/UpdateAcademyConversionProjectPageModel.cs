using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages
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
			get { return TempData["SuccessPage"].ToString(); }
			set { TempData["SuccessPage"] = value; }
		}

		public override async Task<IActionResult> OnPostAsync(int id)
		{
			await SetProject(id);
			
			bool schoolAndTrustInformationSectionComplete = AcademyConversionProject.SchoolAndTrustInformationSectionComplete != null && AcademyConversionProject.SchoolAndTrustInformationSectionComplete.Value;
			if (schoolAndTrustInformationSectionComplete && !Project.HeadTeacherBoardDate.HasValue)
			{
				_errorService.AddError($"/task-list/{id}/confirm-school-trust-information-project-dates/advisory-board-date?return=%2FTaskList%2FSchoolAndTrustInformation/ConfirmSchoolAndTrustInformation&fragment=advisory-board-date",
					"Set an Advisory board date before you generate your project template");
			}
			
			if (AcademyConversionProject.EndOfCurrentFinancialYear.HasValue &&
			    AcademyConversionProject.EndOfNextFinancialYear.HasValue && 
			    AcademyConversionProject.EndOfCurrentFinancialYear != DateTime.MinValue &&
			    AcademyConversionProject.EndOfNextFinancialYear != DateTime.MinValue &&
			    AcademyConversionProject.EndOfCurrentFinancialYear.Value.AddYears(1).AddDays(-1) > AcademyConversionProject.EndOfNextFinancialYear)
			{
				_errorService.AddError($"/task-list/{id}/confirm-school-budget-information/update-school-budget-information?return=%2FTaskList%2FSchoolBudgetInformation/ConfirmSchoolBudgetInformation&fragment=financial-year",
					"The next financial year cannot be before or within a year of the current financial year");
			}		
         

			_errorService.AddErrors(Request.Form.Keys, ModelState);
			if (_errorService.HasErrors())
			{
				return Page();
			}

			var response = await _repository.UpdateProject(id, Build());

			if (!response.Success)
			{
				_errorService.AddTramsError();
				return Page();
			}

			var (returnPage, fragment) = GetReturnPageAndFragment();
			if (!string.IsNullOrWhiteSpace(returnPage))
			{
				return RedirectToPage(returnPage, null, new { id }, fragment);
			}

			return RedirectToPage(SuccessPage, new { id });
		}

		protected UpdateAcademyConversionProject Build()
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
				Form7Received = AcademyConversionProject.Form7Received,
				Form7ReceivedDate = AcademyConversionProject.Form7ReceivedDate,
				ProposedAcademyOpeningDate = AcademyConversionProject.ProposedAcademyOpeningDate,
				PreviousHeadTeacherBoardDateQuestion = AcademyConversionProject.PreviousHeadTeacherBoardDateQuestion,
				PreviousHeadTeacherBoardDate = AcademyConversionProject.PreviousHeadTeacherBoardDateQuestion == "No" ? default(DateTime) : AcademyConversionProject.PreviousHeadTeacherBoardDate,
				SchoolAndTrustInformationSectionComplete = AcademyConversionProject.SchoolAndTrustInformationSectionComplete,
				PublishedAdmissionNumber = AcademyConversionProject.PublishedAdmissionNumber,
				ViabilityIssues = AcademyConversionProject.ViabilityIssues,
				FinancialDeficit = AcademyConversionProject.FinancialDeficit,
				IsThisADiocesanTrust = AcademyConversionProject.IsThisADiocesanTrust,
				DistanceFromSchoolToTrustHeadquarters = AcademyConversionProject.DistanceFromSchoolToTrustHeadquarters,
				DistanceFromSchoolToTrustHeadquartersAdditionalInformation = AcademyConversionProject.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
				MemberOfParliamentName = AcademyConversionProject.MemberOfParliamentName,
				MemberOfParliamentParty = AcademyConversionProject.MemberOfParliamentParty,
				GeneralInformationSectionComplete = AcademyConversionProject.GeneralInformationSectionComplete,
				SchoolPerformanceAdditionalInformation = AcademyConversionProject.SchoolPerformanceAdditionalInformation,
				RationaleForProject = AcademyConversionProject.RationaleForProject,
				RationaleForTrust = AcademyConversionProject.RationaleForTrust,
				RationaleSectionComplete = AcademyConversionProject.RationaleSectionComplete,
				RisksAndIssues = AcademyConversionProject.RisksAndIssues,
				RisksAndIssuesSectionComplete = AcademyConversionProject.RisksAndIssuesSectionComplete,
				LegalRequirementsSectionComplete = AcademyConversionProject.LegalRequirementsSectionComplete, 
				GoverningBodyResolution = AcademyConversionProject.GoverningBodyResolution, 
				Consultation = AcademyConversionProject.Consultation, 
				DiocesanConsent = AcademyConversionProject.DiocesanConsent, 
				FoundationConsent = AcademyConversionProject.FoundationConsent, 
				EndOfCurrentFinancialYear = AcademyConversionProject.EndOfCurrentFinancialYear,
				EndOfNextFinancialYear = AcademyConversionProject.EndOfNextFinancialYear,
				RevenueCarryForwardAtEndMarchCurrentYear = AcademyConversionProject.RevenueCarryForwardAtEndMarchCurrentYear,
				ProjectedRevenueBalanceAtEndMarchNextYear = AcademyConversionProject.ProjectedRevenueBalanceAtEndMarchNextYear,
				CapitalCarryForwardAtEndMarchCurrentYear = AcademyConversionProject.CapitalCarryForwardAtEndMarchCurrentYear,
				CapitalCarryForwardAtEndMarchNextYear = AcademyConversionProject.CapitalCarryForwardAtEndMarchNextYear,
				SchoolBudgetInformationAdditionalInformation = AcademyConversionProject.SchoolBudgetInformationAdditionalInformation,
				SchoolBudgetInformationSectionComplete = AcademyConversionProject.SchoolBudgetInformationSectionComplete,
				SchoolPupilForecastsAdditionalInformation = AcademyConversionProject.SchoolPupilForecastsAdditionalInformation,
				KeyStage2PerformanceAdditionalInformation = AcademyConversionProject.KeyStage2PerformanceAdditionalInformation,
				KeyStage4PerformanceAdditionalInformation = AcademyConversionProject.KeyStage4PerformanceAdditionalInformation,
				KeyStage5PerformanceAdditionalInformation = AcademyConversionProject.KeyStage5PerformanceAdditionalInformation
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
