using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SignificantChangeTaskListBuilderTests
{
   [Fact]
   public void Build_Includes_consultation_section_with_stakeholder_consultation_task()
   {
      SignificantChangeProjectViewBaseModel project = BuildProject();

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Equal("consultation", result.Sections[0].Key);
      Assert.Single(result.Sections[0].Tasks);
      Assert.Equal("stakeholder-consultation", result.Sections[0].Tasks[0].Key);
   }

   [Fact]
   public void Build_Includes_proposed_decsion_and_conversion_dates_section_with_task()
   {
      string[] expectedTasks = [
         "confirm-project-dates"
      ];

      SignificantChangeProjectViewBaseModel project = BuildProject();

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);
      var section = result.Sections[1];

      Assert.Equal("Proposed decision and conversion dates", section.Key);
      Assert.Equal(expectedTasks.Length, section.Tasks.Count);
      Assert.Equal(expectedTasks, section.Tasks.Select(t => t.Key));
   }

   [Fact]
   public void Build_Sets_task_status_to_completed_when_status_is_completed()
   {
      SignificantChangeProjectViewBaseModel project = BuildProject(stakeholderConsultationStatus: SignificantChangeTaskStatus.Completed);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Equal(TaskListItemViewModel.Completed, result.Sections[0].Tasks[0].Status);
   }

   [Fact]
   public void Build_Sets_task_status_to_in_progress_when_status_is_in_progress()
   {
      SignificantChangeProjectViewBaseModel project = BuildProject(stakeholderConsultationStatus: SignificantChangeTaskStatus.InProgress);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Equal(TaskListItemViewModel.InProgress, result.Sections[0].Tasks[0].Status);
   }

   [Fact]
   public void Build_Sets_task_status_to_not_started_when_status_is_not_started()
   {
      SignificantChangeProjectViewBaseModel project = BuildProject(stakeholderConsultationStatus: SignificantChangeTaskStatus.NotStarted);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Equal(TaskListItemViewModel.NotStarted, result.Sections[0].Tasks[0].Status);
   }

   [Fact]
   public void Build_Sets_task_status_to_not_started_when_status_is_default()
   {
      SignificantChangeProjectViewBaseModel defaultStatusProject = BuildProject();

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(defaultStatusProject);

      Assert.Equal(TaskListItemViewModel.NotStarted, result.Sections[0].Tasks[0].Status);
   }

   [Fact]
   public void Build_Includes_project_dates_task_with_not_started_status_when_dates_are_not_set()
   {
      SignificantChangeProjectViewBaseModel project = BuildProject(projectDatesStatus: SignificantChangeTaskStatus.NotStarted);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Equal("confirm-project-dates", result.Sections[1].Tasks[0].Key);
      Assert.Equal(TaskListItemViewModel.NotStarted, result.Sections[1].Tasks[0].Status);
   }

   [Fact]
   public void Build_Sets_project_dates_task_status_to_completed_when_status_is_completed()
   {
      SignificantChangeProjectViewBaseModel project = BuildProject(projectDatesStatus: SignificantChangeTaskStatus.Completed);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Equal(TaskListItemViewModel.Completed, result.Sections[1].Tasks[0].Status);
   }

   [Fact]
   public void Build_Sets_project_dates_task_status_to_in_progress_when_status_is_in_progress()
   {
      SignificantChangeProjectViewBaseModel project = BuildProject(projectDatesStatus: SignificantChangeTaskStatus.InProgress);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Equal(TaskListItemViewModel.InProgress, result.Sections[1].Tasks[0].Status);
   }

   private static SignificantChangeProjectViewBaseModel BuildProject(
      SignificantChangeTaskStatus stakeholderConsultationStatus = SignificantChangeTaskStatus.NotStarted,
      SignificantChangeTaskStatus projectDatesStatus = SignificantChangeTaskStatus.NotStarted)
   {
      return new SignificantChangeProjectViewBaseModel
      {
         Id = 1,
         Urn = 10000001,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Test trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         Status = "Pre decision",
         StatusColour = "yellow",
         StakeholderConsultationStatus = stakeholderConsultationStatus,
         ProjectDatesStatus = projectDatesStatus
      };
   }
}
