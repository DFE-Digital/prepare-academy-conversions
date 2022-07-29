using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.Decision
{
	public class SummaryIntegrationTests : BaseIntegrationTests
	{
		public SummaryIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}


		//[Fact]
		//public async Task Should_display_selected_schoolname()
		//{
		//	var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

		//	await OpenUrlAsync($"/task-list/{project.Id}/decision/summary");

		//	var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

		//	selectedSchool.Should().Be(project.SchoolName);
		//}
	}
}
