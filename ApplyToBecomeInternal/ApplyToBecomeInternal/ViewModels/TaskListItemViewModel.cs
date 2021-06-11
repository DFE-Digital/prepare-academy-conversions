namespace ApplyToBecomeInternal.ViewModels
{
	public class TaskListItemViewModel
	{
		private TaskListItemViewModel(string status, string cssClass)
		{
			Status = status;
			CssClass = cssClass;
		}

		public string Status { get; }
		public string CssClass { get; }

		public static TaskListItemViewModel ReferenceOnly => new TaskListItemViewModel("Reference Only", "govuk-tag--grey");
		public static TaskListItemViewModel NotStarted => new TaskListItemViewModel("Not Started", "govuk-tag--grey");
		public static TaskListItemViewModel InProgress => new TaskListItemViewModel("In Progress", "govuk-tag--blue");
		public static TaskListItemViewModel Completed => new TaskListItemViewModel("Completed", "");
	}
}
