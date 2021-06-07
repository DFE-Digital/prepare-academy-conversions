using ApplyToBecome.Data.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
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

			var content = await response.Content.ReadAsStringAsync();

			var jToken = JToken.Parse(content);
			var schoolPerformance = jToken.SelectToken("misEstablishment").ToObject<SchoolPerformance>() ?? new SchoolPerformance();
			var ofstedLastInspection = jToken.SelectToken("ofstedLastInspection").Value<string>();

			if (DateTime.TryParse(ofstedLastInspection, out var ofstedLastInspectionDate))
				schoolPerformance.OfstedLastInspection = ofstedLastInspectionDate;

			return schoolPerformance;
		}
	}
}