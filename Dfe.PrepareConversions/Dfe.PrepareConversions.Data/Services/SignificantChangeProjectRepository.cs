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
      ISignificantChangeProjectRepository.SignificantChangeFilterOptions? filterOptions = null)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();

      // Empty collections must normalise to null, not to empty lists. GetSignificantProjectsQuery is
      // a record and record equality on List<T> members is reference equality, so an empty list would
      // break both the Moq assertions in SignificantChangeProjectRepositoryTests and the
      // serialised-request-body matching the integration test stubs rely on.
      GetSignificantProjectsQuery query = new(
         page,
         count,
         string.IsNullOrWhiteSpace(filterOptions?.Keyword) ? null : filterOptions.Keyword.Trim(),
         filterOptions?.Statuses?.Length > 0 ? filterOptions.Statuses.ToList() : null,
         filterOptions?.Assignees?.Length > 0 ? filterOptions.Assignees.ToList() : null,
         filterOptions?.Tiers?.Length > 0 ? filterOptions.Tiers.ToList() : null,
         filterOptions?.Routes?.Length > 0 ? filterOptions.Routes.ToList() : null,
         filterOptions?.LocalAuthorities?.Length > 0 ? [.. filterOptions.LocalAuthorities] : null,
         filterOptions?.Regions?.Length > 0 ? [.. filterOptions.Regions] : null);

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

   public async Task UpdateDecision(SignificantChangeDecision decision)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();

      ApiResponse<SignificantChangeDecision> result =
         await httpClientService.Put<SignificantChangeDecision, SignificantChangeDecision>(
            httpClient,
            PathFor.RecordSignificantChangeDecision,
            decision);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }

   public async Task<ApiResponse<SignificantChangeDecision>> GetDecision(int id)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.GetSignificantChangeDecision, id);

      ApiResponse<SignificantChangeDecision> result =
         await httpClientService.Get<SignificantChangeDecision>(
            httpClient,
            path);

      return new ApiResponse<SignificantChangeDecision>(result.StatusCode, result.Body);
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

   public async Task SetReligiousBodyConsultation(int id, SetSignificantChangeReligiousBodyConsultationCommand command)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.SetSignificantChangeReligiousBodyConsultation, id);

      var result = await httpClientService.Put<SetSignificantChangeReligiousBodyConsultationCommand, object>(
         httpClient,
         path,
         command);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }

   public async Task SetProjectDates(int id, SetSignificantChangeProjectDatesCommand command)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.SetSignificantChangeProjectDates, id);

      var result = await httpClientService.Put<SetSignificantChangeProjectDatesCommand, object>(
         httpClient,
         path,
         command);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }
   
   public async Task SetEqualitiesImpactAssessment(int id, SetSignificantChangeEqualitiesImpactAssessmentCommand command)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.SetSignificantChangeEqualitiesImpactAssessment, id);

      var result = await httpClientService.Put<SetSignificantChangeEqualitiesImpactAssessmentCommand, object>(
         httpClient,
         path,
         command);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }
      
   public async Task SetAdmissionVariationConsultation(int id, SetSignificantChangeAdmissionVariationConsultationCommand command)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.SetAdmissionVariationConsultation, id);

      var result = await httpClientService.Put<SetSignificantChangeAdmissionVariationConsultationCommand, object>(
         httpClient,
         path,
         command);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }

   public async Task SetStakeholderObjections(int id, SetSignificantChangeStakeholderObjectionsCommand command)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.SetSignificantChangeStakeholderObjections, id);

      var result = await httpClientService.Put<SetSignificantChangeStakeholderObjectionsCommand, object>(
           httpClient,
         path,
         command);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }
   
   public async Task SetConsultationDuration(int id, SetSignificantChangeConsultationDurationCommand command)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      string path = string.Format(PathFor.SetSignificantChangeConsultationDuration, id);

      var result = await httpClientService.Put<SetSignificantChangeConsultationDurationCommand, object>(
         httpClient,
         path,
         command);

      if (!result.Success)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }
   }
}
