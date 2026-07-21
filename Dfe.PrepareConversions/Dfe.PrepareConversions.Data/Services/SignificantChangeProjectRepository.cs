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

      if (result.Success is false)
      {
         throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
      }

      return new ApiResponse<SignificantChangeProjectResponse>(result.StatusCode, result.Body);
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>> GetAllProjects(int page, int count)
   {
      HttpClient httpClient = httpClientFactory.CreateAcademisationClient();
      GetSignificantProjectsQuery query = new(page, count);

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
}
