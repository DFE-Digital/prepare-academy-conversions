using ApplyToBecome.Data.Models;
using System;

namespace ApplyToBecomeInternal.ViewModels
{

	public class ProjectViewModel
	{
		public ProjectViewModel(AcademyConversionProject project)
		{
			Id = project.Id.ToString();
			//TrustName = project.Trust.Name;
			SchoolName = project.SchoolName;
			SchoolURN = project.Urn.ToString();
			LocalAuthority = project.LocalAuthority;
			ApplicationReceivedDate = FormatDate(project.ApplicationReceivedDate);
			AssignedDate = FormatDate(project.AssignedDate);
			Phase = project.ProjectStatus;
			//ProjectDocuments = project.ProjectDocuments;
			HeadTeacherBoardDate = project.HeadTeacherBoardDate;

			LocalAuthorityInformationTemplateSentDate = project.LocalAuthorityInformationTemplateSentDate;
			LocalAuthorityInformationTemplateReturnedDate = project.LocalAuthorityInformationTemplateReturnedDate;
			LocalAuthorityInformationTemplateComments = project.LocalAuthorityInformationTemplateComments;
			LocalAuthorityInformationTemplateLink = project.LocalAuthorityInformationTemplateLink;
			LocalAuthorityInformationTemplateSectionComplete = project.LocalAuthorityInformationTemplateSectionComplete ?? false;
			LocalAuthorityInformationTemplateTaskListStatus = TaskListItemViewModel.GetLocalAuthorityInformationTemplateTaskListStatus(this);

			RecommendationForProject = project.RecommendationForProject;
			Author = project.Author;
			Version = project.Version;
			ClearedBy = project.ClearedBy;
			AcademyOrderRequired = project.AcademyOrderRequired;
			PreviousHeadTeacherBoardDate = project.PreviousHeadTeacherBoardDate;
			PreviousHeadTeacherBoardLink = project.PreviousHeadTeacherBoardLink;
			TrustReferenceNumber = project.TrustReferenceNumber;
			NameOfTrust = project.NameOfTrust;
			SponsorReferenceNumber = project.SponsorReferenceNumber;
			SponsorName = project.SponsorName;
			AcademyTypeAndRoute = project.AcademyTypeAndRoute;
			ProposedAcademyOpeningDate = project.ProposedAcademyOpeningDate;
			SchoolAndTrustInformationSectionComplete = project.SchoolAndTrustInformationSectionComplete ?? false;
			SchoolAndTrustInformationTaskListStatus = TaskListItemViewModel.GetSchoolAndTrustInformationTaskListStatus(this);

			PublishedAdmissionNumber = project.PublishedAdmissionNumber;
			ViabilityIssues = project.ViabilityIssues;
			FinancialDeficit = project.FinancialDeficit;
			DistanceFromSchoolToTrustHeadquarters = project.DistanceFromSchoolToTrustHeadquarters;
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation;
			GeneralInformationSectionComplete = project.GeneralInformationSectionComplete ?? false;
			GeneralInformationTaskListStatus = TaskListItemViewModel.GetGeneralInformationTaskListStatus(this);

			SchoolPerformanceAdditionalInformation = project.SchoolPerformanceAdditionalInformation;

			RationaleForProject = project.RationaleForProject;
			RationaleForTrust = project.RationaleForTrust;
			RationaleSectionComplete = project.RationaleSectionComplete ?? false;
			RationaleTaskListStatus = TaskListItemViewModel.GetRationaleTaskListStatus(this);

			RisksAndIssues = project.RisksAndIssues;
			EqualitiesImpactAssessmentConsidered = project.EqualitiesImpactAssessmentConsidered;
			RisksAndIssuesSectionComplete = project.RisksAndIssuesSectionComplete ?? false;
			RisksAndIssuesTaskListStatus = TaskListItemViewModel.GetRisksAndIssuesTaskListStatus(this);

			CurrentYearCapacity = project.CurrentYearCapacity;
			CurrentYearPupilNumbers = project.CurrentYearPupilNumbers;
			YearOneProjectedCapacity = project.YearOneProjectedCapacity;
			YearOneProjectedPupilNumbers = project.YearOneProjectedPupilNumbers;
			YearTwoProjectedCapacity = project.YearTwoProjectedCapacity;
			YearTwoProjectedPupilNumbers = project.YearTwoProjectedPupilNumbers;
			YearThreeProjectedCapacity = project.YearThreeProjectedCapacity;
			YearThreeProjectedPupilNumbers = project.YearThreeProjectedPupilNumbers;
			YearFourProjectedCapacity = project.YearFourProjectedCapacity;
			YearFourProjectedPupilNumbers = project.YearFourProjectedPupilNumbers;
			SchoolPupilForecastsAdditionalInformation = project.SchoolPupilForecastsAdditionalInformation;

			RevenueCarryForwardAtEndMarchCurrentYear = project.RevenueCarryForwardAtEndMarchCurrentYear ?? 0;
			ProjectedRevenueBalanceAtEndMarchNextYear = project.ProjectedRevenueBalanceAtEndMarchNextYear ?? 0;
			CapitalCarryForwardAtEndMarchCurrentYear = project.CapitalCarryForwardAtEndMarchCurrentYear ?? 0;
			CapitalCarryForwardAtEndMarchNextYear = project.CapitalCarryForwardAtEndMarchNextYear ?? 0;
			SchoolBudgetInformationAdditionalInformation = project.SchoolBudgetInformationAdditionalInformation;
			SchoolBudgetInformationSectionComplete = project.SchoolBudgetInformationSectionComplete ?? false;
			SchoolBudgetInformationTaskListStatus = TaskListItemViewModel.GetSchoolBudgetInformationTaskListStatus(this);
		}

		private static string FormatDate(DateTime? dateTime) => dateTime.HasValue ? dateTime.Value.ToString("dd MMMM yyyy") : "";

		public string Id { get; }
		public string TrustName { get; }
		public string SchoolName { get; }
		public string SchoolURN { get; }
		public string LocalAuthority { get; }
		public string ApplicationReceivedDate { get; }
		public string AssignedDate { get; }
		public string Phase { get; }
		public DateTime? HeadTeacherBoardDate { get; set; }

		public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }
		public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; set; }
		public string LocalAuthorityInformationTemplateComments { get; set; }
		public string LocalAuthorityInformationTemplateLink { get; set; }
		public bool LocalAuthorityInformationTemplateSectionComplete { get; set; }
		public TaskListItemViewModel LocalAuthorityInformationTemplateTaskListStatus { get; set; }

		//school/trust info
		public string RecommendationForProject { get; set; }
		public string Author { get; set; }
		public string Version { get; set; }
		public string ClearedBy { get; set; }
		public string AcademyOrderRequired { get; set; }
		public DateTime? PreviousHeadTeacherBoardDate { get; set; }
		public string PreviousHeadTeacherBoardLink { get; set; }
		public string TrustReferenceNumber { get; set; }
		public string NameOfTrust { get; set; }
		public string SponsorReferenceNumber { get; set; }
		public string SponsorName { get; set; }
		public string AcademyTypeAndRoute { get; set; }
		public DateTime? ProposedAcademyOpeningDate { get; set; }
		public bool SchoolAndTrustInformationSectionComplete { get; set; }
		public TaskListItemViewModel SchoolAndTrustInformationTaskListStatus { get; set; }

		//general info
		public string PublishedAdmissionNumber { get; set; }
		public string ViabilityIssues { get; set; }
		public string FinancialDeficit { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		public bool GeneralInformationSectionComplete { get; set; }
		public TaskListItemViewModel GeneralInformationTaskListStatus { get; set; }

		//school performance ofsted information
		public string SchoolPerformanceAdditionalInformation { get; set; }

		public string RationaleForProject { get; set; }
		public string RationaleForTrust { get; set; }
		public bool RationaleSectionComplete { get; set; }
		public TaskListItemViewModel RationaleTaskListStatus { get; set; }

		// risk and issues
		public string RisksAndIssues { get; set; }
		public string EqualitiesImpactAssessmentConsidered { get; set; }
		public bool RisksAndIssuesSectionComplete { get; set; }
		public TaskListItemViewModel RisksAndIssuesTaskListStatus { get; set; }

		// pupil schools forecast
		public int? CurrentYearCapacity { get; set; }
		public int? CurrentYearPupilNumbers { get; set; }
		public int? YearOneProjectedCapacity { get; set; }
		public int? YearOneProjectedPupilNumbers { get; set; }
		public int? YearTwoProjectedCapacity { get; set; }
		public int? YearTwoProjectedPupilNumbers { get; set; }
		public int? YearThreeProjectedCapacity { get; set; }
		public int? YearThreeProjectedPupilNumbers { get; set; }
		public int? YearFourProjectedCapacity { get; set; }
		public int? YearFourProjectedPupilNumbers { get; set; }
		public string SchoolPupilForecastsAdditionalInformation { get; set; }

		//school budget info
		public decimal RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
		public decimal CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal CapitalCarryForwardAtEndMarchNextYear { get; set; }
		public string SchoolBudgetInformationAdditionalInformation { get; set; }
		public bool SchoolBudgetInformationSectionComplete { get; set; }
		public TaskListItemViewModel SchoolBudgetInformationTaskListStatus { get; set; }
	}
}
