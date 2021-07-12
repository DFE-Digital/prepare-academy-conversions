using ApplyToBecomeInternal.Extensions;
using AutoFixture;
using static ApplyToBecomeInternal.Extensions.IntegerExtensions;
using FluentAssertions;
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
			var project = AddGetProject(p =>
			{
				p.YearOneProjectedCapacity = _fixture.Create<int>();
				p.YearOneProjectedPupilNumbers = _fixture.Create<int>();
				p.YearTwoProjectedCapacity = _fixture.Create<int>();
				p.YearTwoProjectedPupilNumbers = _fixture.Create<int>();
				p.YearThreeProjectedCapacity = _fixture.Create<int>();
				p.YearThreeProjectedPupilNumbers = _fixture.Create<int>();
			});
			var establishment = AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-pupil-forecasts-status").TextContent.Trim().Should().Be("Reference only");
			Document.QuerySelector("#school-pupil-forecasts-status").ClassName.Should().Contain("grey");

			await NavigateAsync("School pupil forecasts");

			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.SchoolPupilForecastsAdditionalInformation);

			var rows = Document.QuerySelectorAll("tbody tr");
			rows[0].Children[1].TextContent.Should().Be(establishment.SchoolCapacity);
			rows[0].Children[2].TextContent.Should().Be(establishment.Census.NumberOfPupils);
			rows[0].Children[3].TextContent.Should().Be(ToInt(establishment.Census?.NumberOfPupils).AsPercentageOf(ToInt(establishment.SchoolCapacity)));
			rows[1].Children[1].TextContent.Should().Be(project.YearOneProjectedCapacity.ToString());
			rows[1].Children[2].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.ToString());
			rows[1].Children[3].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.AsPercentageOf(project.YearOneProjectedCapacity));
			rows[2].Children[1].TextContent.Should().Be(project.YearTwoProjectedCapacity.ToString());
			rows[2].Children[2].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.ToString());
			rows[2].Children[3].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.AsPercentageOf(project.YearTwoProjectedCapacity));
			rows[3].Children[1].TextContent.Should().Be(project.YearThreeProjectedCapacity.ToString());
			rows[3].Children[2].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.ToString());
			rows[3].Children[3].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.AsPercentageOf(project.YearThreeProjectedCapacity));

			await NavigateAsync("Confirm and continue");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_display_school_pupil_forecasts_table_when_values_are_null()
		{
			var project = AddGetProject(p =>
			{
				p.YearOneProjectedCapacity = null;
				p.YearOneProjectedPupilNumbers = null;
				p.YearTwoProjectedCapacity = null;
				p.YearTwoProjectedPupilNumbers = null;
				p.YearThreeProjectedCapacity = null;
				p.YearThreeProjectedPupilNumbers = null;
			});
			AddGetEstablishmentResponse(project.Urn.ToString(), true);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-pupil-forecasts-status").TextContent.Trim().Should().Be("Reference only");
			Document.QuerySelector("#school-pupil-forecasts-status").ClassName.Should().Contain("grey");

			await NavigateAsync("School pupil forecasts");

			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.SchoolPupilForecastsAdditionalInformation);

			var rows = Document.QuerySelectorAll("tbody tr");
			rows[0].Children[1].TextContent.Should().Be("");
			rows[0].Children[2].TextContent.Should().Be("");
			rows[0].Children[3].TextContent.Should().Be("");
			rows[1].Children[1].TextContent.Should().Be("");
			rows[1].Children[2].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.ToString());
			rows[1].Children[3].TextContent.Should().Be("");
			rows[2].Children[1].TextContent.Should().Be(project.YearTwoProjectedCapacity.ToString());
			rows[2].Children[2].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.ToString());
			rows[2].Children[3].TextContent.Should().Be("");
			rows[3].Children[1].TextContent.Should().Be(project.YearThreeProjectedCapacity.ToString());
			rows[3].Children[2].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.ToString());
			rows[3].Children[3].TextContent.Should().Be("");

			await NavigateAsync("Confirm and continue");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_confirm_school_pupil_forecasts()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("School pupil forecasts");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-pupil-forecasts");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
