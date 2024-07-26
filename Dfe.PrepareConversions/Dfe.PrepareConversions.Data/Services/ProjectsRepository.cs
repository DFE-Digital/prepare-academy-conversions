using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class ProjectsRepository
{
   private readonly HttpClient _httpClient;
   private readonly IHttpClientService _httpClientService;

   public ProjectsRepository(IDfeHttpClientFactory httpClientFactory,
      IHttpClientService httpClientService)
   {
      _httpClient = httpClientFactory.CreateTramsClient();
      _httpClientService = httpClientService;
   }
   
   public async Task<TrustDtoResponse> GetProjectsForGroup(string referenceNumber)
   {
      string projectsForGroupPath = $"projects-for-group/{referenceNumber}";

      ApiResponse<TrustDtoResponse> result = await _httpClientService.Get<TrustDtoResponse>(_httpClient, projectsForGroupPath );

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

      return result.Body;
   }
}