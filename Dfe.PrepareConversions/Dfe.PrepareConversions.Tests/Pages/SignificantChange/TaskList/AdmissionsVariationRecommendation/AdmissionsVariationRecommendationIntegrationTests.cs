using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.AdmissionsVariationRecommendation;

public class AdmissionsVariationRecommendationIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_recommendation_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 821);
      project.AdmissionsVariationRecommendation.AdmissionsVariationRecommendationAnswer = AdmissionsVariationRecommendationAnswer.Decline;
      project.AdmissionsVariationRecommendation.FurtherInformation = "Does not meet criteria";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-recommendation");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Recommendation");
      Document.Body!.TextContent.Should().Contain("Enter a recommendation for this significant change.");
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-recommendation-decline")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='admissions-variation-recommendation-further-information']")!.Value
         .Should().Be("Does not meet criteria");
   }

   [Fact]
   public async Task Should_save_recommendation_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 822);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.AdmissionsVariationRecommendation, project.Id),
         new SetAdmissionsVariationRecommendationCommand(AdmissionsVariationRecommendationAnswer.Approve, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-recommendation");

      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-recommendation-approve")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_recommendation_with_further_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 823);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string furtherInformation = "Subject to board approval";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.AdmissionsVariationRecommendation, project.Id),
         new SetAdmissionsVariationRecommendationCommand(AdmissionsVariationRecommendationAnswer.Defer, furtherInformation),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-recommendation");

      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-recommendation-defer")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='admissions-variation-recommendation-further-information']")!.Value = furtherInformation;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_display_all_recommendation_options()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 825);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-recommendation");

      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-recommendation-approve").Should().NotBeNull();
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-recommendation-defer").Should().NotBeNull();
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-recommendation-decline").Should().NotBeNull();
      Document.QuerySelector<IHtmlInputElement>("#admissions-variation-recommendation-notapplicable").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_selection_is_made()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 824);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admissions-variation-recommendation");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/admissions-variation-recommendation");
      Document.QuerySelector<IHtmlElement>("#AdmissionsVariationRecommendationAnswer-error")!
         .TextContent.Should().Contain("Select an option");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         AdmissionsVariationRecommendation = new SignificantChangeAdmissionsVariationRecommendationResponse(),
         LocalAuthorityName = "Test local authority"
      };
   }
}
