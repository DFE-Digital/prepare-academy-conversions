using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class ProjectsService : IProjects
	{
		private readonly HttpClient _httpClient;

		public ProjectsService(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
		}

		public async Task<ApiResponse<IEnumerable<Project>>> GetAllProjects()
		{
			var response = await _httpClient.GetAsync("conversion-projects");
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<IEnumerable<Project>>(response.StatusCode, Enumerable.Empty<Project>());
			}

			var projects = await response.Content.ReadFromJsonAsync<IEnumerable<Project>>();
			return new ApiResponse<IEnumerable<Project>>(response.StatusCode, projects);
		}

		public async Task<ApiResponse<Project>> GetProjectById(int id)
		{
			var response = await _httpClient.GetAsync($"conversion-projects/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<Project>(response.StatusCode, null);
			}

			var project = await response.Content.ReadFromJsonAsync<Project>();
			return new ApiResponse<Project>(response.StatusCode, project);
		}

		public async Task<ApiResponse<Project>> UpdateProject(int id, UpdateProject updateProject)
		{
			var response = await _httpClient.PatchAsync($"conversion-projects/{id}", JsonContent.Create(updateProject));
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<Project>(response.StatusCode, null);
			}

			var project = await response.Content.ReadFromJsonAsync<Project>();
			return new ApiResponse<Project>(response.StatusCode, project);
		}
	}
}