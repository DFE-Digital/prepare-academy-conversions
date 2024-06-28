using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.NewProject;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Services;

public class AcademyConversionProjectItemsCacheDecorator : IAcademyConversionProjectRepository
{
   private readonly HttpContext _httpContext;
   private readonly IAcademyConversionProjectRepository _innerRepository;

   public AcademyConversionProjectItemsCacheDecorator(
      IAcademyConversionProjectRepository innerRepository,
      IHttpContextAccessor httpContextAccessor)
   {
      _innerRepository = innerRepository;
      _httpContext = httpContextAccessor.HttpContext;
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjects(int page, int count, string titleFilter = "",
      IEnumerable<string> statusFilters = default, IEnumerable<string> deliveryOfficerFilter = default, IEnumerable<string> regionsFilter = default, IEnumerable<string> applicationReferences = default)
   {
      return await _innerRepository.GetAllProjects(page, count, titleFilter, statusFilters, deliveryOfficerFilter, regionsFilter, applicationReferences);
   }
   public async Task<ApiResponse<FileStreamResult>> DownloadProjectExport(
   int page,
   int count,
   string titleFilter = "",
   IEnumerable<string> statusFilters = default,
   IEnumerable<string> deliveryOfficerFilter = default,
   IEnumerable<string> regionsFilter = default,
   IEnumerable<string> localAuthoritiesFilter = default,
   IEnumerable<string> advisoryBoardDatesFilter = default
)
   {
      return await _innerRepository.DownloadProjectExport(page, count, titleFilter, statusFilters, deliveryOfficerFilter, regionsFilter, localAuthoritiesFilter, advisoryBoardDatesFilter);
   }
   public async Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id)
   {
      if (_httpContext.Items.ContainsKey(id) && _httpContext.Items[id] is ApiResponse<AcademyConversionProject> cached)
      {
         return cached;
      }

      ApiResponse<AcademyConversionProject> project = await _innerRepository.GetProjectById(id);

      // only cache if object isn't null.
      if (project != null)
      {
         _httpContext.Items.Add(id, project);
      }

      return project;
   }
   public async Task<ApiResponse<FormAMatProject>> GetFormAMatProjectById(int id)
   {
      ApiResponse<FormAMatProject> project = await _innerRepository.GetFormAMatProjectById(id);
      return project;
   }
   public async Task<ApiResponse<AcademyConversionProject>> CreateProject(CreateNewProject sponsoredProject)
   {
      ApiResponse<AcademyConversionProject> project = await _innerRepository.CreateProject(sponsoredProject);
      return project;
   }
   public async Task CreateFormAMatProject(CreateNewFormAMatProject sponsoredProject)
   {
      await _innerRepository.CreateFormAMatProject(sponsoredProject);
   }

   public async Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject)
   {
      if (_httpContext.Items.ContainsKey(id))
      {
         _httpContext.Items.Remove(id);
      }

      return await _innerRepository.UpdateProject(id, updateProject);
   }

   public async Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters()
   {
      return await _innerRepository.GetFilterParameters();
   }

   public async Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote addProjectNote)
   {
      return await _innerRepository.AddProjectNote(id, addProjectNote);
   }

   public async Task<ApiResponse<SchoolImprovementPlan>> AddSchoolImprovementPlan(int id, AddSchoolImprovementPlan addSchoolImprovementPlan)
   {
      return await _innerRepository.AddSchoolImprovementPlan(id, addSchoolImprovementPlan);
   }

   public async Task SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl)
   {
      await _innerRepository.SetProjectExternalApplicationForm(id, externalApplicationFormSaved, externalApplicationFormUrl);
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjectsV2(int page, int count, string titleFilter = "", IEnumerable<string> statusFilters = null, IEnumerable<string> deliveryOfficerFilter = null, IEnumerable<string> regionsFilter = null, IEnumerable<string> localAuthoritiesFilter = null, IEnumerable<string> advisoryBoardDatesFilter = null)
   {
      return await _innerRepository.GetAllProjectsV2(page, count, titleFilter, statusFilters, deliveryOfficerFilter, regionsFilter, localAuthoritiesFilter, advisoryBoardDatesFilter);
   }
   public async Task SetSchoolOverview(int id, SetSchoolOverviewModel updatedSchoolOverview)
   {
      await _innerRepository.SetSchoolOverview(id, updatedSchoolOverview);
   }
   public async Task SetAssignedUser(int id, SetAssignedUserModel updatedAssignedUser)
   {
      await _innerRepository.SetAssignedUser(id, updatedAssignedUser);
   }
   public async Task SetFormAMatAssignedUser(int id, SetAssignedUserModel updatedAssignedUser)
   {
      await _innerRepository.SetFormAMatAssignedUser(id, updatedAssignedUser);
   }
   public async Task SetPerformanceData(int id, SetPerformanceDataModel setPerformanceDataModel)
   {
      await _innerRepository.SetPerformanceData(id, setPerformanceDataModel);
   }

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<FormAMatProject>>>> GetFormAMatProjects(int page, int count, string titleFilter = "", IEnumerable<string> statusFilters = null, IEnumerable<string> deliveryOfficerFilter = null, IEnumerable<string> regionsFilter = null, IEnumerable<string> localAuthoritiesFilter = null, IEnumerable<string> advisoryBoardDatesFilter = null)
   {
      return await _innerRepository.GetFormAMatProjects(page, count, titleFilter, statusFilters, deliveryOfficerFilter, regionsFilter, localAuthoritiesFilter, advisoryBoardDatesFilter);
   }

   public async Task SetIncomingTrust(int id, SetIncomingTrustDataModel setIncomingTrustDataModel)
   {
      await _innerRepository.SetIncomingTrust(id, setIncomingTrustDataModel);
   }

   public async Task<ApiResponse<IEnumerable<FormAMatProject>>> SearchFormAMatProjects(string searchTerm)
   {
      return await _innerRepository.SearchFormAMatProjects(searchTerm);
   }

   public async Task SetFormAMatProjectReference(int id, SetFormAMatProjectReference setFormAMatProjectReference)
   {
      await _innerRepository.SetFormAMatProjectReference(id, setFormAMatProjectReference);
   }

   public async Task<ApiResponse<IEnumerable<SchoolImprovementPlan>>> GetSchoolImprovementPlansForProject(int id)
   {
      return await _innerRepository.GetSchoolImprovementPlansForProject(id);
   }

   public async Task UpdateSchoolImprovementPlan(int id, UpdateSchoolImprovementPlan updateSchoolImprovementPlan)
   {
      await _innerRepository.UpdateSchoolImprovementPlan(id, updateSchoolImprovementPlan);
   }
}
