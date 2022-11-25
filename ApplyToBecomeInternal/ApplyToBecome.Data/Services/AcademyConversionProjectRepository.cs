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
			IEnumerable<string> deliveryOfficerFilter = default,
			IEnumerable<string> regionsFilter = default)
		{
			AcademyConversionSearchModel searchModel = new() { TitleFilter = titleFilter, Page = page, Count = count };

			await ProcessFilters(statusFilters, deliveryOfficerFilter, regionsFilter, searchModel);

			HttpResponseMessage response =
				await _httpClient.PostAsync($"v2/conversion-projects", JsonContent.Create(searchModel));
			if (!response.IsSuccessStatusCode)
			{
				return new ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>(response.StatusCode,
					new ApiV2Wrapper<IEnumerable<AcademyConversionProject>> { Data = Enumerable.Empty<AcademyConversionProject>() });
			}

			ApiV2Wrapper<IEnumerable<AcademyConversionProject>> outerResponse = await response.Content.ReadFromJsonAsync<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>();

			return new ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>(response.StatusCode, outerResponse);
		}

		private async Task ProcessFilters(IEnumerable<string> statusFilters, IEnumerable<string> deliveryOfficerFilter,
			IEnumerable<string> regionsFilter, AcademyConversionSearchModel searchModel)
		{
			string regionQueryString;
			if (deliveryOfficerFilter != default)
			{
				searchModel.DeliveryOfficerQueryString = deliveryOfficerFilter;
			}

			if (statusFilters != null)
			{
				IEnumerable<string> projectedStatuses = statusFilters.SelectMany(x =>
					_invertedAliasedStatuses.ContainsKey(x.ToLowerInvariant())
						? new[] { x, _invertedAliasedStatuses[x.ToLowerInvariant()] }
						: new[] { x });

				searchModel.StatusQueryString = projectedStatuses.ToArray();
			}

			if (regionsFilter != null && regionsFilter.Any())
			{
				regionQueryString = $@"{regionsFilter.Aggregate(string.Empty,
					(current, region) => $"{current}&regions={HttpUtility.UrlEncode(region)}")}";
				HttpResponseMessage regionResponse =
					await _httpClient.GetAsync($"establishment/regions?{regionQueryString.ToLower()}");
				List<int> regionUrnResponse = await regionResponse.Content.ReadFromJsonAsync<List<int>>();
				searchModel.RegionUrnsQueryString = regionUrnResponse;
			}
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

		public async Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters()
		{
			HttpResponseMessage response = await _httpClient.GetAsync("v2/conversion-projects/parameters");

			if (response.IsSuccessStatusCode is false)
				return new ApiResponse<ProjectFilterParameters>(response.StatusCode, null);

			var filterParameters = await response.Content.ReadFromJsonAsync<ProjectFilterParameters>();

			filterParameters.Statuses =
				filterParameters.Statuses.Select(x => _aliasedStatuses.ContainsKey(x.ToLowerInvariant()) ? _aliasedStatuses[x.ToLowerInvariant()] : x)
					.Distinct()
					.OrderBy(x => x)
					.ToList();

			filterParameters.Regions = new List<string>
			{
				"East Midlands",
				"East of England",
				"London",
				"North East",
				"North West",
				"South East",
				"South West",
				"West Midlands",
				"Yorkshire and the Humber"
			};
			return new ApiResponse<ProjectFilterParameters>(response.StatusCode, filterParameters);
		}
	}
}
