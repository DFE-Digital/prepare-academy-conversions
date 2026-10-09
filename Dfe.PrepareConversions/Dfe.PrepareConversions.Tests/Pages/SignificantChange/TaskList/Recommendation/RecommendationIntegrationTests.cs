using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using RecommendationOptions = Dfe.PrepareConversions.Data.Models.SignificantChange.SignificantChangeRecommendation;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Recommendation;

public class RecommendationIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_recommendation_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 711);
      project.Recommendation.Recommendation = RecommendationOptions.Decline;
      project.Recommendation.RecommendationMoreInformation = "No thanks";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/significant-change-recommendation");

      Document.QuerySelector<IHtmlHeadingElement>("h1").TextContent.Trim().Should().Be("Significant change recommendation");
      Document.QuerySelector<IHtmlInputElement>("#decline-radio").IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("#MoreInformation").Value.Should().Be("No thanks");
   }

   [Theory]
   [InlineData(null, null, SignificantChangeTaskStatus.NotStarted)]
   [InlineData(null, "More information", SignificantChangeTaskStatus.InProgress)]
   [InlineData(RecommendationOptions.Approve, null, SignificantChangeTaskStatus.Completed)]
   [InlineData(RecommendationOptions.Decline, "More information", SignificantChangeTaskStatus.Completed)]
   public void Recommendation_status_should_follow_expected_rules(
      RecommendationOptions? recommendation,
      string moreInformation,
      SignificantChangeTaskStatus expectedStatus)
   {
      SignificantChangeProjectResponse project = BuildProject(701);
      project.Recommendation = new SignificantChangeRecommendationResponse
      {
         Recommendation = recommendation,
         RecommendationMoreInformation = moreInformation ?? string.Empty,
         Status = SignificantChangeTaskStatus.NotStarted
      };

      SignificantChangeProjectViewBaseModel viewModel = SignificantChangeProjectListHelper.Build(project);

      viewModel.RecommendationStatus.Should().Be(expectedStatus);
   }

   [Fact]
   public async Task Should_save_recommendation_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 712);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string additionalInformation = "Some extra detail";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeRecommendation, project.Id),
         new SetSignificantChangeRecommendationCommand(RecommendationOptions.Approve, additionalInformation),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/significant-change-recommendation");

      Document.QuerySelector<IHtmlInputElement>("#approve-radio").IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("#MoreInformation").Value = additionalInformation;
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_recommendation_is_selected()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 713);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/significant-change-recommendation");

      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/significant-change-recommendation");
      Document.QuerySelector<IHtmlElement>("#Recommendation-error")
         .TextContent.Should().Contain("Select a recommendation");
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
         Recommendation = new SignificantChangeRecommendationResponse(),
         LocalAuthorityName = "Test local authority"
      };
   }
}
