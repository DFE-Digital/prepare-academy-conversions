using System;
using System.Collections.Generic;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SignificantChangeTaskListBuilderTests
{

   [Theory]
   [InlineData(1, "key-project-dates", "Key project dates", new[] { "confirm-project-dates" })]
   [InlineData(2, "admissions-variation", "Admissions variation", new string[0])]
   [InlineData(3, "consultation-details", "Consultation details", new[] { "stakeholder-consultation", "consultation-duration", "admission-variation-consultation", "stakeholder-objections", "religious-body-consultation" })]
   [InlineData(4, "academy-performance", "Academy performance", new string[0])]
   [InlineData(5, "public-sector-equality-duty", "Public Sector Equality Duty", new[] { "public-sector-equality-duty" })]
   [InlineData(6, "land-transaction-application-and-planning-permission", "Land transaction application and planning permission", new[] { "planning-permission" })]
   [InlineData(7, "financial-details", "Financial details", new[] { "funding" })]
   [InlineData(8, "high-quality-trust-framework", "High Quality Trust Framework", new string[0])]
   [InlineData(9, "recommendation-on-how-to-proceed", "Recommendation on how to proceed", new[] { "significant-change-recommendation"})]
   public void Build_includes_ordered_sections_and_tasks_when_supplied(int sectionDisplayOrder, string sectionKey, string sectionTitle, string[] taskKeys)
   {
      var expectedSectionCount = 9;

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

   public static TheoryData<SignificantChangeTaskStatus, string> StatusCases =>
    new()
    {
         { SignificantChangeTaskStatus.Completed, TaskListItemViewModel.Completed.Status },
         { SignificantChangeTaskStatus.InProgress, TaskListItemViewModel.InProgress.Status },
         { SignificantChangeTaskStatus.NotStarted, TaskListItemViewModel.NotStarted.Status },
         {default, TaskListItemViewModel.NotStarted.Status }
    };

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_stakeholder_consultation_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "consultation-details";
      const string taskKey = "stakeholder-consultation";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.StakeholderConsultationStatus = TaskStatus);
      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_admission_variation_consultation_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "consultation-details";
      const string taskKey = "admission-variation-consultation";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.AdmissionVariationStatus = TaskStatus);
      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_stakeholder_objections_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "consultation-details";
      const string taskKey = "stakeholder-objections";
      
      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.StakeholderObjectionsStatus = TaskStatus);
      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_religious_body_consultation_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "consultation-details";
      const string taskKey = "religious-body-consultation";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.ReligiousBodyConsultationStatus = TaskStatus);
      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_confirm_project_dates_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "key-project-dates";
      const string taskKey = "confirm-project-dates";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.ProjectDatesStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_public_sector_equality_duty_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "public-sector-equality-duty";
      const string taskKey = "public-sector-equality-duty";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.EqualitiesImpactAssessmentStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   
   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_consultation_duration_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "consultation-details";
      const string taskKey = "consultation-duration";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.ConsultationDurationStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_planning_permission_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "land-transaction-application-and-planning-permission";
      const string taskKey = "planning-permission";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.PlanningPermissionTaskStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_funding_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "financial-details";
      const string taskKey = "funding";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.FundingStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
   }

   [Theory]
   [MemberData(nameof(StatusCases))]
   public void Build_maps_recommendation_status_to_task_status(SignificantChangeTaskStatus TaskStatus, string expectedTaskStatus)
   {
      const string sectionKey = "recommendation-on-how-to-proceed";
      const string taskKey = "significant-change-recommendation";

      SignificantChangeProjectViewBaseModel project = BuildProject(p => p.RecommendationStatus = TaskStatus);

      SignificantChangeTaskListViewModel result = SignificantChangeTaskListBuilder.Build(project);

      var section = Assert.Single(result.Sections, s => s.Key == sectionKey);
      var task = Assert.Single(section.Tasks, t => t.Key == taskKey);

      task.Status.Status.Should().Be(expectedTaskStatus);
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
