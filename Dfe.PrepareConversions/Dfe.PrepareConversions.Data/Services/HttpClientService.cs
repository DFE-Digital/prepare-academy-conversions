using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Newtonsoft.Json;

namespace Dfe.PrepareConversions.Data.Services
{
	public class HttpClientService : IHttpClientService
	{
		private readonly ILogger<HttpClientService> _logger;

		public HttpClientService(ILogger<HttpClientService> logger)
		{
			_logger = logger;
		}

		public async Task<ApiResponse<TResponse>> Post<TRequest, TResponse>(HttpClient httpClient, string path, TRequest requestBody)
			where TResponse : class
		{
			var requestPayload = JsonContent.Create(requestBody);
			var result = await httpClient.PostAsync(path, requestPayload);

			return await HandleResponse<TResponse>(result);
		}

		public async Task<ApiResponse<TResponse>> Put<TRequest, TResponse>(HttpClient httpClient, string path, TRequest requestBody)
			where TResponse : class
		{
			var requestPayload = JsonContent.Create(requestBody);
			var result = await httpClient.PutAsync(path, requestPayload);

			return await HandleResponse<TResponse>(result);
		}

		public async Task<ApiResponse<TResponse>> Get<TResponse>(HttpClient httpClient, string path)
			where TResponse : class
		{
			var result = await httpClient.GetAsync(path);

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
