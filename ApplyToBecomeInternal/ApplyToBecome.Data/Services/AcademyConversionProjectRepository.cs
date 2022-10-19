using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

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

		public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjects(int page, int count, string statusFilters = "",
			string titleFilter = "")
		{
			string encodedTitleFilter = HttpUtility.UrlEncode(titleFilter);
			HttpResponseMessage response = await _httpClient.GetAsync($"v2/conversion-projects?page={page}&count={count}&states={statusFilters}&title={encodedTitleFilter}");
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>(response.StatusCode,
					new ApiV2Wrapper<IEnumerable<AcademyConversionProject>> { Data = Enumerable.Empty<AcademyConversionProject>() });
			}

			ApiV2Wrapper<IEnumerable<AcademyConversionProject>> outerResponse = await response.Content.ReadFromJsonAsync<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>();

			return new ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>(response.StatusCode, outerResponse);
		}

		public async Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id)
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"conversion-projects/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<AcademyConversionProject>(response.StatusCode, null);
			}

			AcademyConversionProject project = await response.Content.ReadFromJsonAsync<AcademyConversionProject>();
			return new ApiResponse<AcademyConversionProject>(response.StatusCode, project);
		}

		public async Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject)
		{
			HttpResponseMessage response = await _httpClient.PatchAsync($"conversion-projects/{id}", JsonContent.Create(updateProject));
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<AcademyConversionProject>(response.StatusCode, null);
			}

			AcademyConversionProject project = await response.Content.ReadFromJsonAsync<AcademyConversionProject>();
			return new ApiResponse<AcademyConversionProject>(response.StatusCode, project);
		}

		public Task<ApiResponse<List<string>>> GetAvailableStatuses()
		{
			return Task.FromResult(new ApiResponse<List<string>>(HttpStatusCode.OK, new List<string>
			{
				"Converter Pre-AO (C)",
				"Pre Advisory Board",
				"Active",
				"Approved",
				"Approved with Conditions",
				"Deferred",
				"Declined"
			}));
		}
	}
}
