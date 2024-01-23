using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.NewProject;
using Microsoft.AspNetCore.Mvc;
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
      IEnumerable<string> regionsFilter = default,
      IEnumerable<string> applicationReferences = default
   );
   Task<ApiResponse<FileStreamResult>> DownloadProjectExport(
   int page,
   int count,
   string titleFilter = "",
   IEnumerable<string> statusFilters = default,
   IEnumerable<string> deliveryOfficerFilter = default,
   IEnumerable<string> regionsFilter = default,
   IEnumerable<string> localAuthoritiesFilter = default,
   IEnumerable<string> advisoryBoardDatesFilter = default
);

   Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjectsV2(
   int page,
   int count,
   string titleFilter = "",
   IEnumerable<string> statusFilters = default,
   IEnumerable<string> deliveryOfficerFilter = default,
   IEnumerable<string> regionsFilter = default,
   IEnumerable<string> localAuthoritiesFilter = default,
   IEnumerable<string> advisoryBoardDatesFilter = default
);

   Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id);
   Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject);
   Task CreateProject(CreateNewProject newProject);
   Task SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl);
   Task SetSchoolOverview(int id, SetSchoolOverviewModel updatedSchoolOverview);
   Task SetPerformanceData(int id, SetPerformanceDataModel setPerformanceDataModel);
   Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters();
   Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote addProjectNote);
}
