using ApplyToBecome.Data.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Features;

public interface IApiClient
{
   Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel);
   Task<HttpResponseMessage> GetSelectedRegionsAsync(string regionQueryString);
   Task<HttpResponseMessage> GetProjectByIdAsync(int id);
   Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject);
   Task<HttpResponseMessage> GetFilterParametersAsync();
   Task<HttpResponseMessage> GetApplicationByReferenceAsync(string id);
}
