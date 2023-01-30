using Dfe.PrepareConversions.Data.Models;
using Microsoft.FeatureManagement;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace Dfe.PrepareConversions.Data.Features;

public class ApiClient : IApiClient
{
   private readonly IHttpClientFactory _httpClientFactory;
   private readonly PathFor _pathFor;
   private readonly bool _useAcademisation;

   public ApiClient(IHttpClientFactory httpClientFactory, IFeatureManager features, PathFor pathFor)
   {
      _pathFor = pathFor;
      _httpClientFactory = httpClientFactory;
      _useAcademisation = features.IsEnabledAsync(FeatureFlags.UseAcademisation).Result;
   }

   private HttpClient TramsClient => _httpClientFactory.CreateClient("TramsClient");
   private HttpClient AcademisationClient => _httpClientFactory.CreateClient("AcademisationClient");
   private HttpClient ActiveClient => _useAcademisation ? AcademisationClient : TramsClient;

   public async Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel)
   {
      return await ActiveClient.PostAsync(_pathFor.GetAllProjects, JsonContent.Create(searchModel));
   }


   public async Task<HttpResponseMessage> GetProjectByIdAsync(int id)
   {
      Task<HttpResponseMessage> getProjectResponse = ActiveClient.GetAsync(string.Format(_pathFor.GetProjectById, id));
      Task<HttpResponseMessage> getProjectNotesResponse =
         _useAcademisation ? Task.FromResult(new HttpResponseMessage()) : TramsClient.GetAsync(string.Format(PathFor.GetProjectNotes, id));

      await Task.WhenAll(getProjectResponse, getProjectNotesResponse);

      if (_useAcademisation) return getProjectResponse.Result;
      if (getProjectResponse.Result.IsSuccessStatusCode is false) return getProjectResponse.Result;

      AcademyConversionProject project = await getProjectResponse.Result.Content.ReadFromJsonAsync<AcademyConversionProject>();
      if (project is null) return new HttpResponseMessage(HttpStatusCode.NotFound);

      if(getProjectNotesResponse.Result.IsSuccessStatusCode )
         project.Notes = (await getProjectNotesResponse.Result.Content.ReadFromJsonAsync<IEnumerable<ProjectNote>>())?.ToList();

      return new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(project) };
   }

   public async Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject)
   {
      return await ActiveClient.PatchAsync(string.Format(_pathFor.UpdateProject, id), JsonContent.Create(updateProject));
   }

   public async Task<HttpResponseMessage> GetFilterParametersAsync()
   {
      return await ActiveClient.GetAsync(_pathFor.GetFilterParameters);
   }

   public async Task<HttpResponseMessage> GetApplicationByReferenceAsync(string id)
   {
      var test = await ActiveClient.GetAsync(string.Format(_pathFor.GetApplicationByReference, id));
      return await ActiveClient.GetAsync(string.Format(_pathFor.GetApplicationByReference, id));
   }

   public async Task<HttpResponseMessage> AddProjectNote(int id, AddProjectNote projectNote)
   {
      return await ActiveClient.PostAsync(string.Format(_pathFor.AddProjectNote, id), JsonContent.Create(projectNote));
   }
}
