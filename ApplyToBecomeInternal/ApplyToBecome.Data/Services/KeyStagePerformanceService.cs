using ApplyToBecome.Data.Models.KeyStagePerformance;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class KeyStagePerformanceService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<KeyStagePerformanceService> _logger;

		public KeyStagePerformanceService(IHttpClientFactory httpClientFactory, ILogger<KeyStagePerformanceService> logger)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
			_logger = logger;
		}

		public async Task<KeyStagePerformanceResponse> GetKeyStagePerformance(string urn)
		{
			var response = await _httpClient.GetAsync($"/educationPerformance/{urn}");
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogWarning("Unable to get key stage performance data for establishment with URN: {urn}", urn);
				return new KeyStagePerformanceResponse();
			}

			return await response.Content.ReadFromJsonAsync<KeyStagePerformanceResponse>();
		}
	}
}