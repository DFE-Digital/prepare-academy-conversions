using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.NewProject;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;

namespace Dfe.PrepareConversions.Data.Services;

public class AcademyConversionProjectRepository : IAcademyConversionProjectRepository
{
   private readonly IReadOnlyDictionary<string, string> _aliasedStatuses = new Dictionary<string, string> { { "converter pre-ao (c)", "Pre advisory board" } };
   private readonly IApiClient _apiClient;
   private readonly IDfeHttpClientFactory _httpClientFactory;
   private readonly IHttpClientService _httpClientService;
   private readonly IReadOnlyDictionary<string, string> _invertedAliasedStatuses;

   public AcademyConversionProjectRepository(IApiClient apiClient,
                                             IDfeHttpClientFactory httpClientFactory = null,
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

   public async Task DeleteProjectAsync(int id)
   {
      var result = await _apiClient.DeleteConversionProject(id);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id)
   {
      HttpResponseMessage response = await _apiClient.GetProjectByIdAsync(id);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<AcademyConversionProject>(response.StatusCode, null);
      }

      AcademyConversionProject project = await ReadFromJsonAndThrowIfNull<AcademyConversionProject>(response.Content);
      return new ApiResponse<AcademyConversionProject>(response.StatusCode, project);
   }
   public async Task<ApiResponse<FormAMatProject>> GetFormAMatProjectById(int id)
   {
      HttpResponseMessage response = await _apiClient.GetFormAMatProjectById(id);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<FormAMatProject>(response.StatusCode, null);
      }

      FormAMatProject project = await ReadFromJsonAndThrowIfNull<FormAMatProject>(response.Content);
      return new ApiResponse<FormAMatProject>(response.StatusCode, project);
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

   public async Task<ApiResponse<AcademyConversionProject>> CreateProject(CreateNewProject newProject)
   {
      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();
      

      ApiResponse<AcademyConversionProject> result = await _httpClientService.Post<CreateNewProject, AcademyConversionProject>(
         httpClient,
         @"legacy/project/new-conversion-project",
         newProject);

      if (result.Success is false)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }

      return new ApiResponse<AcademyConversionProject>(result.StatusCode, result.Body);

   }
   public async Task CreateFormAMatProject(CreateNewFormAMatProject newProject)
   {
      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();

      ApiResponse<string> result = await _httpClientService.Post<CreateNewFormAMatProject, string>(
         httpClient,
         @"conversion-project/FormAMatProject",
         newProject);

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task<ApiResponse<IEnumerable<AcademyConversionProject>>> GetProjectsForGroup(string id)
   {
      HttpResponseMessage response = await _apiClient.GetProjectsForGroup(id);
      
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<IEnumerable<AcademyConversionProject>>(response.StatusCode, null);
      }

      IEnumerable<AcademyConversionProject> projects = await ReadFromJsonAndThrowIfNull<IEnumerable<AcademyConversionProject>>(response.Content);
      return new ApiResponse<IEnumerable<AcademyConversionProject>>(response.StatusCode, projects);
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
   public async Task<ApiResponse<SchoolImprovementPlan>> AddSchoolImprovementPlan(int id, AddSchoolImprovementPlan addSchoolImprovementPlan)
   {
      HttpResponseMessage response = await _apiClient.AddSchoolImprovementPlan(id, addSchoolImprovementPlan);

      return response.IsSuccessStatusCode
         ? new ApiResponse<SchoolImprovementPlan>(response.StatusCode, addSchoolImprovementPlan.ToSchoolImprovementPlan())
         : new ApiResponse<SchoolImprovementPlan>(response.StatusCode, null);
   }
   public async Task<ApiResponse<FileStreamResult>> DownloadProjectExport(
      int page,
      int count,
      string titleFilter = "",
      IEnumerable<string> statusFilters = default,
      IEnumerable<string> deliveryOfficerFilter = default,
      IEnumerable<string> regionsFilter = default,
      IEnumerable<string> localAuthoritiesFilter = default,
      IEnumerable<string> advisoryBoardDatesFilter = default)
   {
      AcademyConversionSearchModelV2 searchModel = new() { TitleFilter = titleFilter, Page = page, Count = count };

      ProcessFiltersV2(statusFilters, deliveryOfficerFilter, searchModel, regionsFilter, localAuthoritiesFilter, advisoryBoardDatesFilter);

      HttpResponseMessage response = await _apiClient.DownloadProjectExport(searchModel);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<FileStreamResult>(response.StatusCode, null);
      }

      var stream = await response.Content.ReadAsStreamAsync();
      FileStreamResult fileStreamResult = new(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

      return new ApiResponse<FileStreamResult>(response.StatusCode, fileStreamResult);
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

   private void ProcessFiltersV2(IEnumerable<string> statusFilters,
                                  IEnumerable<string> deliveryOfficerFilter,
                                  AcademyConversionSearchModelV2 searchModel,
                                  IEnumerable<string> regionsFilter = default,
                                  IEnumerable<string> localAuthoritiesFilter = default,
                                  IEnumerable<string> advisoryBoardDateFilter = default
                                  )
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
      if (localAuthoritiesFilter != default)
      {
         searchModel.LocalAuthoritiesQueryString = localAuthoritiesFilter.Select(x => x.ToLower()).ToList();
      }
      if (advisoryBoardDateFilter != default)
      {
         searchModel.AdvisoryBoardDatesQueryString = advisoryBoardDateFilter.Select(x => x.ToLower()).ToList();
      }
   }

   private async Task<T> ReadFromJsonAndThrowIfNull<T>(HttpContent content)
   {
      T responseObj = await content.ReadFromJsonAsync<T>();
      if (responseObj == null)
      {
         throw new ApiResponseException("The response body after deserialization resulted in [null]");
      }
      return responseObj;
   }

   public async Task SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl)
   {
      HttpResponseMessage result = await _apiClient.SetProjectExternalApplicationForm(id, externalApplicationFormSaved, externalApplicationFormUrl);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task SetPerformanceData(int id, SetPerformanceDataModel setPerformanceDataModel)
   {
      HttpResponseMessage result = await _apiClient.SetPerformanceData(id, setPerformanceDataModel);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }
   public async Task SetSchoolOverview(int id, SetSchoolOverviewModel updatedSchoolOverview)
   {
      HttpResponseMessage result = await _apiClient.SetSchoolOverview(id, updatedSchoolOverview);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }
   public async Task SetAssignedUser(int id, SetAssignedUserModel updatedAssignedUser)
   {
      HttpResponseMessage result = await _apiClient.SetAssignedUser(id, updatedAssignedUser);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }
   public async Task SetFormAMatAssignedUser(int id, SetAssignedUserModel updatedAssignedUser)
   {
      HttpResponseMessage result = await _apiClient.SetFormAMatAssignedUser(id, updatedAssignedUser);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjectsV2(int page, int count, string titleFilter = "", IEnumerable<string> statusFilters = null, IEnumerable<string> deliveryOfficerFilter = null, IEnumerable<string> regionsFilter = null, IEnumerable<string> localAuthoritiesFilter = null, IEnumerable<string> advisoryBoardDatesFilter = null)
   {
      AcademyConversionSearchModelV2 searchModel = new() { TitleFilter = titleFilter, Page = page, Count = count };

      ProcessFiltersV2(statusFilters, deliveryOfficerFilter, searchModel, regionsFilter, localAuthoritiesFilter, advisoryBoardDatesFilter);

      HttpResponseMessage response = await _apiClient.GetAllProjectsV2Async(searchModel);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>(response.StatusCode,
            new ApiV2Wrapper<IEnumerable<AcademyConversionProject>> { Data = Enumerable.Empty<AcademyConversionProject>() });
      }

      ApiV2Wrapper<IEnumerable<AcademyConversionProject>> outerResponse = await response.Content.ReadFromJsonAsync<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>();

      return new ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>(response.StatusCode, outerResponse);
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<FormAMatProject>>>> GetFormAMatProjects(int page, int count, string titleFilter = "", IEnumerable<string> statusFilters = null, IEnumerable<string> deliveryOfficerFilter = null, IEnumerable<string> regionsFilter = null, IEnumerable<string> localAuthoritiesFilter = null, IEnumerable<string> advisoryBoardDatesFilter = null)
   {
      AcademyConversionSearchModelV2 searchModel = new() { TitleFilter = titleFilter, Page = page, Count = count };

      ProcessFiltersV2(statusFilters, deliveryOfficerFilter, searchModel, regionsFilter, localAuthoritiesFilter, advisoryBoardDatesFilter);

      HttpResponseMessage response = await _apiClient.GetFormAMatProjectsAsync(searchModel);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<ApiV2Wrapper<IEnumerable<FormAMatProject>>>(response.StatusCode,
            new ApiV2Wrapper<IEnumerable<FormAMatProject>> { Data = Enumerable.Empty<FormAMatProject>() });
      }

      ApiV2Wrapper<IEnumerable<FormAMatProject>> outerResponse = await response.Content.ReadFromJsonAsync<ApiV2Wrapper<IEnumerable<FormAMatProject>>>();

      return new ApiResponse<ApiV2Wrapper<IEnumerable<FormAMatProject>>>(response.StatusCode, outerResponse);
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<ProjectGroup>>>> GetProjectGroups(int page, int count, string titleFilter = "", IEnumerable<string> statusFilters = null, IEnumerable<string> deliveryOfficerFilter = null, IEnumerable<string> regionsFilter = null, IEnumerable<string> localAuthoritiesFilter = null, IEnumerable<string> advisoryBoardDatesFilter = null)
   {
      AcademyConversionSearchModelV2 searchModel = new() { TitleFilter = titleFilter, Page = page, Count = count };

      ProcessFiltersV2(statusFilters, deliveryOfficerFilter, searchModel, regionsFilter, localAuthoritiesFilter, advisoryBoardDatesFilter);

      HttpResponseMessage response = await _apiClient.GetProjectGroupsAsync(searchModel);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<ApiV2Wrapper<IEnumerable<ProjectGroup>>>(response.StatusCode,
            new ApiV2Wrapper<IEnumerable<ProjectGroup>> { Data = Enumerable.Empty<ProjectGroup>() });
      }

      ApiV2Wrapper<IEnumerable<ProjectGroup>> outerResponse = await response.Content.ReadFromJsonAsync<ApiV2Wrapper<IEnumerable<ProjectGroup>>>();

      return new ApiResponse<ApiV2Wrapper<IEnumerable<ProjectGroup>>>(response.StatusCode, outerResponse);
   }

   public async Task SetIncomingTrust(int id, SetIncomingTrustDataModel setIncomingTrustDataModel)
   {
      HttpResponseMessage result = await _apiClient.SetIncomingTrust(id, setIncomingTrustDataModel);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task<ApiResponse<IEnumerable<FormAMatProject>>> SearchFormAMatProjects(string searchTerm)
   {
      HttpResponseMessage response = await _apiClient.SearchFormAMatProjects(searchTerm);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<IEnumerable<FormAMatProject>>(response.StatusCode, Enumerable.Empty<FormAMatProject>());
      }

      IEnumerable<FormAMatProject> outerResponse = await response.Content.ReadFromJsonAsync<IEnumerable<FormAMatProject>>();

      return new ApiResponse<IEnumerable<FormAMatProject>>(response.StatusCode, outerResponse);
   }

   public async Task SetFormAMatProjectReference(int id, SetFormAMatProjectReference setFormAMatProjectReference)
   {
      HttpResponseMessage result = await _apiClient.SetFormAMatProjectReference(id, setFormAMatProjectReference);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task<ApiResponse<IEnumerable<SchoolImprovementPlan>>> GetSchoolImprovementPlansForProject(int id)
   {
      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();

      return await _httpClientService.Get<IEnumerable<SchoolImprovementPlan>>(httpClient, string.Format(PathFor.GetSchoolImprovementPlans, id));
   }

   public async Task UpdateSchoolImprovementPlan(int id, UpdateSchoolImprovementPlan updateSchoolImprovementPlan)
   {
      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();

      ApiResponse<SchoolImprovementPlan> result = await _httpClientService
   .Put<UpdateSchoolImprovementPlan, SchoolImprovementPlan>(httpClient, string.Format(PathFor.GetSchoolImprovementPlans, id, updateSchoolImprovementPlan.Id), updateSchoolImprovementPlan);

      if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task SetProjectDates(int id, SetProjectDatesModel updatedProjectDates)
   {
      HttpResponseMessage result = await _apiClient.SetProjectDates(id, updatedProjectDates);
      if (result.IsSuccessStatusCode is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task<ApiResponse<IEnumerable<OpeningDateHistoryDto>>> GetOpeningDateHistoryForConversionProject(int id)
   {
      HttpResponseMessage response = await _apiClient.GetOpeningDateHistoryForConversionProject(id);
      if (!response.IsSuccessStatusCode)
      {
         return new ApiResponse<IEnumerable<OpeningDateHistoryDto>>(response.StatusCode, null);
      }

      IEnumerable<OpeningDateHistoryDto> history = await ReadFromJsonAndThrowIfNull<IEnumerable<OpeningDateHistoryDto>>(response.Content);
      return new ApiResponse<IEnumerable<OpeningDateHistoryDto>>(response.StatusCode, history);
   }

}
