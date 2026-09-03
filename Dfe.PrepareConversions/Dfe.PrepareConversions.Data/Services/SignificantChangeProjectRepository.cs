#nullable enable

using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class SignificantChangeProjectRepository(
   IDfeHttpClientFactory httpClientFactory,
   IHttpClientService httpClientService) : ISignificantChangeProjectRepository
{
   public async Task<ApiResponse<SignificantChangeProjectResponse>> CreateProject(CreateSignificantProjectCommand command)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();

      ApiResponse<SignificantChangeProjectResponse> result =
         await httpClientService.Post<CreateSignificantProjectCommand, SignificantChangeProjectResponse>(
            httpClient,
            PathFor.CreateSignificantChangeProject,
            command);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }

      return new ApiResponse<SignificantChangeProjectResponse>(result.StatusCode, result.Body);
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>> GetAllProjects(
      int page,
      int count,
      string? keyword = null,
      string[]? statuses = null,
      string[]? assignees = null,
      byte[]? tiers = null,
      string[]? routes = null)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();

      // Empty collections must normalise to null, not to empty lists. GetSignificantProjectsQuery is
      // a record and record equality on List<T> members is reference equality, so an empty list would
      // break both the Moq assertions in SignificantChangeProjectRepositoryTests and the
      // serialised-request-body matching the integration test stubs rely on.
      GetSignificantProjectsQuery query = new(
         page,
         count,
         string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim(),
         statuses?.Length > 0 ? statuses.ToList() : null,
         assignees?.Length > 0 ? assignees.ToList() : null,
         tiers?.Length > 0 ? tiers.ToList() : null,
         routes?.Length > 0 ? routes.ToList() : null);

      ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>> result =
         await httpClientService.Post<GetSignificantProjectsQuery, ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
            httpClient,
            PathFor.GetAllSignificantChangeProjects,
            query);

      if (result.Success)
      {
         return new ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(result.StatusCode, result.Body);
      }

      return new ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
         result.StatusCode,
         new ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>
         {
            Data = Enumerable.Empty<SignificantChangeProjectResponse>(),
            Paging = new ApiV2PagingInfo
            {
               Page = page,
               RecordCount = 0,
               NextPageUrl = null
            }
         });
   }

      public async Task<ApiResponse<SignificantChangeFilterParameters>> GetFilterParameters()
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();

      ApiResponse<SignificantChangeFilterParameters> result =
         await httpClientService.Get<SignificantChangeFilterParameters>(
            httpClient,
            PathFor.GetSignificantChangeFilterParameters);

      // Fail soft, matching GetAllProjects above. An empty filter panel is a far better failure than
      // a 500 on the whole list page — and this endpoint may not be deployed yet.
      if (result.Success && result.Body is not null)
      {
         return new ApiResponse<SignificantChangeFilterParameters>(result.StatusCode, result.Body);
      }

      return new ApiResponse<SignificantChangeFilterParameters>(
         result.StatusCode,
         new SignificantChangeFilterParameters());
   }

   public async Task<ApiResponse<SignificantChangeProjectResponse>> GetProjectById(int id)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.GetSignificantChangeProjectById, id);

      ApiResponse<SignificantChangeProjectResponse> result =
         await httpClientService.Get<SignificantChangeProjectResponse>(
            httpClient,
            path);

      return new ApiResponse<SignificantChangeProjectResponse>(result.StatusCode, result.Body);
   }

   public async Task SetAssignedUser(int id, SetAssignedUserSignificantChangeCommand updatedAssignedUser)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.SetSignificantChangeAssignedUser, id);

      var result = await httpClientService.Put<SetAssignedUserSignificantChangeCommand, object>(
            httpClient,
            path,
            updatedAssignedUser);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }

   public async Task RecordDecision(SignificantChangeDecision decision)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();

      ApiResponse<SignificantChangeDecision> result =
         await httpClientService.Post<SignificantChangeDecision, SignificantChangeDecision>(
            httpClient,
            PathFor.RecordSignificantChangeDecision,
            decision);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }

   public async Task SetStakeholderConsultation(int id, SetSignificantChangeStakeholderConsultationCommand command)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.SetSignificantChangeStakeholderConsultation, id);

      var result = await httpClientService.Put<SetSignificantChangeStakeholderConsultationCommand, object>(
         httpClient,
         path,
         command);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }

}
