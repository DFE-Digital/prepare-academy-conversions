using System.Net.Http;
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
			Factory.AddGetProjects();

			var response = await HttpClient.GetAsync(url);

			response.EnsureSuccessStatusCode();
			Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
		}
	}
}
