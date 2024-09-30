using Dfe.Prepare.Data;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.UserRole;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services
{
   public class RoleCapablitiesRepository(IDfeHttpClientFactory httpClientFactory,
      IHttpClientService httpClientService) : IRoleCapablitiesRepository
   {
      public async Task<ApiResponse<RoleCapabilitiesModel>> GetRolesCapabilities(List<string> roles)
      {
         var httpClient = httpClientFactory.CreateAcademisationClient();

         var result = await httpClientService.Post<List<string>, RoleCapabilitiesModel>(
            httpClient,
             PathFor.GetRoleCapabilities, roles);

         if (result.Success is false)
         {
            throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
         }
         return new ApiResponse<RoleCapabilitiesModel>(result.StatusCode, result.Body);
      }
   }
}
