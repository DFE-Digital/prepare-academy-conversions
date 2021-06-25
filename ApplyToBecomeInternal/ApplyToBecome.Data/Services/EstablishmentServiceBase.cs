using ApplyToBecome.Data.Models.Establishment;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public abstract class EstablishmentServiceBase
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<SchoolPerformanceService> _logger;

		protected EstablishmentServiceBase(IHttpClientFactory httpClientFactory, ILogger<SchoolPerformanceService> logger)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
			_logger = logger;
		}

		protected async Task<EstablishmentResponse> GetEstablishmentByUrn(string urn)
		{
			var response = await _httpClient.GetAsync($"/establishment/urn/{urn}");
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogWarning("Unable to get school performance information for establishment with URN: {urn}", urn);
				return new EstablishmentResponse();
			}

			return await response.Content.ReadFromJsonAsync<EstablishmentResponse>();
		}
	}
}