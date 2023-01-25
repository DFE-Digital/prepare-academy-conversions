using RichardSzalay.MockHttp;
using System.Net.Http;
using System;

namespace Dfe.PrepareConversions.Data.Tests.TestDoubles
{
	internal class MockHttpClientFactory : IHttpClientFactory
	{
		private readonly MockHttpMessageHandler _mockHttpMessageHandler;

		public MockHttpClientFactory(MockHttpMessageHandler mockHttpMessageHandler)
		{
			_mockHttpMessageHandler = mockHttpMessageHandler;
		}

		public HttpClient CreateClient(string name)
		{
			var client = _mockHttpMessageHandler.ToHttpClient();
			client.BaseAddress = new Uri("http://localhost/");
			return client;
		}
	}
}
