using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.FormAMat
{
   public class OtherSchoolsInMatTests : BaseIntegrationTests
   {
      public OtherSchoolsInMatTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
      {
      }

      [Fact]
      public async Task Should_display_list_of_projects_within_the_same_Mat()
      {
         List<AcademyConversionProject> projects = GenerateProjects();
         AddGetStatuses();
         AcademyConversionProject firstProject = AddGetProject(p => p.Id = projects.First().Id);
         await OpenAndConfirmPathAsync($"/other-schools-in-mat/{firstProject.Id}");

         for (int i = 0; i < 2; i++)
         {
            AcademyConversionProject project = projects.ElementAt(i);

            Document.QuerySelector($"#school-name-{i}")?.TextContent.Should().Contain(project.SchoolName);
            Document.QuerySelector($"#urn-{i}")?.TextContent.Should().Contain(project.Urn.ToString());
            Document.QuerySelector($"#application-to-join-trust-{i}")?.TextContent.Should().Contain(project.NameOfTrust);
            Document.QuerySelector($"#local-authority-{i}")?.TextContent.Should().Contain(project.LocalAuthority);
            Document.QuerySelector($"#Advisory-Board-date-{i}")?.TextContent.Should().Contain(project.HeadTeacherBoardDate.ToDateString());
            Document.QuerySelector($"#opening-date-{i}")?.TextContent.Should().Contain(project.OpeningDate.ToDateString());
            Document.QuerySelector($"#application-received-date-{i}")?.TextContent.Should().Contain(project.CreatedOn.ToDateString());
            Document.QuerySelector($"#delivery-officer-{i}")?.TextContent.Should().Contain(project.AssignedUser.FullName);
         }

         ResetServer();
      }

      [Fact]
      public async Task Should_display_pre_advisory_board_status_on_list_of_projects_within_the_same_Mat()
      {
         List<AcademyConversionProject> projects = GenerateProjects();
         AddGetStatuses();
         AcademyConversionProject firstProject = AddGetProject(p => p.Id = projects.First().Id);
         await OpenAndConfirmPathAsync($"/other-schools-in-mat/{firstProject.Id}");

         for (int i = 0; i < 2; i++)
         {
            AcademyConversionProject project = projects.ElementAt(i);

            Document.QuerySelector($"#project-status-{project.Id}")?.TextContent.Should().Contain("PRE ADVISORY BOARD");
         }

         ResetServer();
      }
      private List<AcademyConversionProject> GenerateProjects()
      {
         List<AcademyConversionProject> projects = AddGetProjects(project =>
         {
            project.ApplicationReceivedDate = null;
            project.Form7ReceivedDate = null;
            project.ApplicationReferenceNumber = "A2B_1";
            project.AcademyTypeAndRoute = "Form a Mat";
         }).ToList();
         return projects;
      }
   }
}
