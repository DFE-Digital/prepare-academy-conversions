using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.ViewModels;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Utils;

public static class SignificantChangeTaskListBuilder
{
   private static readonly IReadOnlyList<SignificantChangeTaskSectionDefinition> TaskSections =
   [
      new SignificantChangeTaskSectionDefinition(
         "consultation",
         "Consultation",
         1,
         [
            new SignificantChangeTaskDefinition(
               "stakeholder-consultation",
               "Stakeholder consultation",
               1,
               Links.SignificantChange.StakeholderConsultation,
               project => GetTaskStatus(project.StakeholderConsultationStatus))
         ]),
      new SignificantChangeTaskSectionDefinition(
         "public-sector-equality-duty",
         "Public Sector Equality Duty",
         3,
         [
            new SignificantChangeTaskDefinition(
               "public-sector-equality-duty",
               "Public Sector Equality Duty",
               1,
               Links.SignificantChange.PublicSectorEqualityDuty,
               project => GetTaskStatus(project.EqualitiesImpactAssessmentStatus))
         ])
   ];

   public static SignificantChangeTaskListViewModel Build(SignificantChangeProjectViewBaseModel project)
   {
      List<SignificantChangeTaskSectionViewModel> sections = [.. TaskSections
         .OrderBy(section => section.DisplayOrder)
         .Select(section => new SignificantChangeTaskSectionViewModel
         {
            Key = section.Key,
            Title = section.Title,
            DisplayOrder = section.DisplayOrder,
            Tasks = [.. section.Tasks
               .Where(task => task.IsVisible(project))
               .OrderBy(task => task.DisplayOrder)
               .Select(task => new SignificantChangeTaskItemViewModel
               {
                  Key = task.Key,
                  Title = task.Title,
                  Page = task.Link.Page,
                  DisplayOrder = task.DisplayOrder,
                  Status = task.GetStatus(project)
               })]
         })
         .Where(section => section.Tasks.Any())];

      return new SignificantChangeTaskListViewModel
      {
         Sections = sections
      };
   }

   private sealed record SignificantChangeTaskSectionDefinition(
      string Key,
      string Title,
      int DisplayOrder,
      IReadOnlyList<SignificantChangeTaskDefinition> Tasks);

   private sealed record SignificantChangeTaskDefinition(
      string Key,
      string Title,
      int DisplayOrder,
      LinkItem Link,
      Func<SignificantChangeProjectViewBaseModel, TaskListItemViewModel> GetStatus)
   {
      public Func<SignificantChangeProjectViewBaseModel, bool> IsVisible { get; init; } = _ => true;
   }

    private static TaskListItemViewModel GetTaskStatus(SignificantChangeTaskStatus status)
    {
        return status switch
        {
            SignificantChangeTaskStatus.Completed => TaskListItemViewModel.Completed,
            SignificantChangeTaskStatus.InProgress => TaskListItemViewModel.InProgress,
            _ => TaskListItemViewModel.NotStarted
        };
    }
}
