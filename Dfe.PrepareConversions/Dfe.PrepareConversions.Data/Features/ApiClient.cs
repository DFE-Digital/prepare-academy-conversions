using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Microsoft.FeatureManagement;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Features;

public class ApiClient : IApiClient
{
   private readonly IDfeHttpClientFactory _httpClientFactory;
   private readonly PathFor _pathFor;
   private readonly bool _useAcademisationApplication;

   public ApiClient(IDfeHttpClientFactory httpClientFactory, IFeatureManager features, PathFor pathFor)
   {
      _pathFor = pathFor;
      _useAcademisationApplication = features.IsEnabledAsync(FeatureFlags.UseAcademisationApplication).Result;
      _httpClientFactory = httpClientFactory;
   }

   private HttpClient TramsClient => _httpClientFactory.CreateTramsClient();
   private HttpClient AcademisationClient => _httpClientFactory.CreateAcademisationClient();
   private HttpClient ActiveApplicationClient => _useAcademisationApplication ? AcademisationClient : TramsClient;

   public async Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel)
   {
      return await AcademisationClient.PostAsync(PathFor.GetAllProjects, JsonContent.Create(searchModel));
   }
   public async Task<HttpResponseMessage> DownloadProjectExport(AcademyConversionSearchModelV2 searchModel)
   {
      return await AcademisationClient.PostAsync(PathFor.DownloadProjectExport, JsonContent.Create(searchModel));
   }

   public async Task<HttpResponseMessage> GetProjectByIdAsync(int id)
   {
      HttpResponseMessage getProjectResponse = await AcademisationClient.GetAsync(string.Format(PathFor.GetProjectById, id));
      return getProjectResponse;
   }

   public async Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject)
   {
      return await AcademisationClient.PatchAsync(string.Format(PathFor.UpdateProject, id), JsonContent.Create(updateProject));
   }

   public async Task<HttpResponseMessage> GetFilterParametersAsync()
   {
      return await AcademisationClient.GetAsync(PathFor.GetFilterParameters);
   }

   public async Task<HttpResponseMessage> GetApplicationByReferenceAsync(string id)
   {
      return await ActiveApplicationClient.GetAsync(string.Format(_pathFor.GetApplicationByReference, id));
   }

   public async Task<HttpResponseMessage> AddProjectNote(int id, AddProjectNote projectNote)
   {
      return await AcademisationClient.PostAsync(string.Format(PathFor.AddProjectNote, id), JsonContent.Create(projectNote));
   }

   public async Task<HttpResponseMessage> SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl)
   {
      var payload = new
      {
         externalApplicationFormSaved = externalApplicationFormSaved,
         externalApplicationFormUrl = externalApplicationFormUrl ?? string.Empty
      };

      return await AcademisationClient.PutAsync(string.Format(PathFor.SetExternalApplicationForm, id), JsonContent.Create(payload));
   }

   public async Task<HttpResponseMessage> GetAllProjectsV2Async(AcademyConversionSearchModelV2 searchModel)
   {
      return await AcademisationClient.PostAsync(PathFor.GetAllProjectsV2, JsonContent.Create(searchModel));
   }
}
