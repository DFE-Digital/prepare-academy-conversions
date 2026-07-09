using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.ViewModels;
using FluentAssertions;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.ViewModels;

public class TaskListItemViewModelSfsoTests
{
   private static ProjectViewModel Project(DateTime? proposedDecisionDate, DateTime? requestedDate) =>
      new(new AcademyConversionProject
      {
         HeadTeacherBoardDate = proposedDecisionDate,
         SfsoCommissioningRequestedDate = requestedDate
      });

   [Fact]
   public void NotStarted_when_no_requested_date_and_future_decision()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(30), null))
         .Should().Be(TaskListItemViewModel.NotStarted);
   }

   [Fact]
   public void InProgress_when_requested_and_decision_in_future()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(10), DateTime.Today))
         .Should().Be(TaskListItemViewModel.InProgress);
   }

   [Fact]
   public void Completed_when_decision_date_reached_today()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today, DateTime.Today))
         .Should().Be(TaskListItemViewModel.Completed);
   }

   [Fact]
   public void Completed_when_decision_date_in_past()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(-1), null))
         .Should().Be(TaskListItemViewModel.Completed);
   }
}