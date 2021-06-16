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

		public static TaskListItemViewModel ReferenceOnly => new TaskListItemViewModel("Reference Only", "govuk-tag--grey");
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
	}
}
