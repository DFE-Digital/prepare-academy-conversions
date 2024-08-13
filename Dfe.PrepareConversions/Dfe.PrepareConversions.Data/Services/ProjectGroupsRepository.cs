using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class ProjectGroupsRepository : IProjectGroupsRepository
{
   
   private readonly IApiClient _apiClient;
   private readonly IDfeHttpClientFactory _httpClientFactory;
   private readonly IHttpClientService _httpClientService;


   public ProjectGroupsRepository(IApiClient apiClient,
      IDfeHttpClientFactory httpClientFactory = null,
      IHttpClientService httpClientService = null)
   {
      _apiClient = apiClient;
      _httpClientFactory = httpClientFactory;
      _httpClientService = httpClientService;
   }

   public async Task<ApiResponse<ProjectGroup>> CreateNewProjectGroup(CreateProjectGroup createProjectGroup)
   {
      
      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();
      

      ApiResponse<ProjectGroup> result = await _httpClientService.Post<CreateProjectGroup, ProjectGroup>(
         httpClient,
         @"/project-group/create-project-group",
         createProjectGroup);

      if (result.Success is false)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }

      return new ApiResponse<ProjectGroup>(result.StatusCode, result.Body);
      
   }

   public async Task SetProjectGroup(string referenceNumber, SetProjectGroup setProjectGroup)
   {

      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();


      var result = await _httpClientService.Put<SetProjectGroup, string>(
         httpClient,
         @$"/project-group/{referenceNumber}/set-project-group",
         setProjectGroup);

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

   }

   public async Task AssignProjectGroupUser(string referenceNumber, SetAssignedUserModel user)
   {

      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();


      var result = await _httpClientService.Put<SetAssignedUserModel, string>(
         httpClient,
         @$"/project-group/{referenceNumber}/assign-project-group-user",
         user);

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

   }

   public async Task<ApiResponse<IEnumerable<ProjectGroup>>> GetAllGroups()
   {
      return null;
   }

   public async Task<ApiResponse<ProjectGroup>> GetProjectGroupById(int id)
   {
      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();

      ApiResponse<ProjectGroup> result = await _httpClientService.Get<ProjectGroup>(
         httpClient,
         $"/project-group/{id}");

      if (result.Success is false)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }

      return new ApiResponse<ProjectGroup>(result.StatusCode, result.Body);
   }

   public async Task<ApiResponse<ProjectGroup>> GetProjectGroupByReference(string referenceNumber)
   {
      HttpClient httpClient = _httpClientFactory.CreateAcademisationClient();

      ApiResponse<ProjectGroup> result = await _httpClientService.Get<ProjectGroup>(
         httpClient,
         $"/project-group/{referenceNumber}");

      if (result.Success is false)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }

      return new ApiResponse<ProjectGroup>(result.StatusCode, result.Body);
   }
}
