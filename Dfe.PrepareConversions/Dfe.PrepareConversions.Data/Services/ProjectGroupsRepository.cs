using Dfe.Prepare.Data;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class ProjectGroupsRepository(IDfeHttpClientFactory httpClientFactory = null,
   IHttpClientService httpClientService = null) : IProjectGroupsRepository
{
   public async Task<ApiResponse<ProjectGroup>> CreateNewProjectGroup(CreateProjectGroup createProjectGroup)
   {
      
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      

      ApiResponse<ProjectGroup> result = await httpClientService.Post<CreateProjectGroup, ProjectGroup>(
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

      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();


      var result = await httpClientService.Put<SetProjectGroup, string>(
         httpClient,
         @$"/project-group/{referenceNumber}/set-project-group",
         setProjectGroup);

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

   }

   public async Task DeleteProjectGroup(string referenceNumber)
   {

      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();


      var result = await httpClientService.Delete<string>(
         httpClient,
         @$"/project-group/{referenceNumber}");

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

   }

   public async Task AssignProjectGroupUser(string referenceNumber, SetAssignedUserModel user)
   {

      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();


      var result = await httpClientService.Put<SetAssignedUserModel, string>(
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
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();

      ApiResponse<ProjectGroup> result = await httpClientService.Get<ProjectGroup>(
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
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();

      ApiResponse<ProjectGroup> result = await httpClientService.Get<ProjectGroup>(
         httpClient,
         $"/project-group/{referenceNumber}");

      if (result.Success is false)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }

      return new ApiResponse<ProjectGroup>(result.StatusCode, result.Body);
   }
}
