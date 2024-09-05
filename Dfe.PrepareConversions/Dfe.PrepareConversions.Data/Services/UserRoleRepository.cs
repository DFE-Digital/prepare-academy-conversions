using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.UserRole;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services
{
   public class UserRoleRepository(IDfeHttpClientFactory httpClientFactory,
      IHttpClientService httpClientService) : IUserRoleRepository
   {
      public async Task<ApiResponse<RoleCapabilitiesModel>> GetUserRoleCapabilities(string email)
      {
         var httpClient = httpClientFactory.CreateAcademisationClient();

         ApiResponse<RoleCapabilitiesModel> result = await httpClientService.Get<RoleCapabilitiesModel>(
            httpClient,
            $"/user-role/{email}");

         if (result.Success is false)
         {
            throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
         }
         return new ApiResponse<RoleCapabilitiesModel>(result.StatusCode, result.Body);
      }
   }
}
