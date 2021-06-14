using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class AcademyConversionProjectRepository
	{
		private readonly HttpClient _httpClient;

		public AcademyConversionProjectRepository(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
		}

		public async Task<ApiResponse<IEnumerable<AcademyConversionProject>>> GetAllProjects()
		{
			var response = await _httpClient.GetAsync("conversion-projects");
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<IEnumerable<AcademyConversionProject>>(response.StatusCode, Enumerable.Empty<AcademyConversionProject>());
			}

			var projects = await response.Content.ReadFromJsonAsync<IEnumerable<AcademyConversionProject>>();
			return new ApiResponse<IEnumerable<AcademyConversionProject>>(response.StatusCode, projects);
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