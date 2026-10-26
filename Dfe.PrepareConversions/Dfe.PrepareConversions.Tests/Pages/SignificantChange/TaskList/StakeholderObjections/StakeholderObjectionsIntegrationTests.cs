using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.StakeholderObjections;

public class StakeholderObjectionsIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_other_stakeholder_engagement_values()
   {
      SignificantChangeProjectResponse project = BuildProject(601);
      project.OtherStakeholderEngagement = new SignificantChangeOtherStakeholderEngagementResponse
      {
         TrustConsultedStakeholders = false,
         TrustConsultedStakeholdersNotConsultedReason = "Consultation planned",
         WereThereAnyObjectionsRaised = true,
         StakeholderObjectionsDetails = "Objection details",
         RelevantStakeholdersNotifiedOfAdmissionsChanges = YesNoNotApplicableAnswer.NotApplicable,
         SupportingEvidence = "https://example.test/evidence"
      };
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/other-stakeholder-engagement");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Other stakeholder engagement");
      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-stakeholders-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>("#objections-raised-yes")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>("#stakeholders-notified-NotApplicable")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='stakeholder-objections-details']")!.Value.Should().Be("Objection details");
      Document.QuerySelector<IHtmlInputElement>("[data-test='other-stakeholder-supporting-evidence']")!.Value.Should().Be("https://example.test/evidence");
   }

   [Fact]
   public async Task Should_save_all_stakeholder_answers_in_one_request()
   {
      SignificantChangeProjectResponse project = BuildProject(602);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);
      const string reason = "Additional details";
      const string evidence = "https://example.test/evidence";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeOtherStakeholderEngagement, project.Id),
         new SetSignificantChangeOtherStakeholderEngagementCommand(
            false,
            reason,
            true,
            reason,
            YesNoNotApplicableAnswer.No,
            reason,
            evidence),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/other-stakeholder-engagement");
      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-stakeholders-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='trust-consulted-stakeholders-not-consulted-reason']")!.Value = reason;
      Document.QuerySelector<IHtmlInputElement>("#objections-raised-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='stakeholder-objections-details']")!.Value = reason;
      Document.QuerySelector<IHtmlInputElement>("#stakeholders-notified-No")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='stakeholders-notified-reason']")!.Value = reason;
      Document.QuerySelector<IHtmlInputElement>("[data-test='other-stakeholder-supporting-evidence']")!.Value = evidence;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_require_conditional_details_when_triggering_answer_is_selected()
   {
      SignificantChangeProjectResponse project = BuildProject(603);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/other-stakeholder-engagement");
      Document.QuerySelector<IHtmlInputElement>("#trust-consulted-stakeholders-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#objections-raised-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#stakeholders-notified-Yes")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"/other-stakeholder-engagement");
      Document.QuerySelector<IHtmlElement>("#StakeholderObjectionsDetails-error")!.TextContent.Should().Contain("Add details");
   }

   private static SignificantChangeProjectResponse BuildProject(int id) =>
      new()
      {
         Id = id,
         ApplicationId = "1234",
         ApplicationReference = "56789",
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         LocalAuthorityName = "Test local authority",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision"
      };
}
