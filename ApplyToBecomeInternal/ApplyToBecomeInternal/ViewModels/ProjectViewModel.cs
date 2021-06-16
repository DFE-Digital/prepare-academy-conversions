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
			SetLocalAuthorityInformationTemplateTaskListStatus();

			RationaleForProject = project.RationaleForProject;
			RationaleForTrust = project.RationaleForTrust;
			RationaleSectionComplete = project.RationaleSectionComplete ?? false;
			SetRationaleTaskListStatus();
			RisksAndIssues = project.RisksAndIssues;
			EqualitiesImpactAssessmentConsidered = project.EqualitiesImpactAssessmentConsidered;
			RisksAndIssuesSectionComplete = project.RisksAndIssuesSectionComplete ?? false;
			SetRisksAndIssuesTaskListStatus();

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
			SetSchoolBudgetInformationTaskListStatus();
		}

		private void SetLocalAuthorityInformationTemplateTaskListStatus()
		{
			if (LocalAuthorityInformationTemplateSectionComplete)
			{
				LocalAuthorityInformationTemplateTaskListStatus = TaskListItemViewModel.Completed;
			}
			else if (!LocalAuthorityInformationTemplateSentDate.HasValue 
				&& !LocalAuthorityInformationTemplateReturnedDate.HasValue 
				&& string.IsNullOrWhiteSpace(LocalAuthorityInformationTemplateComments) 
				&& string.IsNullOrWhiteSpace(LocalAuthorityInformationTemplateLink))
			{
				LocalAuthorityInformationTemplateTaskListStatus = TaskListItemViewModel.NotStarted;
			}
			else
			{
				LocalAuthorityInformationTemplateTaskListStatus = TaskListItemViewModel.InProgress;
			}
		}

		private void SetRationaleTaskListStatus()
		{
			if (RationaleSectionComplete)
			{
				RationaleTaskListStatus = TaskListItemViewModel.Completed;
			}
			else if (string.IsNullOrWhiteSpace(RationaleForProject) && string.IsNullOrWhiteSpace(RationaleForTrust))
			{
				RationaleTaskListStatus = TaskListItemViewModel.NotStarted;
			}
			else
			{
				RationaleTaskListStatus = TaskListItemViewModel.InProgress;
			}
		}

		private void SetRisksAndIssuesTaskListStatus()
		{
			if (RisksAndIssuesSectionComplete)
			{
				RisksAndIssuesTaskListStatus = TaskListItemViewModel.Completed;
			}
			else if (string.IsNullOrWhiteSpace(RisksAndIssues))
			{
				RisksAndIssuesTaskListStatus = TaskListItemViewModel.NotStarted;
			}
			else
			{
				RisksAndIssuesTaskListStatus = TaskListItemViewModel.InProgress;
			}
		}

		private void SetSchoolBudgetInformationTaskListStatus()
		{
			if (SchoolBudgetInformationSectionComplete)
			{
				SchoolBudgetInformationTaskListStatus = TaskListItemViewModel.Completed;
			}
			else if (string.IsNullOrWhiteSpace(RisksAndIssues))
			{
				SchoolBudgetInformationTaskListStatus = TaskListItemViewModel.NotStarted;
			}
			else
			{
				SchoolBudgetInformationTaskListStatus = TaskListItemViewModel.InProgress;
			}
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
