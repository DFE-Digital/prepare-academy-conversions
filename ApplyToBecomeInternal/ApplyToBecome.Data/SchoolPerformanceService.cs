using ApplyToBecome.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplyToBecome.Data
{
	public class SchoolPerformanceService
	{
		private readonly HttpClient _httpClient;

		public SchoolPerformanceService(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
		}

		public async Task<SchoolPerformance> GetSchoolPerformanceByUrn(string urn)
		{
			var response = await _httpClient.GetAsync($"/establishment/urn/{urn}");
			if (!response.IsSuccessStatusCode)
			{
				return new SchoolPerformance();
			}

			var content = await response.Content.ReadAsStringAsync();

			var jToken = JToken.Parse(content);
			var schoolPerformance = jToken.SelectToken("misEstablishment").ToObject<SchoolPerformance>() ?? new SchoolPerformance();
			var ofstedLastInspection = jToken.SelectToken("ofstedLastInspection").Value<string>();

			if (DateTime.TryParse(ofstedLastInspection, out var ofstedLastInspectionDate))
			{
				schoolPerformance.OfstedLastInspection = ofstedLastInspectionDate;
			}

			return schoolPerformance;
		}
	}
}