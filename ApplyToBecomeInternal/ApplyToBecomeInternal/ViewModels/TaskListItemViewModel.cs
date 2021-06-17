using System;
using System.Diagnostics.CodeAnalysis;

namespace ApplyToBecomeInternal.ViewModels
{
	public class TaskListItemViewModel : IEquatable<TaskListItemViewModel>
	{
		private TaskListItemViewModel(string status, string cssClass)
		{
			Status = status;
			CssClass = cssClass;
		}

		public string Status { get; }
		public string CssClass { get; }

		public static TaskListItemViewModel NotStarted => new TaskListItemViewModel("Not Started", "govuk-tag--grey");
		public static TaskListItemViewModel InProgress => new TaskListItemViewModel("In Progress", "govuk-tag--blue");
		public static TaskListItemViewModel Completed => new TaskListItemViewModel("Completed", "");

		public bool Equals([AllowNull] TaskListItemViewModel other)
		{
			if (other == null) return false;

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Status.Equals(other.Status) && CssClass.Equals(other.CssClass);
		}

		public static TaskListItemViewModel GetLocalAuthorityInformationTemplateTaskListStatus(ProjectViewModel project)
		{
			if (project.LocalAuthorityInformationTemplateSectionComplete)
			{
				return Completed;
			}
			else if (!project.LocalAuthorityInformationTemplateSentDate.HasValue
				&& !project.LocalAuthorityInformationTemplateReturnedDate.HasValue
				&& string.IsNullOrWhiteSpace(project.LocalAuthorityInformationTemplateComments)
				&& string.IsNullOrWhiteSpace(project.LocalAuthorityInformationTemplateLink))
			{
				return NotStarted;
			}
			else
			{
				return InProgress;
			}
		}

		public static TaskListItemViewModel GetSchoolAndTrustInformationTaskListStatus(ProjectViewModel project)
		{
			if (project.SchoolAndTrustInformationSectionComplete)
			{
				return Completed;
			}
			else if (string.IsNullOrWhiteSpace(project.RecommendationForProject)
				&& string.IsNullOrWhiteSpace(project.Author)
				&& string.IsNullOrWhiteSpace(project.ClearedBy)
				&& string.IsNullOrWhiteSpace(project.AcademyOrderRequired)
				&& !project.HeadTeacherBoardDate.HasValue
				&& !project.PreviousHeadTeacherBoardDate.HasValue)
			{
				return NotStarted;
			}
			else
			{
				return InProgress;
			}
		}

		public static TaskListItemViewModel GetGeneralInformationTaskListStatus(ProjectViewModel project)
		{
			if (project.GeneralInformationSectionComplete)
			{
				return Completed;
			}
			else if (string.IsNullOrWhiteSpace(project.PublishedAdmissionNumber)
				&& string.IsNullOrWhiteSpace(project.ViabilityIssues)
				&& string.IsNullOrWhiteSpace(project.FinancialDeficit)
				&& !project.IsThisADiocesanTrust.HasValue
				&& !project.DistanceFromSchoolToTrustHeadquarters.HasValue)
			{
				return NotStarted;
			}
			else
			{
				return InProgress;
			}
		}

		public static TaskListItemViewModel GetRationaleTaskListStatus(ProjectViewModel project)
		{
			if (project.RationaleSectionComplete)
			{
				return Completed;
			}
			else if (string.IsNullOrWhiteSpace(project.RationaleForProject) 
				&& string.IsNullOrWhiteSpace(project.RationaleForTrust))
			{
				return NotStarted;
			}
			else
			{
				return InProgress;
			}
		}

		public static TaskListItemViewModel GetRisksAndIssuesTaskListStatus(ProjectViewModel project)
		{
			if (project.RisksAndIssuesSectionComplete)
			{
				return Completed;
			}
			else if (string.IsNullOrWhiteSpace(project.RisksAndIssues))
			{
				return NotStarted;
			}
			else
			{
				return InProgress;
			}
		}

		public static TaskListItemViewModel GetSchoolBudgetInformationTaskListStatus(ProjectViewModel project)
		{
			if (project.SchoolBudgetInformationSectionComplete)
			{
				return Completed;
			}
			else if (project.RevenueCarryForwardAtEndMarchCurrentYear == 0
				&& project.ProjectedRevenueBalanceAtEndMarchNextYear == 0
				&& project.CapitalCarryForwardAtEndMarchCurrentYear == 0
				&& project.CapitalCarryForwardAtEndMarchNextYear == 0
				&& string.IsNullOrWhiteSpace(project.SchoolBudgetInformationAdditionalInformation))
			{
				return NotStarted;
			}
			else
			{
				return InProgress;
			}
		}
	}
}
