using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolPupilForecasts
{
	public class AdditionalInformationIntegrationTests : BaseIntegrationTests
	{
		public AdditionalInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_additional_information()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());

			var request = AddPatchProject(project, r => r.SchoolPupilForecastsAdditionalInformation);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-pupil-forecasts");
			await NavigateAsync("Change", 0);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-pupil-forecasts/additional-information#additional-information");
			var textArea = Document.QuerySelector<IHtmlTextAreaElement>("#additional-information");
			textArea.TextContent.Should().Be(project.SchoolPupilForecastsAdditionalInformation);

			textArea.Value = request.SchoolPupilForecastsAdditionalInformation;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-pupil-forecasts");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());

			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-pupil-forecasts/additional-information");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_confirm_school_pupil_forecasts()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-pupil-forecasts/additional-information");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-pupil-forecasts");
		}
	}
}
