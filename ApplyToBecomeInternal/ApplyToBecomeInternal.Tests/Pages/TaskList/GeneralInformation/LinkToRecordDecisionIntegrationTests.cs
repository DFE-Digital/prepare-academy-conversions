using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.GeneralInformation
{
	public class LinkToRecordDecisionIntegrationTests : BaseIntegrationTests
	{
		public LinkToRecordDecisionIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		// Test do remain commented out until the RecordDecision button is introduced.
		//[Fact]
		//public async Task Should_redirect_to_record_decision()
		//{
		//	var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

		//	await OpenUrlAsync($"/task-list/{project.Id}");

		//	await NavigateAsync("Record a decision");			

		//	Document.Url.Should().Contain($"/task-list/{project.Id}/decision/record-decision");
		//}
	}
}