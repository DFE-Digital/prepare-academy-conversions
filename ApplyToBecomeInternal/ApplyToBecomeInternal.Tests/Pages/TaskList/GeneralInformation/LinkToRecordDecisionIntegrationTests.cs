using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using static ApplyToBecomeInternal.Tests.Helpers.WaitHelper;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.GeneralInformation
{
	public class LinkToRecordDecisionIntegrationTests : BaseIntegrationTests
	{
		public LinkToRecordDecisionIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task Should_redirect_to_record_decision()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector<IHtmlAnchorElement>("#record-decision-link").DoClick();

			WaitUntil(() => Document.Url.Contains("decision"));

			Document.Url.Should().Contain($"/task-list/{project.Id}/decision/record-decision");
		}
	}
}