using ApplyToBecome.Data.Models;
using Microsoft.FeatureManagement;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Features;

public static class FeatureFlags
{
   public const string UseAcademisation = nameof(UseAcademisation);
}

public interface IApiClient
{
   Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel);
   Task<HttpResponseMessage> GetSelectedRegionsAsync(string regionQueryString);
   Task<HttpResponseMessage> GetProjectByIdAsync(int id);
   Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject);
   Task<HttpResponseMessage> GetFilterParametersAsync();
}

public class ApiClient : IApiClient
{
   private readonly HttpClient _academisationClient;
   private readonly PathFor _pathFor;
   private readonly HttpClient _tramsClient;
   private readonly bool _useAcademisation;

   public ApiClient(IHttpClientFactory httpClientFactory, IFeatureManager features, PathFor pathFor)
   {
      _pathFor = pathFor;
      _tramsClient = httpClientFactory.CreateClient("TramsClient");
      _academisationClient = httpClientFactory.CreateClient("AcademisationClient");
      _useAcademisation = features.IsEnabledAsync(FeatureFlags.UseAcademisation).Result;
   }

   private HttpClient ActiveClient => _useAcademisation ? _academisationClient : _tramsClient;

   public async Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel)
   {
      return await ActiveClient.PostAsync(_pathFor.GetAllProjects, JsonContent.Create(searchModel));
   }

   public async Task<HttpResponseMessage> GetSelectedRegionsAsync(string regionQueryString)
   {
      return await _tramsClient.GetAsync(string.Format(_pathFor.GetSelectedRegions, regionQueryString.ToLowerInvariant()));
   }

   public async Task<HttpResponseMessage> GetProjectByIdAsync(int id)
   {
      return await ActiveClient.GetAsync(string.Format(_pathFor.GetProjectById, id));
   }

   public async Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject)
   {
      return await ActiveClient.PatchAsync(string.Format(_pathFor.UpdateProject, id), JsonContent.Create(updateProject));
   }

   public async Task<HttpResponseMessage> GetFilterParametersAsync()
   {
      return await ActiveClient.GetAsync(_pathFor.GetFilterParameters);
   }
}

public class PathFor
{
   private readonly bool _useAcademisation;

   private PathFor(bool useAcademisation)
   {
      _useAcademisation = useAcademisation;
   }

   public PathFor(IFeatureManager features)
   {
      _useAcademisation = features.IsEnabledAsync(FeatureFlags.UseAcademisation).Result;
   }

   public string GetAllProjects => _useAcademisation ? "/legacy/projects" : "v2/conversion-projects";
   public string GetSelectedRegions => "establishment/regions?{0}";
   public string GetProjectById => _useAcademisation ? "/legacy/project/{0}" : "conversion-projects/{0}";
   public string UpdateProject => _useAcademisation ? "/legacy/project/{0}" : "conversion-projects/{0}";
   public string GetFilterParameters => _useAcademisation ? "/legacy/projects/status" : "v2/conversion-projects/parameters";

   public static PathFor UsingAcademisation(bool useAcademisation)
   {
      return new PathFor(useAcademisation);
   }
}
