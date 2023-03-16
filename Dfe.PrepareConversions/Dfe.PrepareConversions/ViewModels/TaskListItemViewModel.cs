using System;
using System.Diagnostics.CodeAnalysis;

namespace Dfe.PrepareConversions.ViewModels;

public sealed class TaskListItemViewModel : IEquatable<TaskListItemViewModel>
{
   private TaskListItemViewModel(string status, string cssClass)
   {
      Status = status;
      CssClass = cssClass;
   }

   public string Status { get; }
   public string CssClass { get; }

   public static TaskListItemViewModel NotStarted => new("Not Started", "govuk-tag--grey");
   public static TaskListItemViewModel InProgress => new("In Progress", "govuk-tag--blue");
   public static TaskListItemViewModel Completed => new("Completed", "");

   public bool Equals([AllowNull] TaskListItemViewModel other)
   {
      if (other == null) return false;

      if (ReferenceEquals(this, other))
      {
         return true;
      }

      return Status.Equals(other.Status) && CssClass.Equals(other.CssClass);
   }

   public override bool Equals(object obj)
   {
      return Equals(obj as TaskListItemViewModel);
   }

   public static TaskListItemViewModel GetLocalAuthorityInformationTemplateTaskListStatus(ProjectViewModel project)
   {
      if (project.LocalAuthorityInformationTemplateSectionComplete)
      {
         return Completed;
      }

      if (!project.LocalAuthorityInformationTemplateSentDate.HasValue
          && !project.LocalAuthorityInformationTemplateReturnedDate.HasValue
          && string.IsNullOrWhiteSpace(project.LocalAuthorityInformationTemplateComments)
          && string.IsNullOrWhiteSpace(project.LocalAuthorityInformationTemplateLink))
      {
         return NotStarted;
      }

      return InProgress;
   }

   public static TaskListItemViewModel GetSchoolAndTrustInformationTaskListStatus(ProjectViewModel project)
   {
      if (project.SchoolAndTrustInformationSectionComplete)
      {
         return Completed;
      }

      if (string.IsNullOrWhiteSpace(project.RecommendationForProject)
          && string.IsNullOrWhiteSpace(project.Author)
          && string.IsNullOrWhiteSpace(project.ClearedBy)
          && string.IsNullOrWhiteSpace(project.AcademyOrderRequired)
          && !project.HeadTeacherBoardDate.HasValue
          && !project.PreviousHeadTeacherBoardDate.HasValue)
      {
         return NotStarted;
      }

      return InProgress;
   }

   public static TaskListItemViewModel GetGeneralInformationTaskListStatus(ProjectViewModel project)
   {
      if (project.GeneralInformationSectionComplete)
      {
         return Completed;
      }

      if (string.IsNullOrWhiteSpace(project.PublishedAdmissionNumber)
          && string.IsNullOrWhiteSpace(project.ViabilityIssues)
          && string.IsNullOrWhiteSpace(project.FinancialDeficit)
          && !project.DistanceFromSchoolToTrustHeadquarters.HasValue)
      {
         return NotStarted;
      }

      return InProgress;
   }

   public static TaskListItemViewModel GetRationaleTaskListStatus(ProjectViewModel project)
   {
      if (project.RationaleSectionComplete)
      {
         return Completed;
      }

      if (string.IsNullOrWhiteSpace(project.RationaleForProject)
          && string.IsNullOrWhiteSpace(project.RationaleForTrust))
      {
         return NotStarted;
      }

      return InProgress;
   }

   public static TaskListItemViewModel GetRisksAndIssuesTaskListStatus(ProjectViewModel project)
   {
      if (project.RisksAndIssuesSectionComplete)
      {
         return Completed;
      }

      if (string.IsNullOrWhiteSpace(project.RisksAndIssues))
      {
         return NotStarted;
      }

      return InProgress;
   }

   public static TaskListItemViewModel GetLegalRequirementsTaskListStatus(ProjectViewModel project)
   {
      if (project.LegalRequirementsSectionComplete)
      {
         return Completed;
      }

      if (string.IsNullOrWhiteSpace(project.GoverningBodyResolution) &&
          string.IsNullOrWhiteSpace(project.Consultation) &&
          string.IsNullOrWhiteSpace(project.DiocesanConsent) &&
          string.IsNullOrWhiteSpace(project.FoundationConsent))
      {
         return NotStarted;
      }

      return InProgress;
   }

   public static TaskListItemViewModel GetSchoolBudgetInformationTaskListStatus(ProjectViewModel project)
   {
      if (project.SchoolBudgetInformationSectionComplete)
      {
         return Completed;
      }

      if (project.EndOfCurrentFinancialYear == null
          && project.RevenueCarryForwardAtEndMarchCurrentYear == null
          && project.CapitalCarryForwardAtEndMarchCurrentYear == null
          && project.EndOfNextFinancialYear == null
          && project.ProjectedRevenueBalanceAtEndMarchNextYear == null
          && project.CapitalCarryForwardAtEndMarchNextYear == null
          && string.IsNullOrWhiteSpace(project.SchoolBudgetInformationAdditionalInformation))
      {
         return NotStarted;
      }

      return InProgress;
   }

   public override int GetHashCode()
   {
      return $"{Status} {CssClass}".GetHashCode();
   }
}
