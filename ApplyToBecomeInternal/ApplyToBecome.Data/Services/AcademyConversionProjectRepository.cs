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
	public class AcademyConversionProjectRepository : IAcademyConversionProjectRepository
	{
		private readonly IReadOnlyDictionary<string, string> _aliasedStatuses = new Dictionary<string, string> { { "converter pre-ao (c)", "Pre advisory board" } };

		private readonly HttpClient _httpClient;
		private readonly IReadOnlyDictionary<string, string> _invertedAliasedStatuses;

		public AcademyConversionProjectRepository(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("TramsClient");
			_invertedAliasedStatuses = _aliasedStatuses.ToDictionary(x => x.Value.ToLowerInvariant(), x => x.Key);
		}

		public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjects(int page, int count,
			string titleFilter = "",
			IEnumerable<string> statusFilters = default,
			IEnumerable<string> deliveryOfficerFilter = default)
		{
			string encodedTitleFilter = HttpUtility.UrlEncode(titleFilter);
			string deliveryOfficerQueryString = string.Empty;

			if (deliveryOfficerFilter != default)
			{
				deliveryOfficerQueryString = $@"{deliveryOfficerFilter.Aggregate(string.Empty,
					(current, officer) => $"{current}&deliveryOfficers={HttpUtility.UrlEncode(officer)}")}";
			}

			string statusFiltersString = string.Empty;
			if (statusFilters != null)
			{
				IEnumerable<string> projectedStatuses = statusFilters.SelectMany(x =>
					_invertedAliasedStatuses.ContainsKey(x.ToLowerInvariant())
						? new[] { x, _invertedAliasedStatuses[x.ToLowerInvariant()] }
						: new[] { x });

				statusFiltersString = string.Join(',', projectedStatuses);
			}

			HttpResponseMessage response =
				await _httpClient.GetAsync($"v2/conversion-projects?page={page}&count={count}&states={statusFiltersString}&title={encodedTitleFilter}{deliveryOfficerQueryString}");
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

		public async Task<ApiResponse<List<string>>> GetAvailableStatuses()
		{
			HttpResponseMessage response = await _httpClient.GetAsync("v2/conversion-projects/statuses");

			if (response.IsSuccessStatusCode is false)
				return new ApiResponse<List<string>>(response.StatusCode, null);

			List<string> loadedStatuses = await response.Content.ReadFromJsonAsync<List<string>>();

			List<string> statusList =
				loadedStatuses.Select(x => _aliasedStatuses.ContainsKey(x.ToLowerInvariant()) ? _aliasedStatuses[x.ToLowerInvariant()] : x)
					.Distinct()
					.OrderBy(x => x)
					.ToList();

			return new ApiResponse<List<string>>(HttpStatusCode.OK, statusList);
		}
	}
}
