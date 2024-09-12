
using Dfe.PrepareConversions.Data.Models.UserRole;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces
{
   public interface IUserRoleRepository
   {
      Task<ApiResponse<RoleCapabilitiesModel>> GetUserRoleCapabilities(string email);
   }
}
