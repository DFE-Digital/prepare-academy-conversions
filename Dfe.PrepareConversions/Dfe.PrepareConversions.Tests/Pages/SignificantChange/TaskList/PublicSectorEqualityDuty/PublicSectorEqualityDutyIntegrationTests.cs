using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.PublicSectorEqualityDuty;

public class PublicSectorEqualityDutyIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_public_sector_equality_duty_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 701);
      project.EqualitiesImpactAssessment.EqualitiesImpactAssessmentCompleted = true;
      project.EqualitiesImpactAssessment.EqualitiesImpactIdentified = EqualitiesImpact.ImpactsIdentified;
      project.EqualitiesImpactAssessment.EqualitiesImpactIdentifiedMitigation = "Additional info required";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/public-sector-equality-duty");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Public Sector Equality Duty");
      Document.QuerySelector<IHtmlInputElement>("#assessment-completed-yes")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>("#equalities-impact-impacts-identified")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='which-groups-affected']")!.Value
         .Should().Be("Additional info required");
   }

   [Fact]
   public async Task Should_save_no_material_impact_answer_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 702);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeEqualitiesImpactAssessment, project.Id),
         new SetSignificantChangeEqualitiesImpactAssessmentCommand(true, EqualitiesImpact.None, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/public-sector-equality-duty");

      Document.QuerySelector<IHtmlInputElement>("#assessment-completed-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#equalities-impact-none")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_impacts_identified_answer_with_groups_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 703);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string groups = "Pupils with SEND - additional transitional support planned";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeEqualitiesImpactAssessment, project.Id),
         new SetSignificantChangeEqualitiesImpactAssessmentCommand(true, EqualitiesImpact.ImpactsIdentified, groups),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/public-sector-equality-duty");

      Document.QuerySelector<IHtmlInputElement>("#assessment-completed-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#equalities-impact-impacts-identified")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='which-groups-affected']")!.Value = groups;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
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
         EqualitiesImpactAssessment = new SignificantChangeEqualitiesImpactAssessmentResponse()
      };
   }
}
