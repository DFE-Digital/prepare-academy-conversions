using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation
{
	public class AcademyTypeAndRouteIntegrationTests : BaseIntegrationTests
	{
		public AcademyTypeAndRouteIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_conversion_support_grant_amount()
		{
			var project = AddGetProject();
			var request = AddPatchProjectMany(project, composer =>
				composer
					.With(r => r.ConversionSupportGrantAmount)
					.With(r => r.ConversionSupportGrantChangeReason)
               .With(r => r.Urn, project.Urn)
			);

			await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			await NavigateDataTestAsync("change-academy-type-and-route");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/route-and-grant");
			Document.QuerySelector<IHtmlInputElement>("#conversion-support-grant-amount").Value.Should().Be(project.ConversionSupportGrantAmount.Value.ToMoneyString());
			Document.QuerySelector<IHtmlTextAreaElement>("#conversion-support-grant-change-reason").TextContent.Should().Be(project.ConversionSupportGrantChangeReason);

			Document.QuerySelector<IHtmlInputElement>("#conversion-support-grant-amount").Value = request.ConversionSupportGrantAmount.Value.ToMoneyString();
			Document.QuerySelector<IHtmlTextAreaElement>("#conversion-support-grant-change-reason").Value = request.ConversionSupportGrantChangeReason;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}
	}
}