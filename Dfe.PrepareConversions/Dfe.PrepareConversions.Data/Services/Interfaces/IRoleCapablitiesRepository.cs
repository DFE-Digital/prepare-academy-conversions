
using Dfe.PrepareConversions.Data.Models.UserRole;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces
{
   public interface IRoleCapablitiesRepository
   {
      Task<ApiResponse<RoleCapabilitiesModel>> GetRolesCapabilities(List<string> roles);
   }
}
