using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.ProjectList;

public class ProjectListIntegrationTests : BaseIntegrationTests
{
   public ProjectListIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact]
   public async Task Should_display_list_of_projects_and_navigate_to_and_from_task_list()
   {
      List<AcademyConversionProject> projects = AddGetProjects(x => x.AcademyTypeAndRoute = "Converter").ToList();
      AddGetStatuses();

      await OpenAndConfirmPathAsync("/project-list");
      AcademyConversionProject firstProject = AddGetProject(p => p.Id = projects.First().Id);

      await NavigateAsync(projects.First().SchoolName);
      Document.Url.Should().BeUrl($"/task-list/{firstProject.Id}");

      await NavigateAsync("Back");
      Document.Url.Should().BeUrl("/project-list");

      for (int i = 0; i < 2; i++)
      {
         AcademyConversionProject project = projects.ElementAt(i);

         Document.QuerySelector($"#school-name-{i}")?.TextContent.Should().Contain(project.SchoolName);
         Document.QuerySelector($"#urn-{i}")?.TextContent.Should().Contain(project.Urn.ToString());
         Document.QuerySelector($"#application-to-join-trust-{i}")?.TextContent.Should().Contain(project.NameOfTrust);
         Document.QuerySelector($"#local-authority-{i}")?.TextContent.Should().Contain(project.LocalAuthority);
         Document.QuerySelector($"#Advisory-Board-date-{i}")?.TextContent.Should().Contain(project.HeadTeacherBoardDate.ToDateString());
         Document.QuerySelector($"#opening-date-{i}")?.TextContent.Should().Contain(project.ProposedAcademyOpeningDate.ToDateString());
         Document.QuerySelector($"#application-received-date-{i}")?.TextContent.Should().Contain(project.CreatedOn.ToDateString());
         Document.QuerySelector($"#delivery-officer-{i}")?.TextContent.Should().Contain(project.AssignedUser.FullName);
      }

      ResetServer();
   }

   [Fact]
   public async Task Should_display_approved_after_recording_decision()
   {
      AddGetStatuses();
      IEnumerable<AcademyConversionProject> projects = AddGetProjects(p => p.ProjectStatus = "Approved");

      await OpenAndConfirmPathAsync("/project-list");

      Document.QuerySelector<IHtmlElement>($"#project-status-{projects.First().Id}")?.Text().Should()
         .Be("APPROVED");
   }

   [Fact]
   public async Task Should_display_pre_advisory_board_by_default()
   {
      AddGetStatuses();
      List<AcademyConversionProject> projects = AddGetProjects().ToList();

      await OpenAndConfirmPathAsync("/project-list");

      Document.QuerySelector<IHtmlElement>($"#project-status-{projects.First().Id}")?.Text().Should()
         .Be("PRE ADVISORY BOARD");
   }

   [Fact]
   public async Task Should_display_application_received_date_when_no_htb_date_set()
   {
      AddGetStatuses();
      IEnumerable<AcademyConversionProject> projects = AddGetProjects(p => p.HeadTeacherBoardDate = null);

      await OpenAndConfirmPathAsync("/project-list");

      AcademyConversionProject project = projects.First();
      Document.QuerySelector("#application-to-join-trust-0")?.TextContent.Should().Contain(project.NameOfTrust);
      Document.QuerySelector("#local-authority-0")?.TextContent.Should().Contain(project.LocalAuthority);
      Document.QuerySelector("#application-received-date-0")?.TextContent.Should().Contain(project.CreatedOn.ToDateString());

      ResetServer();
   }
}
