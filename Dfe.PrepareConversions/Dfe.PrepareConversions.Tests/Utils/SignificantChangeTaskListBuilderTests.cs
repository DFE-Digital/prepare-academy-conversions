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
   [InlineData(1, "consultation", "Consultation", new[] { "stakeholder-consultation", "stakeholder-objections" })]
   public void Build_includes_correct_sections_when_supplied(int sectionDisplayOrder, string sectionKey, string sectionTitle, string[] taskKeys)
   {
      SignificantChangeProjectViewBaseModel project = BuildProject();

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      Assert.Single(result.Sections);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      
      Assert.Equal(sectionDisplayOrder, section.DisplayOrder);
      Assert.Equal(sectionTitle, section.Title);
      
      var tasks = section.Tasks;

      Assert.Equal(taskKeys.Length, tasks.Count);
      Assert.Equal(taskKeys, tasks.Select(t => t.Key).ToArray());
   }

   public static TheoryData<SignificantChangeTaskStatus, TaskListItemViewModel, string> StatusCases =>
    new()
    {
         { SignificantChangeTaskStatus.Completed, TaskListItemViewModel.Completed, "InProgress" },
         { SignificantChangeTaskStatus.InProgress, TaskListItemViewModel.InProgress, "InProgress" },
         { SignificantChangeTaskStatus.NotStarted, TaskListItemViewModel.NotStarted, "NotStarted" },
         {default, TaskListItemViewModel.NotStarted, "Default" }
    };

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_stakeholder_consultation_status_to_task_status(SignificantChangeTaskStatus TaskStatus, TaskListItemViewModel expectedTaskStatus, string caseName)
   {
      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.StakeholderConsultationStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      const int SectionIndex = 0;
      const int TaskIndex = 0;

      // Assert.Equal(expectedTaskStatus, result.Sections[SectionIndex].Tasks[TaskIndex].Status);

      result.Sections[SectionIndex].Tasks[TaskIndex].Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_stakeholder_objections_status_to_task_status(SignificantChangeTaskStatus TaskStatus, TaskListItemViewModel expectedTaskStatus, string caseName)
   {
      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.StakeholderObjectionsStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      const int SectionIndex = 0;
      const int TaskIndex = 1;

      result.Sections[SectionIndex].Tasks[TaskIndex].Status.Should().Be(expectedTaskStatus);
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
