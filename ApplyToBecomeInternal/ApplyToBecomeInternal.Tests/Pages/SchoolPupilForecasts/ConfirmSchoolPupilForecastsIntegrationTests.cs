using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolPupilForecasts
{
	public class ConfirmSchoolPupilForecastsIntegrationTests : BaseIntegrationTests
	{
		public ConfirmSchoolPupilForecastsIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_reference_only_and_display_school_pupil_forecasts_table()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-pupil-forecasts-status").TextContent.Should().Be("Reference only");
			Document.QuerySelector("#school-pupil-forecasts-status").ClassName.Should().Contain("grey");

			await NavigateAsync("School pupil forecasts");

			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.SchoolPupilForecastsAdditionalInformation);

			var rows = Document.QuerySelectorAll("tbody tr");
			rows[0].Children[1].TextContent.Should().Be(project.CurrentYearCapacity.ToString());
			rows[0].Children[2].TextContent.Should().Be(project.CurrentYearPupilNumbers.ToString());
			rows[0].Children[3].TextContent.Should().Be(string.Format("{0:F0}%", 100 / project.CurrentYearCapacity * project.CurrentYearPupilNumbers));
			rows[1].Children[1].TextContent.Should().Be(project.YearOneProjectedCapacity.ToString());
			rows[1].Children[2].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.ToString());
			rows[1].Children[3].TextContent.Should().Be(string.Format("{0:F0}%", 100 / project.YearOneProjectedCapacity * project.YearOneProjectedPupilNumbers));
			rows[2].Children[1].TextContent.Should().Be(project.YearTwoProjectedCapacity.ToString());
			rows[2].Children[2].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.ToString());
			rows[2].Children[3].TextContent.Should().Be(string.Format("{0:F0}%", 100 / project.YearTwoProjectedCapacity * project.YearTwoProjectedPupilNumbers));
			rows[3].Children[1].TextContent.Should().Be(project.YearThreeProjectedCapacity.ToString());
			rows[3].Children[2].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.ToString());
			rows[3].Children[3].TextContent.Should().Be(string.Format("{0:F0}%", 100 / project.YearThreeProjectedCapacity * project.YearThreeProjectedPupilNumbers));
			rows[4].Children[1].TextContent.Should().Be(project.YearFourProjectedCapacity.ToString());
			rows[4].Children[2].TextContent.Should().Be(project.YearFourProjectedPupilNumbers.ToString());
			rows[4].Children[3].TextContent.Should().Be(string.Format("{0:F0}%", 100 / project.YearFourProjectedCapacity * project.YearFourProjectedPupilNumbers));
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_confirm_school_pupil_forecasts()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("School pupil forecasts");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-pupil-forecasts");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
