using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Features;

public interface IApiClient
{
   Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel);
   Task<HttpResponseMessage> DownloadProjectExport(AcademyConversionSearchModelV2 searchModel);
   Task<HttpResponseMessage> GetAllProjectsV2Async(AcademyConversionSearchModelV2 searchModel);
   Task<HttpResponseMessage> GetProjectByIdAsync(int id);
   Task<HttpResponseMessage> GetFormAMatProjectById(int id);
   Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject);
   Task<HttpResponseMessage> GetProjectsForGroup(string id);
   Task<HttpResponseMessage> GetFilterParametersAsync();
   Task<HttpResponseMessage> GetApplicationByReferenceAsync(string id);
   Task<HttpResponseMessage> AddProjectNote(int id, AddProjectNote projectNote);
   Task<HttpResponseMessage> AddSchoolImprovementPlan(int id, AddSchoolImprovementPlan addSchoolImprovementPlanCommand);

   Task<HttpResponseMessage> SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl);
   Task<HttpResponseMessage> SetSchoolOverview(int id, SetSchoolOverviewModel updatedSchoolOverview);
   Task<HttpResponseMessage> SetAssignedUser(int id, SetAssignedUserModel updatedAssignedUser);
   Task<HttpResponseMessage> SetFormAMatAssignedUser(int id, SetAssignedUserModel updatedAssignedUser);
   Task<HttpResponseMessage> SetPerformanceData(int id, SetPerformanceDataModel setPerformanceDataModel);
   Task<HttpResponseMessage> SetIncomingTrust(int id, SetIncomingTrustDataModel setIncomingTrustDataModel);
   Task<HttpResponseMessage> GetFormAMatProjectsAsync(AcademyConversionSearchModelV2 searchModel);
   Task<HttpResponseMessage> SearchFormAMatProjects(string searchTerm);
   Task<HttpResponseMessage> SetFormAMatProjectReference(int id, SetFormAMatProjectReference setFormAMatProjectReference);
   Task<HttpResponseMessage> SetProjectDates(int id, SetProjectDatesModel updatedProjectDates);
   Task<HttpResponseMessage> GetOpeningDateHistoryForConversionProject(int id);
   
   Task<HttpResponseMessage> CreateNewProjectGroup(CreateProjectGroup createProjectGroup);
   Task<HttpResponseMessage> GetProjectGroupsAsync(AcademyConversionSearchModelV2 searchModel);
   Task<HttpResponseMessage> DeleteConversionProject(int id);
}
