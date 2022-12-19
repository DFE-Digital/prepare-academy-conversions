using ApplyToBecome.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			IEnumerable<ProjectNote> projectNotes = (project ?? Enumerable.Empty<ProjectNote>()).ToList().OrderByDescending(x => x.Date);
			return new ApiResponse<IEnumerable<ProjectNote>>(response.StatusCode, projectNotes);
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
