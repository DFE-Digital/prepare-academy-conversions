using ApplyToBecome.Data.Models;
using System.Collections.Generic;
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

		public async Task<IEnumerable<Project>> GetAllProjects()
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<Project>>("conversion-projects");
		}

		public async Task<Project> GetProjectById(int id)
		{
			return await _httpClient.GetFromJsonAsync<Project>($"conversion-projects/{id}");
		}
	}
}