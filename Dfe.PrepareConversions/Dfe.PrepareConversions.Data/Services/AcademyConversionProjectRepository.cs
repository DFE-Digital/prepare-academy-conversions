using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.InvoluntaryProject;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class AcademyConversionProjectRepository : IAcademyConversionProjectRepository
{
   private readonly IReadOnlyDictionary<string, string> _aliasedStatuses = new Dictionary<string, string> { { "converter pre-ao (c)", "Pre advisory board" } };
   private readonly IApiClient _apiClient;
   private readonly IHttpClientFactory _httpClientFactory;
   private readonly IHttpClientService _httpClientService;
   private readonly IReadOnlyDictionary<string, string> _invertedAliasedStatuses;

    public AcademyConversionProjectRepository(IApiClient apiClient,
       IHttpClientFactory httpClientFactory = null,
       IHttpClientService httpClientService = null)
   {
      _apiClient = apiClient;
      _httpClientFactory = httpClientFactory;
      _httpClientService = httpClientService;
      _invertedAliasedStatuses = _aliasedStatuses.ToDictionary(x => x.Value.ToLowerInvariant(), x => x.Key);
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjects(int page,
                                                                                                      int count,
                                                                                                      string titleFilter = "",
                                                                                                      IEnumerable<string> statusFilters = default,
                                                                                                      IEnumerable<string> deliveryOfficerFilter = default,
                                                                                                      IEnumerable<string> regionsFilter = default,
                                                                                                      IEnumerable<string> applicationReferences = default)
   {
      AcademyConversionSearchModel searchModel = new() { TitleFilter = titleFilter, Page = page, Count = count };

      ProcessFilters(statusFilters, deliveryOfficerFilter, searchModel, regionsFilter, applicationReferences);

      HttpResponseMessage response = await _apiClient.GetAllProjectsAsync(searchModel);
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
      HttpResponseMessage response = await _apiClient.GetProjectByIdAsync(id);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<AcademyConversionProject>(response.StatusCode, null);
      }

      AcademyConversionProject project = await response.Content.ReadFromJsonAsync<AcademyConversionProject>();
      return new ApiResponse<AcademyConversionProject>(response.StatusCode, project);
   }

   public async Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject)
   {
      ApiResponse<AcademyConversionProject> projectResponse = await GetProjectById(id);
      if (projectResponse.Success is false)
         return new ApiResponse<AcademyConversionProject>(projectResponse.StatusCode, null);

      updateProject.Urn = projectResponse.Body.Urn;

      HttpResponseMessage updateResponse = await _apiClient.UpdateProjectAsync(id, updateProject);
      if (updateResponse.IsSuccessStatusCode is false)
         return new ApiResponse<AcademyConversionProject>(updateResponse.StatusCode, null);

      AcademyConversionProject project = await updateResponse.Content.ReadFromJsonAsync<AcademyConversionProject>();
      return new ApiResponse<AcademyConversionProject>(updateResponse.StatusCode, project);
   }

   public async Task CreateInvoluntaryProject(CreateInvoluntaryProject involuntaryProject)
   {
      var httpClient = _httpClientFactory.CreateClient("AcademisationClient");

      var result = await _httpClientService.Post<CreateInvoluntaryProject, string>(
         httpClient,
         @"legacy/project/involuntary-conversion-project",
         involuntaryProject);

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
    }

   public async Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters()
   {
      HttpResponseMessage response = await _apiClient.GetFilterParametersAsync();

      if (response.IsSuccessStatusCode is false)
         return new ApiResponse<ProjectFilterParameters>(response.StatusCode, null);

      ProjectFilterParameters filterParameters = await response.Content.ReadFromJsonAsync<ProjectFilterParameters>();

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

   public async Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote addProjectNote)
   {
      HttpResponseMessage response = await _apiClient.AddProjectNote(id, addProjectNote);

      return response.IsSuccessStatusCode
         ? new ApiResponse<ProjectNote>(response.StatusCode, addProjectNote.ToProjectNote())
         : new ApiResponse<ProjectNote>(response.StatusCode, null);
   }

   private void ProcessFilters(IEnumerable<string> statusFilters,
                                     IEnumerable<string> deliveryOfficerFilter,
                                     AcademyConversionSearchModel searchModel,
                                     IEnumerable<string> regionsFilter = default,
                                     IEnumerable<string> applicationReferences = default)
   {
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
      if (regionsFilter != default)
      {
         searchModel.RegionQueryString = regionsFilter.Select(x => x.ToLower()).ToList();
      }
      if (applicationReferences != default)
      {
         searchModel.ApplicationReferences = applicationReferences;
      }
   }
}



