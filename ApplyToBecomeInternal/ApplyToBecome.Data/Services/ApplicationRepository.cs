using ApplyToBecome.Data.Models.Application;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class ApplicationRepository
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<ApplicationRepository> _logger;

		public ApplicationRepository(IHttpClientFactory httpClientFactory, ILogger<ApplicationRepository> logger)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
			_logger = logger;
		}

		public async Task<ApiResponse<FullApplication>> GetApplicationByReference(string id) // id is the application reference number
		{
			var response = await _httpClient.GetAsync($"/v2/apply-to-become/application/{id}");
			if(!response.IsSuccessStatusCode)
			{
				_logger.LogWarning($"Unable to get school application form data for establishment with id: {id}");
				return new ApiResponse<FullApplication>(response.StatusCode, null);
			}
			var fullApplication = await response.Content.ReadFromJsonAsync<FullApplication>();

			return new ApiResponse<FullApplication>(response.StatusCode, fullApplication); 
		}
	}
}