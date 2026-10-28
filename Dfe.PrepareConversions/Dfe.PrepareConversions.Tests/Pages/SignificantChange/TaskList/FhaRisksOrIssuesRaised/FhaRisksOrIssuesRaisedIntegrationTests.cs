using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.FhaRisksOrIssuesRaised;

public class FhaRisksOrIssuesRaisedIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_existing_answers_and_evidence()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 931);
      project.FHARisksOrIssuesRaised.FhaRisksOrIssuesRaised = true;
      project.FHARisksOrIssuesRaised.AdditionalInformation = "A deficit is forecast";
      project.FHARisksOrIssuesRaised.SupportingEvidence = "Financial forecast";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/fha-risks-or-issues-raised");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim()
         .Should().Be("Financial Health Assessment (FHA) outcomes");
      Document.QuerySelector<IHtmlInputElement>("#fha-risks-or-issues-raised-yes")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='fha-risks-or-issues-raised-additional-information']")!.Value
         .Should().Be("A deficit is forecast");
      Document.QuerySelector<IHtmlInputElement>("[data-test='fha-risks-or-issues-raised-supporting-evidence']")!.Value
         .Should().Be("Financial forecast");
   }

   [Fact]
   public async Task Should_save_yes_answer_with_additional_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 932);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string additionalInformation = "A deficit is forecast";
      const string supportingEvidence = "Financial forecast";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeFhaRisksOrIssuesRaised, project.Id),
         new SetSignificantChangeFhaRisksOrIssuesRaisedCommand(true, additionalInformation, supportingEvidence),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/fha-risks-or-issues-raised");

      Document.QuerySelector<IHtmlInputElement>("#fha-risks-or-issues-raised-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='fha-risks-or-issues-raised-additional-information']")!.Value = additionalInformation;
      Document.QuerySelector<IHtmlInputElement>("[data-test='fha-risks-or-issues-raised-supporting-evidence']")!.Value = supportingEvidence;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_without_additional_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 933);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeFhaRisksOrIssuesRaised, project.Id),
         new SetSignificantChangeFhaRisksOrIssuesRaisedCommand(false, null, "No issues evidence"),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/fha-risks-or-issues-raised");

      Document.QuerySelector<IHtmlInputElement>("#fha-risks-or-issues-raised-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='fha-risks-or-issues-raised-additional-information']")!.Value = "Clear this";
      Document.QuerySelector<IHtmlInputElement>("[data-test='fha-risks-or-issues-raised-supporting-evidence']")!.Value = "No issues evidence";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_require_additional_information_when_yes_is_selected()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 934);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/fha-risks-or-issues-raised");

      Document.QuerySelector<IHtmlInputElement>("#fha-risks-or-issues-raised-yes")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/fha-risks-or-issues-raised");
      Document.QuerySelector<IHtmlElement>("#AdditionalInformation-error")!
         .TextContent.Should().Contain("Add details of the risks or issues raised");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         ApplicationId = "12345",
         ApplicationReference = "12345",
         Urn = 10000000 + id,
         SchoolName = "Financial health assessment test school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         LocalAuthorityName = "Test local authority",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         FHARisksOrIssuesRaised = new SignificantChangeFhaRisksOrIssuesRaisedResponse()
      };
   }
}
