using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.Application;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

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
      HttpResponseMessage response = await _apiClient.GetApplicationByReferenceAsync(id);

      if (response.IsSuccessStatusCode)
      {
         ApiV2Wrapper<Application> outerResponse = await response.Content.ReadFromJsonAsync<ApiV2Wrapper<Application>>();
         return new ApiResponse<Application>(response.StatusCode, outerResponse.Data);
      }

      _logger.LogWarning("Unable to get school application form data for establishment with id: {id}", id);
      return new ApiResponse<Application>(response.StatusCode, null);
   }
}
