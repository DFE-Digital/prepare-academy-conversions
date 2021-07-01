using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class ProjectNotesRepository : IProjectNotesRepository
	{
		private readonly HttpClient _httpClient;

		public ProjectNotesRepository(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
		}

		public async Task<ApiResponse<IEnumerable<ProjectNote>>> GetProjectNotesById(int id)
		{
			var response = await _httpClient.GetAsync($"project-notes/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<IEnumerable<ProjectNote>>(response.StatusCode, null);
			}

			var project = await response.Content.ReadFromJsonAsync<IEnumerable<ProjectNote>>();
			return new ApiResponse<IEnumerable<ProjectNote>>(response.StatusCode, project);
		}

		public async Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote projectNote)
		{
			var response = await _httpClient.PostAsync($"project-notes/{id}", JsonContent.Create(projectNote));
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<ProjectNote>(response.StatusCode, null);
			}

			var project = await response.Content.ReadFromJsonAsync<ProjectNote>();
			return new ApiResponse<ProjectNote>(response.StatusCode, project);
		}
	}
}
