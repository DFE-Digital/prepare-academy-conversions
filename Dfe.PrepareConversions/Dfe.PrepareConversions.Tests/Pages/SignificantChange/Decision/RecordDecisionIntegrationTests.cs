using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Features;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class RecordDecisionIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   [Fact]
   public async Task Should_offer_only_the_four_significant_change_decisions()
   {
      AddProject();

      await OpenAndConfirmPathAsync(PathTo("record-decision"));

      Document.QuerySelectorAll("#SignificantChangeDecision input[type=radio]")
         .Select(r => r.Id)
         .Should().BeEquivalentTo("approved-radio", "declined-radio", "withdrawn-radio");
   }

   [Fact]
   public async Task Should_not_offer_dao_revoked()
   {
      AddProject();

      await OpenAndConfirmPathAsync(PathTo("record-decision"));

      Document.QuerySelector("#daorevoked-radio").Should().BeNull();
   }

   [Theory]
   [InlineData(SignificantChangeDecisions.Approved, "any-conditions")]
   [InlineData(SignificantChangeDecisions.Declined, "declined-reason")]
   [InlineData(SignificantChangeDecisions.Withdrawn, "why-withdrawn")]
   public async Task Should_branch_to_the_correct_reason_page(SignificantChangeDecisions decision, string expectedStep)
   {
      AddProject();
      await Wizard.StartFor(ProjectId);

      await Wizard.SetDecisionToAndContinue(decision);

      Document.Url.Should().Be(BuildRequestAddress(PathTo(expectedStep)));
   }

   [Fact]
   public async Task Should_show_an_error_when_no_decision_is_selected()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);

      await Wizard.ClickSubmitButton();

      Document.Url.Should().Be(BuildRequestAddress(PathTo("record-decision")));
      Document.QuerySelector("[data-cy='error-summary']")?.TextContent.Should().Contain("Select a decision");
      Document.QuerySelector("[data-cy='error-summary'] a")?.GetAttribute("href")
         .Should().Be("#SignificantChangeDecision");
   }

   [Theory]
   [InlineData(SignificantChangeTaskStatus.NotStarted)]
   [InlineData(SignificantChangeTaskStatus.InProgress)]
   public async Task Should_link_each_incomplete_task_error_to_its_task_page(SignificantChangeTaskStatus taskStatus)
   {
      AddProject(taskStatus: taskStatus);
      await OpenAndConfirmPathAsync($"/significant-change/task-list/{ProjectId}");
      var taskLinks = Document.QuerySelectorAll(".app-task-list__task-name a")
         .Select(link => new { Title = link.TextContent.Trim(), Url = link.GetAttribute("href") })
         .ToList();
      taskLinks.Should().HaveCount(13);

      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("record-decision")));
      var errors = Document.QuerySelectorAll("[data-cy='error-summary'] a");
      errors.Should().HaveCount(taskLinks.Count);
      foreach (var task in taskLinks)
      {
         errors.Single(error => error.TextContent.Contains($"'{task.Title}'"))
            .GetAttribute("href").Should().Be($"{task.Url}?return=%2FSignificantChange%2FDecision%2FRecordDecision");
      }

      await OpenAndConfirmPathAsync(taskLinks[0].Url);
   }

   [Fact]
   public async Task Should_return_to_record_decision_after_completing_a_task_from_an_error_link()
   {
      AddProject(taskStatus: SignificantChangeTaskStatus.NotStarted);
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeProjectDates, ProjectId),
         new SetSignificantChangeProjectDatesCommand(new DateTime(2026, 11, 21), new DateTime(2026, 12, 22)),
         new object());
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);

      var taskLink = Document.QuerySelectorAll("[data-cy='error-summary'] a")
         .Single(link => link.TextContent.Contains("'Confirm project dates'"));
      await OpenAndConfirmPathAsync(taskLink.GetAttribute("href"));

      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-day").Value = "21";
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.QuerySelector("[data-cy='error-summary']").TextContent
         .Should().Contain("must include a month and year");
      new Uri(Document.Url).AbsolutePath.Should()
         .Be($"/significant-change/task-list/{ProjectId}/confirm-project-dates");
      new Uri(Document.Url).Query.Should().Contain("return=");

      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-month").Value = "11";
      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-year").Value = "2026";
      Document.QuerySelector<IHtmlInputElement>("#proposed-change-date-day").Value = "22";
      Document.QuerySelector<IHtmlInputElement>("#proposed-change-date-month").Value = "12";
      Document.QuerySelector<IHtmlInputElement>("#proposed-change-date-year").Value = "2026";
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().Be(BuildRequestAddress(PathTo("record-decision")));
      Document.QuerySelector("h1").TextContent.Trim().Should().Be("Record the decision");
   }

   [Fact]
   public async Task Should_link_missing_project_owner_error_to_project_assignment()
   {
      AddProject(assigned: false);
      await Wizard.StartFor(ProjectId);

      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("record-decision")));
      var error = Document.QuerySelectorAll("[data-cy='error-summary'] a").Single();
      error.TextContent.Should().Contain("The project must be assigned to a user");
      error.GetAttribute("href").Should().Be($"/significant-change/project-assignment/{ProjectId}?returnUrl=%252FSignificantChange%252FDecision%252FRecordDecision");
   }

   [Fact]
   public async Task Should_preselect_the_decision_already_held_in_session()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Declined);

      await OpenAndConfirmPathAsync(PathTo("record-decision"));

      Document.QuerySelector<IHtmlInputElement>("#declined-radio").IsChecked.Should().BeTrue();
   }

   [Fact]
   public async Task Should_link_back_to_the_project_details_tab()
   {
      AddProject();

      await OpenAndConfirmPathAsync(PathTo("record-decision"));

      Document.QuerySelector("[data-cy='select-backlink']")?.GetAttribute("href")
         .Should().Contain($"/significant-change/task-list/{ProjectId}");
   }
}