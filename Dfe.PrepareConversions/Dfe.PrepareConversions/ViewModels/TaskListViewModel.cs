using Dfe.PrepareConversions.Utils;

namespace Dfe.PrepareConversions.ViewModels;

public class TaskListViewModel
{
   public TaskListItemViewModel LocalAuthorityInformationTemplateTaskListStatus { get; set; }
   public bool LocalAuthorityInformationTemplateSectionNotStarted => LocalAuthorityInformationTemplateTaskListStatus.Equals(TaskListItemViewModel.NotStarted);
   public TaskListItemViewModel SchoolAndTrustInformationTaskListStatus { get; set; }
   public TaskListItemViewModel SchoolOverviewTaskListStatus { get; set; }
   public TaskListItemViewModel RationaleTaskListStatus { get; set; }
   public TaskListItemViewModel RisksAndIssuesTaskListStatus { get; set; }
   public TaskListItemViewModel LegalRequirementsTaskListStatus { get; set; }
   public TaskListItemViewModel SchoolBudgetInformationTaskListStatus { get; set; }
   public TaskListItemViewModel ProjectDatesTaskListTaskListStatus { get; set; }


   
   public string ProjectStatus { get; set; }
   public string ProjectStatusColour { get; set; }
   public bool HasKeyStage2PerformanceTables { get; set; }
   public bool HasKeyStage4PerformanceTables { get; set; }
   public bool HasKeyStage5PerformanceTables { get; set; }
   public string PerformanceDataHintText { get; set; }
   public bool HasAbsenceData { get; set; }

   public static TaskListViewModel Build(ProjectViewModel project, bool hasSchoolAbsenceData)
   {
      return new TaskListViewModel
      {
         LocalAuthorityInformationTemplateTaskListStatus = TaskListItemViewModel.GetLocalAuthorityInformationTemplateTaskListStatus(project),
         SchoolAndTrustInformationTaskListStatus = TaskListItemViewModel.GetSchoolAndTrustInformationTaskListStatus(project),
         SchoolOverviewTaskListStatus = TaskListItemViewModel.GetSchoolOverviewTaskListStatus(project),
         RationaleTaskListStatus = TaskListItemViewModel.GetRationaleTaskListStatus(project),
         RisksAndIssuesTaskListStatus = TaskListItemViewModel.GetRisksAndIssuesTaskListStatus(project),
         LegalRequirementsTaskListStatus = TaskListItemViewModel.GetLegalRequirementsTaskListStatus(project),
         SchoolBudgetInformationTaskListStatus = TaskListItemViewModel.GetSchoolBudgetInformationTaskListStatus(project),
         ProjectStatus = ProjectListHelper.MapProjectStatus(project.ProjectStatus).Value,
         ProjectStatusColour = ProjectListHelper.MapProjectStatus(project.ProjectStatus).Colour,
         PerformanceDataHintText = ProjectListHelper.MapPerformanceDataHint(project.SchoolType, hasSchoolAbsenceData),
         ProjectDatesTaskListTaskListStatus = TaskListItemViewModel.GetProjectDatesTaskListStatus(project)
      };
   }
}
