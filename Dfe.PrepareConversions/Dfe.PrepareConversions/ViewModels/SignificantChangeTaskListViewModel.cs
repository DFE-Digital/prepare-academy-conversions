using System.Collections.Generic;

namespace Dfe.PrepareConversions.ViewModels;

public class SignificantChangeTaskListViewModel
{
   public IReadOnlyList<SignificantChangeTaskSectionViewModel> Sections { get; init; } = [];
}

public class SignificantChangeTaskSectionViewModel
{
   public required string Key { get; init; }
   public required string Title { get; init; }
   public int DisplayOrder { get; init; }
   public IReadOnlyList<SignificantChangeTaskItemViewModel> Tasks { get; init; } = [];
}

public class SignificantChangeTaskItemViewModel
{
   public required string Key { get; init; }
   public required string Title { get; init; }
   public required string Page { get; init; }
   public int DisplayOrder { get; init; }
   public required TaskListItemViewModel Status { get; init; }

   public string StatusId => $"task-status-{Key}";
}
