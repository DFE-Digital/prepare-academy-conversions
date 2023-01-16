using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Features;

public interface IApiClient
{
   Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel);
   Task<HttpResponseMessage> GetProjectByIdAsync(int id);
   Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject);
   Task<HttpResponseMessage> GetFilterParametersAsync();
   Task<(HttpResponseMessage, bool)> GetApplicationByReferenceAsync(string id);
   Task<HttpResponseMessage> AddProjectNote(int id, AddProjectNote projectNote);
}
