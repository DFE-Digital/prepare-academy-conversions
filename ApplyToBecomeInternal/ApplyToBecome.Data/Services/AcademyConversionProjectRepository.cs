using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	// TODO?: Use URN rather than ID.
	// Noticed that most services use the URN in the url
	// Should probably do the same and use URN in the URN and as the parameter in TRAMS controllers and DB queries
	// Could also put an index on the URN column in the DB if not already
	// This would also mean ViewComponents wouldn't need to use this class as the URN would be available in the route data
	public class AcademyConversionProjectRepository : IAcademyConversionProjectRepository
	{
		private readonly HttpClient _httpClient;

		public AcademyConversionProjectRepository(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
		}

		public async Task<ApiResponse<IEnumerable<AcademyConversionProject>>> GetAllProjects(int page = 1, int count = 50)
		{
			var response = await _httpClient.GetAsync($"v2/conversion-projects?page={page}&count={count}");
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<IEnumerable<AcademyConversionProject>>(response.StatusCode, Enumerable.Empty<AcademyConversionProject>());
			}

			var outerResponse = await response.Content.ReadFromJsonAsync<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>();

			return new ApiResponse<IEnumerable<AcademyConversionProject>>(response.StatusCode, outerResponse.Data);
		}

		public async Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id)
		{
			var response = await _httpClient.GetAsync($"conversion-projects/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<AcademyConversionProject>(response.StatusCode, null);
			}

			var project = await response.Content.ReadFromJsonAsync<AcademyConversionProject>();
			return new ApiResponse<AcademyConversionProject>(response.StatusCode, project);
		}

		public async Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject)
		{
			var response = await _httpClient.PatchAsync($"conversion-projects/{id}", JsonContent.Create(updateProject));
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<AcademyConversionProject>(response.StatusCode, null);
			}

			var project = await response.Content.ReadFromJsonAsync<AcademyConversionProject>();
			return new ApiResponse<AcademyConversionProject>(response.StatusCode, project);
		}
	}
}