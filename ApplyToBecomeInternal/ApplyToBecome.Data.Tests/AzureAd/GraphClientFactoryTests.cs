using ApplyToBecome.Data.Services.AzureAd;
using ApplyToBecome.Data.Tests.AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ApplyToBecome.Data.Tests.AzureAd
{
	public class GraphClientFactoryTests
	{
		[Theory, AutoMoqData]
		public void Create_ReturnsGraphClient(Mock<IOptions<AzureAdOptions>> options, AzureAdOptions azureAdOptions)
		{
			options.SetupGet(m => m.Value).Returns(azureAdOptions);
			var sut = new GraphClientFactory(options.Object);

			var client = sut.Create();

			Assert.Multiple(
				() => Assert.NotNull(client),
				() => Assert.Equal("https://graph.microsoft.com/V1.0", client.BaseUrl)
			);
		}
	}
}
