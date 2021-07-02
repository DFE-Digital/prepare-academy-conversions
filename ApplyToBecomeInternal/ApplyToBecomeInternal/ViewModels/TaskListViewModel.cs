namespace ApplyToBecomeInternal.ViewModels
{
	public class TaskListViewModel
	{
		public TaskListItemViewModel LocalAuthorityInformationTemplateTaskListStatus { get; set; }
		public bool LocalAuthorityInformationTemplateSectionNotStarted => LocalAuthorityInformationTemplateTaskListStatus.Equals(TaskListItemViewModel.NotStarted);
		public TaskListItemViewModel SchoolAndTrustInformationTaskListStatus { get; set; }
		public TaskListItemViewModel GeneralInformationTaskListStatus { get; set; }
		public TaskListItemViewModel RationaleTaskListStatus { get; set; }
		public TaskListItemViewModel RisksAndIssuesTaskListStatus { get; set; }
		public TaskListItemViewModel SchoolBudgetInformationTaskListStatus { get; set; }

		public static TaskListViewModel Build(ProjectViewModel project)
		{
			return new TaskListViewModel
			{
				LocalAuthorityInformationTemplateTaskListStatus = TaskListItemViewModel.GetLocalAuthorityInformationTemplateTaskListStatus(project),
				SchoolAndTrustInformationTaskListStatus = TaskListItemViewModel.GetSchoolAndTrustInformationTaskListStatus(project),
				GeneralInformationTaskListStatus = TaskListItemViewModel.GetGeneralInformationTaskListStatus(project),
				RationaleTaskListStatus = TaskListItemViewModel.GetRationaleTaskListStatus(project),
				RisksAndIssuesTaskListStatus = TaskListItemViewModel.GetRisksAndIssuesTaskListStatus(project),
				SchoolBudgetInformationTaskListStatus = TaskListItemViewModel.GetSchoolBudgetInformationTaskListStatus(project)
			};
		}
	}
}
