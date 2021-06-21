using ApplyToBecome.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class SchoolPerformanceService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<SchoolPerformanceService> _logger;

		public SchoolPerformanceService(IHttpClientFactory httpClientFactory, ILogger<SchoolPerformanceService> logger)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
			_logger = logger;
		}

		public async Task<SchoolPerformance> GetSchoolPerformanceByUrn(string urn)
		{
			var response = await _httpClient.GetAsync($"/establishment/urn/{urn}");
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogWarning("Unable to get school performance information for establishment with URN: {urn}", urn);
				return new SchoolPerformance();
			}

			var content = await response.Content.ReadFromJsonAsync<EstablishmentData>();
			var schoolPerformance = content.misEstablishment;
			//schoolPerformance.OfstedLastInspection = content.ofstedLastInspection;

			return schoolPerformance;
		}

		private class EstablishmentData
		{
			public SchoolPerformance misEstablishment { get; set; }

			//public DateTime ofstedLastInspection { get; set; }
		}
	}
}