
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.ViewModels;
using FluentAssertions;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.ViewModels;

public class TaskListItemViewModelSfsoTests
{
   private static ProjectViewModel Project(DateTime? proposedDecisionDate, DateTime? sfsoRequestedDate, bool includeAllMandatoryInformation = false) =>
      new(new AcademyConversionProject
      {
         // ProjectViewModel's ctor calls ProjectListHelper.MapProjectStatus(ProjectStatus); a non-null
         // value avoids the pre-existing NRE at ProjectListHelper.cs:109.
         ProjectStatus = "Pre advisory board",
         HeadTeacherBoardDate = proposedDecisionDate,
         SfsoCommissioningRequestedDate = sfsoRequestedDate,
         ProposedConversionDate = includeAllMandatoryInformation ? DateTime.Today.AddDays(60) : null,
         RevenueCarryForwardAtEndMarchCurrentYear = includeAllMandatoryInformation ? 1m : null,
         CapitalCarryForwardAtEndMarchCurrentYear = includeAllMandatoryInformation ? 2m : null,
         ProjectedRevenueBalanceAtEndMarchNextYear = includeAllMandatoryInformation ? 3m : null,
         CapitalCarryForwardAtEndMarchNextYear = includeAllMandatoryInformation ? 4m : null
      });

   [Fact]
   public void NotStarted_when_no_proposed_decision_date()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(null, sfsoRequestedDate: null, includeAllMandatoryInformation: true))
         .Should().Be(TaskListItemViewModel.NotStarted);
   }

   [Fact]
   public void Completed_when_requested_date_is_set_and_mandatory_information_is_present()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(30), sfsoRequestedDate: DateTime.Today.AddDays(15), includeAllMandatoryInformation: true))
         .Should().Be(TaskListItemViewModel.Completed);
   }

   [Fact]
   public void NotStarted_when_requested_date_is_missing_even_if_other_mandatory_information_is_present()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today, sfsoRequestedDate: null, includeAllMandatoryInformation: true))
         .Should().Be(TaskListItemViewModel.NotStarted);
   }

   [Fact]
   public void NotStarted_when_requested_date_is_set_but_mandatory_information_is_missing()
   {
      TaskListItemViewModel.GetRequestFinancialHealthAssessmentTaskListStatus(Project(DateTime.Today.AddDays(30), sfsoRequestedDate: DateTime.Today.AddDays(15), includeAllMandatoryInformation: false))
         .Should().Be(TaskListItemViewModel.NotStarted);
   }
}