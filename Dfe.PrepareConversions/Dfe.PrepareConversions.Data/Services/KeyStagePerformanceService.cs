using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class KeyStagePerformanceService
{
   private readonly HttpClient _httpClient;
   private readonly ILogger<KeyStagePerformanceService> _logger;

   public KeyStagePerformanceService(IDfeHttpClientFactory httpClientFactory, ILogger<KeyStagePerformanceService> logger)
   {
      _httpClient = httpClientFactory.CreateTramsClient();
      _logger = logger;
   }

   public async Task<KeyStagePerformance> GetKeyStagePerformance(string urn)
   {
      HttpResponseMessage response = await _httpClient.GetAsync($"/educationPerformance/{urn}");
      if (!response.IsSuccessStatusCode)
      {
         _logger.LogWarning("Unable to get key stage performance data for establishment with URN: {urn}", urn);
         return new KeyStagePerformance();
      }

      KeyStagePerformanceResponse keyStagePerformanceResponse = await response.Content.ReadFromJsonAsync<KeyStagePerformanceResponse>();

      return new KeyStagePerformance
      {
         KeyStage2 = keyStagePerformanceResponse.KeyStage2, KeyStage4 = keyStagePerformanceResponse.KeyStage4, KeyStage5 = keyStagePerformanceResponse.KeyStage5, SchoolAbsenceData = keyStagePerformanceResponse.absenceData
      };
   }
}
