using ApplyToBecome.Data.Features;
using ApplyToBecome.Data.Models.AcademisationApplication;
using ApplyToBecome.Data.Models.Application;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services;

public class ApplicationRepository
{
   private readonly IApiClient _apiClient;
   private readonly ILogger<ApplicationRepository> _logger;

   public ApplicationRepository(IApiClient apiClient, ILogger<ApplicationRepository> logger)
   {
      _apiClient = apiClient;
      _logger = logger;
   }

   public async Task<ApiResponse<Application>> GetApplicationByReference(string id)
   {
      (HttpResponseMessage, bool) response = await _apiClient.GetApplicationByReferenceAsync(id);

      if (response.Item1.IsSuccessStatusCode)
      {
         if (response.Item2)
         {
            ApiV2Wrapper<AcademisationApplication> academisationOuterResponse = await response.Item1.Content.ReadFromJsonAsync<ApiV2Wrapper<AcademisationApplication>>();
            AcademisationApplication.MapToApplication();
            return new ApiResponse<Application>(response.Item1.StatusCode, academisationOuterResponse.Data);
         }
         ApiV2Wrapper<Application> outerResponse = await response.Item1.Content.ReadFromJsonAsync<ApiV2Wrapper<Application>>();
         return new ApiResponse<Application>(response.Item1.StatusCode, outerResponse.Data);
      }

      _logger.LogWarning("Unable to get school application form data for establishment with id: {id}", id);
      return new ApiResponse<Application>(response.Item1.StatusCode, null);
   }
}
