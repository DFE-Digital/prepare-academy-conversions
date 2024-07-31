using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.NewProject;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
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

   Task<ApiResponse<ApiV2Wrapper<IEnumerable<FormAMatProject>>>> GetFormAMatProjects(
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
   Task<ApiResponse<FormAMatProject>> GetFormAMatProjectById(int id);
   Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject);
   Task<ApiResponse<AcademyConversionProject>> CreateProject(CreateNewProject newProject);
   Task<ApiResponse<IEnumerable<AcademyConversionProject>>> GetProjectsForGroup(string id);
   Task CreateFormAMatProject(CreateNewFormAMatProject newProject);
   Task SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl);
   Task SetAssignedUser(int id, SetAssignedUserModel updatedAssignedUser);
   Task SetFormAMatAssignedUser(int id, SetAssignedUserModel updatedAssignedUser);
   Task SetSchoolOverview(int id, SetSchoolOverviewModel updatedSchoolOverview);
   Task SetPerformanceData(int id, SetPerformanceDataModel setPerformanceDataModel);
   Task SetIncomingTrust(int id, SetIncomingTrustDataModel setIncomingTrustDataModel);
   Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters();
   Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote addProjectNote);
   Task<ApiResponse<SchoolImprovementPlan>> AddSchoolImprovementPlan(int id, AddSchoolImprovementPlan addSchoolImprovementPlanCommand);
   Task<ApiResponse<IEnumerable<FormAMatProject>>> SearchFormAMatProjects(string searchTerm);
   Task SetFormAMatProjectReference(int id, SetFormAMatProjectReference setFormAMatProjectReference);

   Task<ApiResponse<IEnumerable<SchoolImprovementPlan>>> GetSchoolImprovementPlansForProject(int id);
   Task UpdateSchoolImprovementPlan(int id, UpdateSchoolImprovementPlan updateSchoolImprovementPlan);
   Task SetProjectDates(int id, SetProjectDatesModel updatedProjectDates);
   Task<ApiResponse<IEnumerable<OpeningDateHistoryDto>>> GetOpeningDateHistoryForConversionProject(int id);

   Task<ApiResponse<ApiV2Wrapper<IEnumerable<ProjectGroup>>>> GetProjectGroups(
   int page,
   int count,
   string titleFilter = "",
   IEnumerable<string> statusFilters = default,
   IEnumerable<string> deliveryOfficerFilter = default,
   IEnumerable<string> regionsFilter = default,
   IEnumerable<string> localAuthoritiesFilter = default,
   IEnumerable<string> advisoryBoardDatesFilter = default
);

}
