﻿using ApplyToBecome.Data.Models;
using Microsoft.FeatureManagement;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace ApplyToBecome.Data.Features;

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

   public async Task<HttpResponseMessage> GetSelectedRegionsAsync(IEnumerable<string> regions)
   {
      string regionQueryString = $@"{regions.Aggregate(string.Empty, (current, region) => $"{current}&regions={HttpUtility.UrlEncode(region)}")}";

      return await TramsClient.GetAsync(string.Format(PathFor.GetSelectedRegions, regionQueryString.ToLowerInvariant()));
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

   public async Task<HttpResponseMessage> GetApplicationByReferenceAsync(string id)
   {
      return await ActiveClient.GetAsync(string.Format(_pathFor.GetApplicationByReference, id));
   }
}