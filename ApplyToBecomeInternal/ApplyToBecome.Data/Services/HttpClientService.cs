using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using ApplyToBecome.Data.Services.Interfaces;
using Newtonsoft.Json;

namespace ApplyToBecome.Data.Services
{
	public class HttpClientService : IHttpClientService
	{
		private readonly ILogger<HttpClientService> _logger;
		private readonly HttpClient _client;

		public HttpClientService(IHttpClientFactory httpClientFactory, ILogger<HttpClientService> logger)
		{
			_logger = logger;
			_client = httpClientFactory.CreateClient("AcademisationClient");
		}

		public async Task<ApiResponse<TResponse>> Post<TRequest, TResponse>(string path, TRequest requestBody)
			where TResponse : class
		{
			var requestPayload = JsonContent.Create(requestBody);
			var result = await _client.PostAsync(path, requestPayload);

			return await HandleResponse<TResponse>(result);
		}

		public async Task<ApiResponse<TResponse>> Put<TRequest, TResponse>(string path, TRequest requestBody)
			where TResponse : class
		{
			var requestPayload = JsonContent.Create(requestBody);
			var result = await _client.PutAsync(path, requestPayload);

			return await HandleResponse<TResponse>(result);
		}

		public async Task<ApiResponse<TResponse>> Get<TResponse>(string path)
			where TResponse : class
		{
			var result = await _client.GetAsync(path);

			return await HandleResponse<TResponse>(result);
		}

		private async Task<ApiResponse<TResponse>> HandleResponse<TResponse>(HttpResponseMessage result) where TResponse : class
		{
			if (!result.IsSuccessStatusCode) return await HandleUnsuccessfulRequest<TResponse>(result);

			var json = await result.Content.ReadAsStringAsync();

			return new ApiResponse<TResponse>(result.StatusCode, JsonConvert.DeserializeObject<TResponse>(json));
		}

		private async Task<ApiResponse<TResponse>> HandleUnsuccessfulRequest<TResponse>(HttpResponseMessage result)
			where TResponse : class
		{
			var content = "";
			if (result.Content != null) content = await result.Content.ReadAsStringAsync();

			_logger.LogError("Request to Api failed | StatusCode - {StatusCode} | Content - {content}",
				result.StatusCode, content);

			return new ApiResponse<TResponse>(result.StatusCode, null);
		}
	}
}
