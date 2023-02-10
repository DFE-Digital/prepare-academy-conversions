using ApplyToBecome.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Application;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class ApplicationRepository
{
   private readonly IApiClient _apiClient;
   private readonly ILogger<ApplicationRepository> _logger;
   private readonly bool _useAcademisationApplication;

   public ApplicationRepository(IApiClient apiClient, IFeatureManager features, ILogger<ApplicationRepository> logger)
   {
      _apiClient = apiClient;
      _logger = logger;
      _useAcademisationApplication = features.IsEnabledAsync(FeatureFlags.UseAcademisationApplication).Result;
   }

   public async Task<ApiResponse<Application>> GetApplicationByReference(string id)
   {
      HttpResponseMessage response = await _apiClient.GetApplicationByReferenceAsync(id);

      if (response.IsSuccessStatusCode)
      {
         if (_useAcademisationApplication)
         {
            AcademisationApplication academisationOuterResponse = await response.Content.ReadFromJsonAsync<AcademisationApplication>();
            
            return new ApiResponse<Application>(response.StatusCode, AcademisationApplication.MapToApplication(academisationOuterResponse));
         }

         ApiV2Wrapper<Application> outerResponse = await response.Content.ReadFromJsonAsync<ApiV2Wrapper<Application>>();
         return new ApiResponse<Application>(response.StatusCode, outerResponse.Data);
      }

      _logger.LogWarning("Unable to get school application form data for establishment with id: {id}", id);
      return new ApiResponse<Application>(response.StatusCode, null);

   }
}
