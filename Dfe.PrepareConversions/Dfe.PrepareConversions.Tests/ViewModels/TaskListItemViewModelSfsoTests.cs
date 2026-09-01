
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.ViewModels;
using FluentAssertions;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.ViewModels;

public class TaskListItemViewModelSfsoTests
{
   private static ProjectViewModel Project(DateTime? proposedDecisionDate, bool includeAllMandatoryInformation = false) =>
      new(new AcademyConversionProject
      {
         // ProjectViewModel's ctor calls ProjectListHelper.MapProjectStatus(ProjectStatus); a non-null
         // value avoids the pre-existing NRE at ProjectListHelper.cs:109.
         ProjectStatus = "Pre advisory board",
         HeadTeacherBoardDate = proposedDecisionDate,
         ProposedConversionDate = includeAllMandatoryInformation ? DateTime.Today.AddDays(60) : null,
         RevenueCarryForwardAtEndMarchCurrentYear = includeAllMandatoryInformation ? 1m : null,
         CapitalCarryForwardAtEndMarchCurrentYear = includeAllMandatoryInformation ? 2m : null,
         ProjectedRevenueBalanceAtEndMarchNextYear = includeAllMandatoryInformation ? 3m : null,
         CapitalCarryForwardAtEndMarchNextYear = includeAllMandatoryInformation ? 4m : null
      });

   [Fact]
   public void NotStarted_when_no_proposed_decision_date()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(null, includeAllMandatoryInformation: true))
         .Should().Be(TaskListItemViewModel.NotStarted);
   }

   [Fact]
   public void Completed_when_proposed_decision_date_in_future()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(30), includeAllMandatoryInformation: true))
         .Should().Be(TaskListItemViewModel.Completed);
   }

   [Fact]
   public void Completed_when_proposed_decision_date_today()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today, includeAllMandatoryInformation: true))
         .Should().Be(TaskListItemViewModel.Completed);
   }

   [Fact]
   public void Completed_when_proposed_decision_date_in_past()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(-1), includeAllMandatoryInformation: true))
         .Should().Be(TaskListItemViewModel.Completed);
   }
}