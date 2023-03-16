using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.InvoluntaryProject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public interface IAcademyConversionProjectRepository
{
   Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjects(
      int page,
      int count,
      string titleFilter = "",
      IEnumerable<string> statusFilters = default,
      IEnumerable<string> deliveryOfficerFilter = default,
      IEnumerable<string> regionsFilter = default
   );

   Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id);
   Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject);
   Task CreateInvoluntaryProject(CreateInvoluntaryProject involuntaryProject);
   Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters();
   Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote addProjectNote);
}
