
#nullable enable

using Dfe.PrepareConversions.Data.Models.SignificantChange;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces;

public interface ISignificantChangeProjectRepository
{
   Task<ApiResponse<SignificantChangeProjectResponse>> CreateProject(CreateSignificantProjectCommand command);

   Task<ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>> GetAllProjects(
      int page,
      int count,
      string? keyword = null,
      string[]? statuses = null,
      string[]? assignees = null,
      byte[]? tiers = null,
      string[]? routes = null);

   Task<ApiResponse<SignificantChangeProjectResponse>> GetProjectById(int id);

   Task<ApiResponse<SignificantChangeFilterParameters>> GetFilterParameters();

   Task SetAssignedUser(int id, SetAssignedUserSignificantChangeCommand updatedAssignedUser);

   Task SetStakeholderConsultation(int id, SetSignificantChangeStakeholderConsultationCommand command);

}
