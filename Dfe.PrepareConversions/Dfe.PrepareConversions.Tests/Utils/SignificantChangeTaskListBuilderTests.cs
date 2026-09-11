using System;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using Xunit;
using System.Linq;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SignificantChangeTaskListBuilderTests
{
   
   [Theory]
   [InlineData(1, "consultation", "Consultation", new[] { "stakeholder-consultation", "stakeholder-objections", "religious-body-consultation" })]
   [InlineData(5, "Proposed decision and conversion dates", "Proposed decision and conversion dates", new[] { "confirm-project-dates" })]
   [InlineData(10, "public-sector-equality-duty", "Public Sector Equality Duty", new[] { "public-sector-equality-duty" })]
   public void Build_includes_ordered_sections_and_tasks_when_supplied(int sectionDisplayOrder, string sectionKey, string sectionTitle, string[] taskKeys)
   {
      var expectedSectionCount = 3;

      SignificantChangeProjectViewBaseModel project = BuildProject();
      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Equal(expectedSectionCount, result.Sections.Count);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      
      Assert.Equal(sectionDisplayOrder, section.DisplayOrder);
      Assert.Equal(sectionTitle, section.Title);
      
      var tasks = section.Tasks;

      Assert.Equal(taskKeys.Length, tasks.Count);
      Assert.Equal(taskKeys, tasks.OrderBy(t => t.DisplayOrder).Select(t => t.Key).ToArray());
   }

   public static TheoryData<SignificantChangeTaskStatus, TaskListItemViewModel> StatusCases =>
    new()
    {
         { SignificantChangeTaskStatus.Completed, TaskListItemViewModel.Completed },
         { SignificantChangeTaskStatus.InProgress, TaskListItemViewModel.InProgress },
         { SignificantChangeTaskStatus.NotStarted, TaskListItemViewModel.NotStarted },
         {default, TaskListItemViewModel.NotStarted }
    };

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_stakeholder_consultation_status_to_task_status(SignificantChangeTaskStatus TaskStatus, TaskListItemViewModel expectedTaskStatus)
   {
      const string sectionKey = "consultation";
      const string taskKey = "stakeholder-consultation";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.StakeholderConsultationStatus = TaskStatus);
      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_stakeholder_objections_status_to_task_status(SignificantChangeTaskStatus TaskStatus, TaskListItemViewModel expectedTaskStatus)
   {
      const string sectionKey = "consultation";
      const string taskKey = "stakeholder-objections";
      
      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.StakeholderObjectionsStatus = TaskStatus);
      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_religious_body_consultation_status_to_task_status(SignificantChangeTaskStatus TaskStatus, TaskListItemViewModel expectedTaskStatus)
   {
      const string sectionKey = "consultation";
      const string taskKey = "religious-body-consultation";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.ReligiousBodyConsultationStatus = TaskStatus);
      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_confirm_project_dates_status_to_task_status(SignificantChangeTaskStatus TaskStatus, TaskListItemViewModel expectedTaskStatus)
   {
      const string sectionKey = "Proposed decision and conversion dates";
      const string taskKey = "confirm-project-dates";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.ProjectDatesStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_public_sector_equality_duty_status_to_task_status(SignificantChangeTaskStatus TaskStatus, TaskListItemViewModel expectedTaskStatus)
   {
      const string sectionKey = "public-sector-equality-duty";
      const string taskKey = "public-sector-equality-duty";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.ProjectDatesStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Should().Be(expectedTaskStatus);
   }

   private static SignificantChangeProjectViewBaseModel BuildProject(Action<SignificantChangeProjectViewBaseModel> configure = null)
   {
      var project = new SignificantChangeProjectViewBaseModel
      {
         Id = 1,
         Urn = 10000001,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Test trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         Status = "Pre decision",
         StatusColour = "yellow"
      };

      configure?.Invoke(project);

      return project;
   }
}
