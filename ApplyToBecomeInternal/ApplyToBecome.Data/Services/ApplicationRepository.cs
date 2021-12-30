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

		public async Task<Application> GetApplicationById(string id) // assume id is the URN for now
		{
			var response = await _httpClient.GetAsync($"/v2/apply-to-become/applyingSchool/{id}");
			if(!response.IsSuccessStatusCode)
			{
				_logger.LogWarning($"Unable to get applying school form data for establishment with id: {id}");
				return new Application();
			}
			var fullApplication = await response.Content.ReadFromJsonAsync<Application>();

			// currently we don't have the application id
			int HARDCODED_id = 1;
			var response2 = await _httpClient.GetAsync($"/v2/apply-to-become/application/{HARDCODED_id}");
			if (!response2.IsSuccessStatusCode)
			{
				_logger.LogWarning($"Unable to get school application form data for establishment with id: {id}");
				return fullApplication;
			}
			
			var partApplication = await response2.Content.ReadFromJsonAsync<ApplicationResponse>();
			fullApplication.FormTrustProposedNameOfTrust = partApplication.FormTrustProposedNameOfTrust;
			fullApplication.ApplicationLeadAuthorName = partApplication.ApplicationLeadAuthorName;
			fullApplication.ChangesToTrust = partApplication?.ChangesToTrust;
			fullApplication.ChangesToLaGovernance = partApplication?.ChangesToLaGovernance;
			return fullApplication;
		}
	}
}