using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services
{
   public class TrustsRespository : ITrustsRespository
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpClientService _httpClientService;

		public TrustsRespository(
			IHttpClientFactory httpClientFactory,
			IHttpClientService httpClientService)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
			_httpClientService = httpClientService;
		}

		public async Task<IEnumerable<Trust>> SearchTrusts(string searchQuery)
		{
			var path = $"trusts?name={searchQuery}&urn={searchQuery}&companiesHouseNumber={searchQuery}";

			var result = await _httpClientService.Get<IEnumerable<Trust>>(_httpClient, path);

			if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

			return result.Body;
		}
	}
}
