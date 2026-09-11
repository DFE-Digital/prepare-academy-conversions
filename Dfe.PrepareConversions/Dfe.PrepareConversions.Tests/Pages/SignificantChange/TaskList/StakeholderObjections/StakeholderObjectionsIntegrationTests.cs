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
   public async Task Should_display_stakeholder_objections_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 601);
      project.StakeholderObjections = new SignificantChangeStakeholderObjectionsResponse
      {
         StakeholderObjections = SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided,
         StakeholderObjectionsComment = "Objections were raised but no further information was provided"
      };

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-objections");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Stakeholder objections");
      Document.QuerySelector<IHtmlInputElement>("#stakeholder-objections-YesNoFurtherInformationProvided")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='stakeholder-objections-additional-comments']")!.Value
         .Should().Be("Objections were raised but no further information was provided");
   }

   [Fact]
   public async Task Should_save_yes_no_further_information_provided_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 602);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string comment = "Additional information about the objections was supplied";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeStakeholderObjections, project.Id),
         new SetSignificantChangeStakeholderObjectionsCommand(SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided, comment),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-objections");

      Document.QuerySelector<IHtmlInputElement>("#stakeholder-objections-YesNoFurtherInformationProvided")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='stakeholder-objections-additional-comments']")!.Value = comment;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_yes_all_objections_addressed_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 603);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeStakeholderObjections, project.Id),
         new SetSignificantChangeStakeholderObjectionsCommand(SignificantChangeStakeholderObjection.YesAllObjectionsAddressed, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-objections");

      Document.QuerySelector<IHtmlInputElement>("#stakeholder-objections-YesAllObjectionsAddressed")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_selection_is_made()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 604);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-objections");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/stakeholder-objections");
      Document.QuerySelector<IHtmlElement>("#StakeholderObjections-error")!
         .TextContent.Should().Contain("Select whether there are any stakeholder objections");
   }

   [Fact]
   public async Task Should_show_validation_error_when_yes_no_further_information_provided_without_comment()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 605);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/stakeholder-objections");

      Document.QuerySelector<IHtmlInputElement>("#stakeholder-objections-YesNoFurtherInformationProvided")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='stakeholder-objections-additional-comments']")!.Value = "";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/stakeholder-objections");
      Document.QuerySelector<IHtmlElement>("#StakeholderObjectionsComment-error")!
         .TextContent.Should().Contain("Enter the additional information provided");
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
         Status = "pre decision",
         StakeholderObjections = new SignificantChangeStakeholderObjectionsResponse()
      };
   }
}
