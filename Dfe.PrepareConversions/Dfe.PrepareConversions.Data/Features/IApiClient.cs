using Dfe.PrepareConversions.Data.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Features;

public interface IApiClient
{
   Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel);
   Task<HttpResponseMessage> DownloadProjectExport(AcademyConversionSearchModelV2 searchModel);
   Task<HttpResponseMessage> GetAllProjectsV2Async(AcademyConversionSearchModelV2 searchModel);
   Task<HttpResponseMessage> GetProjectByIdAsync(int id);
   Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject);
   Task<HttpResponseMessage> GetFilterParametersAsync();
   Task<HttpResponseMessage> GetApplicationByReferenceAsync(string id);
   Task<HttpResponseMessage> AddProjectNote(int id, AddProjectNote projectNote);

   Task<HttpResponseMessage> SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl);
   Task<HttpResponseMessage> SetSchoolOverview(int id, SetSchoolOverviewModel updatedSchoolOverview);
   Task<HttpResponseMessage> GetMATProjectsAsync(AcademyConversionSearchModelV2 searchModel);
}
