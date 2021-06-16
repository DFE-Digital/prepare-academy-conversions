using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.GenerateHTBTemplate;
using System;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ProjectViewModel
	{
		private readonly Dictionary<ProjectPhase, string> _projectPhaseText = new Dictionary<ProjectPhase, string>
		{
			{ProjectPhase.PreHTB, "Pre HTB"},
			{ProjectPhase.PostHTB, "Post HTB"}
		};

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

			LocalAuthorityInformationTemplateSentDate = project.LocalAuthorityInformationTemplateSentDate;
			LocalAuthorityInformationTemplateReturnedDate = project.LocalAuthorityInformationTemplateReturnedDate;
			LocalAuthorityInformationTemplateComments = project.LocalAuthorityInformationTemplateComments;
			LocalAuthorityInformationTemplateLink = project.LocalAuthorityInformationTemplateLink;
			LocalAuthorityInformationTemplateSectionComplete = project.LocalAuthorityInformationTemplateSectionComplete ?? false;
			LocalAuthorityInformationTemplateTaskListStatus = TaskListItemViewModel.GetLocalAuthorityInformationTemplateTaskListStatus(this);

			SchoolPhase = project.SchoolPhase;
			AgeRange = project.AgeRange;
			SchoolType = project.SchoolType;
			PublishedAdmissionNumber = project.PublishedAdmissionNumber;
			PercentageFreeSchoolMeals = project.PercentageFreeSchoolMeals;
			PartOfPfiScheme = project.PartOfPfiScheme;
			ViabilityIssues = project.ViabilityIssues;
			FinancialDeficit = project.FinancialDeficit;
			IsThisADiocesanTrust = project.IsThisADiocesanTrust;
			PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = project.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust;
			DistanceFromSchoolToTrustHeadquarters = project.DistanceFromSchoolToTrustHeadquarters;
			MemberOfParliamentParty = project.MemberOfParliamentParty;
			GeneralInformationSectionComplete = project.GeneralInformationSectionComplete ?? false;
			GeneralInformationTaskListStatus = TaskListItemViewModel.GetGeneralInformationTaskListStatus(this);

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
		public IEnumerable<DocumentDetails> ProjectDocuments { get; set; }

		public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }
		public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; set; }
		public string LocalAuthorityInformationTemplateComments { get; set; }
		public string LocalAuthorityInformationTemplateLink { get; set; }
		public bool LocalAuthorityInformationTemplateSectionComplete { get; set; }
		public TaskListItemViewModel LocalAuthorityInformationTemplateTaskListStatus { get; set; }

		//general info
		public string SchoolPhase { get; set; }
		public string AgeRange { get; set; }
		public string SchoolType { get; set; }
		public string PublishedAdmissionNumber { get; set; }
		public decimal? PercentageFreeSchoolMeals { get; set; }
		public string PartOfPfiScheme { get; set; }
		public string ViabilityIssues { get; set; }
		public string FinancialDeficit { get; set; }
		public bool? IsThisADiocesanTrust { get; set; }
		public decimal? PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		public string MemberOfParliamentParty { get; set; }
		public bool GeneralInformationSectionComplete { get; set; }
		public TaskListItemViewModel GeneralInformationTaskListStatus { get; set; }

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
