using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public class BasicTests : BaseIntegrationTests
	{
		public BasicTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Theory]
		[InlineData("/project-list")]
		public async Task Should_be_success_result_on_get(string url)
		{
			AddGetProjects();

			await OpenUrlAsync(url);

			Document.StatusCode.Should().Be(HttpStatusCode.OK);
			Document.ContentType.Should().Be("text/html");
			Document.CharacterSet.Should().Be("utf-8");
		}
	}
}
