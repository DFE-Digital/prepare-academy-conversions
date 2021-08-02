using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.ProjectList
{
	public class ProjectListIntegrationTests : BaseIntegrationTests
	{
		public ProjectListIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_display_list_of_projects_and_navigate_to_and_from_task_list()
		{
			var projects = AddGetProjects().ToList();

			await OpenUrlAsync($"/project-list");
			var firstProject = AddGetProject(p => p.Id = projects.First().Id);

			await NavigateAsync(projects.First().SchoolName);
			Document.Url.Should().BeUrl($"/task-list/{firstProject.Id}");

			await NavigateAsync("Back to all conversion projects");
			Document.Url.Should().BeUrl($"/project-list");

			for (int i = 0; i < 2; i++)
			{
				var project = projects.ElementAt(i);
				Document.QuerySelector($"#school-name-{i}").TextContent.Should().Contain(project.SchoolName);
				Document.QuerySelector($"#urn-{i}").TextContent.Should().Contain(project.Urn.ToString());
				Document.QuerySelector($"#application-to-join-trust-{i}").TextContent.Should().Contain(project.NameOfTrust);
				Document.QuerySelector($"#local-authority-{i}").TextContent.Should().Contain(project.LocalAuthority);
				Document.QuerySelector($"#htb-date-{i}").TextContent.Should().Contain(project.HeadTeacherBoardDate.ToDateString());
				Document.QuerySelector($"#opening-date-{i}").TextContent.Should().Contain(project.ProposedAcademyOpeningDate.ToDateString());
				Document.QuerySelector($"#application-received-date-{i}").Should().BeNull();
				Document.QuerySelector($"#assigned-to-me-{i}").Should().BeNull();
			}

			ResetServer();
		}

		[Fact]
		public async Task Should_display_application_received_date_and_assigned_to_me_date_when_no_htb_date_set()
		{
			var projects = AddGetProjects(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/project-list");

			var project = projects.First();
			Document.QuerySelector("#application-to-join-trust-0").TextContent.Should().Contain(project.NameOfTrust);
			Document.QuerySelector("#local-authority-0").TextContent.Should().Contain(project.LocalAuthority);
			Document.QuerySelector("#application-received-date-0").TextContent.Should().Contain(project.ApplicationReceivedDate.ToDateString());
			Document.QuerySelector("#assigned-to-me-0").TextContent.Should().Contain(project.AssignedDate.ToDateString());

			ResetServer();
		}
	}
}
