using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages
{
	public class UpdateAcademyConversionProjectPageModel : BaseAcademyConversionProjectPageModel
	{
		public UpdateAcademyConversionProjectPageModel(AcademyConversionProjectRepository repository) : base(repository) { }

		[BindProperty]
		public AcademyConversionProjectPostModel AcademyConversionProject { get; set; }

		[BindProperty]
		public string SuccessPage { get; set; }

		public bool ShowError { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var response = await _repository.UpdateProject(id, Build());

			if (!response.Success)
			{
				ShowError = true;
				await SetProject(id);
				return Page();
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
				OpeningDate = AcademyConversionProject.OpeningDate,
				BaselineDate = AcademyConversionProject.BaselineDate,
				LocalAuthorityInformationTemplateSentDate = AcademyConversionProject.LocalAuthorityInformationTemplateSentDate,
				LocalAuthorityInformationTemplateReturnedDate = AcademyConversionProject.LocalAuthorityInformationTemplateReturnedDate,
				LocalAuthorityInformationTemplateComments = AcademyConversionProject.LocalAuthorityInformationTemplateComments,
				LocalAuthorityInformationTemplateLink = AcademyConversionProject.LocalAuthorityInformationTemplateLink,
				LocalAuthorityInformationTemplateSectionComplete = AcademyConversionProject.LocalAuthorityInformationTemplateSectionComplete,
				RecommendationForProject = AcademyConversionProject.RecommendationForProject,
				Author = AcademyConversionProject.Author,
				ClearedBy = AcademyConversionProject.ClearedBy,
				IsAoRequired = AcademyConversionProject.IsAoRequired,
				ProposedAcademyOpeningDate = AcademyConversionProject.ProposedAcademyOpeningDate,
				SchoolAndTrustInformationSectionComplete = AcademyConversionProject.SchoolAndTrustInformationSectionComplete,
				PublishedAdmissionNumber = AcademyConversionProject.PublishedAdmissionNumber,
				ViabilityIssues = AcademyConversionProject.ViabilityIssues,
				FinancialSurplusOrDeficit = AcademyConversionProject.FinancialSurplusOrDeficit,
				IsThisADiocesanTrust = AcademyConversionProject.IsThisADiocesanTrust,
				DistanceFromSchoolToTrustHeadquarters = AcademyConversionProject.DistanceFromSchoolToTrustHeadquarters,
				MemberOfParliamentParty = AcademyConversionProject.MemberOfParliamentParty,
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
				SchoolPupilForecastsAdditionalInformation = AcademyConversionProject.SchoolPupilForecastsAdditionalInformation,
				KeyStagePerformanceTablesAdditionalInformation = AcademyConversionProject.KeyStagePerformanceTablesAdditionalInformation,
			};
		}
	}
}
