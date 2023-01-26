using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.Public
{
	public class AccessibilityStatementTests : BaseIntegrationTests
	{
		public AccessibilityStatementTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task Should_navigate_to_the_accessibility_statement_from_the_link()
		{
			var project = AddGetProject();
			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateDataTestAsync("accessibility-statement");

			Document.Url.Should().Contain("/public/accessibility");
		}
	}
}