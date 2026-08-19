using Dfe.PrepareConversions.Data.Models.SignificantChange;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces;

public interface ISignificantChangeProjectRepository
{
   Task<ApiResponse<SignificantChangeProjectResponse>> CreateProject(CreateSignificantProjectCommand command);

   Task<ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>> GetAllProjects(int page, int count);

   Task<ApiResponse<SignificantChangeProjectResponse>> GetProjectById(int id);

   Task SetAssignedUser(int id, SetAssignedUserSignificantChangeCommand updatedAssignedUser);

   Task SetStakeholderConsultation(int id, SetSignificantChangeStakeholderConsultationCommand command);

   Task SetAdmissionVariationConsultation(int id, SetSignificantChangeAdmissionVariationConsultationCommand command);

}
