
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.ViewModels;
using FluentAssertions;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.ViewModels;

public class TaskListItemViewModelSfsoTests
{
   private static ProjectViewModel Project(DateTime? proposedDecisionDate) =>
      new(new AcademyConversionProject
      {
         // ProjectViewModel's ctor calls ProjectListHelper.MapProjectStatus(ProjectStatus); a non-null
         // value avoids the pre-existing NRE at ProjectListHelper.cs:109.
         ProjectStatus = "Pre advisory board",
         HeadTeacherBoardDate = proposedDecisionDate
      });

   [Fact]
   public void NotStarted_when_no_proposed_decision_date()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(null))
         .Should().Be(TaskListItemViewModel.NotStarted);
   }

   [Fact]
   public void Completed_when_proposed_decision_date_in_future()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(30)))
         .Should().Be(TaskListItemViewModel.Completed);
   }

   [Fact]
   public void Completed_when_proposed_decision_date_today()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today))
         .Should().Be(TaskListItemViewModel.Completed);
   }

   [Fact]
   public void Completed_when_proposed_decision_date_in_past()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(-1)))
         .Should().Be(TaskListItemViewModel.Completed);
   }
}