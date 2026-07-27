using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.ProjectAssignment;

public class SignificantChangeProjectAssignmentIntegrationTests : BaseIntegrationTests
{
   public SignificantChangeProjectAssignmentIntegrationTests(IntegrationTestingWebApplicationFactory factory)
      : base(factory)
   {
   }

   [Fact]
   public async Task Should_display_significant_change_project_assignment_page()
   {
      User assignedUser = (await _factory.UserRepository.GetAllUsers()).First();
      SignificantChangeProjectResponse project = BuildProject(
         id: 201,
         schoolName: "Significant change school 201",
         assignedUser: assignedUser);

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/project-assignment/{project.Id}");

      Document.QuerySelector<IHtmlElement>("[data-id='school-name']")!.Text().Should().Be(project.SchoolName);
      Document.QuerySelector<IHtmlElement>("h1")!.Text().Should().Be("Who will be on this project?");
      Document.QuerySelector<IHtmlOptionElement>($"option[value='{project.AssignedUser.FullName}']")!.IsSelected.Should().BeTrue();
   }

   [Fact]
   public async Task Should_assign_a_significant_change_project()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 202, schoolName: "Significant change school 202", assignedUser: null);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      User assignedUser = (await _factory.UserRepository.GetAllUsers()).First();
      SetAssignedUserSignificantChangeCommand command =
         new(Guid.Parse(assignedUser.Id), assignedUser.FullName, assignedUser.EmailAddress);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeAssignedUser, project.Id),
         command,
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/project-assignment/{project.Id}");

      Document.QuerySelector<IHtmlInputElement>("#delivery-officer-input")!.Value = assignedUser.FullName;
      Document.QuerySelector<IHtmlOptionElement>($"option[value='{assignedUser.FullName}']")!.IsSelected = true;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector("#notification-message")?.TextContent.Trim().Should().Be("Project is assigned");
      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_unassign_a_significant_change_project()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 203, schoolName: "Significant change school 203", assignedUser: null);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeAssignedUser, project.Id),
         new SetAssignedUserSignificantChangeCommand(Guid.Empty, string.Empty, string.Empty),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/project-assignment/{project.Id}");

      Document.QuerySelector<IHtmlInputElement>("#UnassignDeliveryOfficer")!.Value = "true";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector("#notification-message")?.TextContent.Trim().Should().Be("Project is unassigned");
      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   private static SignificantChangeProjectResponse BuildProject(int id, string schoolName, User assignedUser)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = schoolName,
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = assignedUser,
         TypeOfSignificantChange = "Route A",
         Status = "Pre decision"
      };
   }
}
