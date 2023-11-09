using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class EstablishmentService : IGetEstablishment
{
   private readonly HttpClient _httpClient;
   private readonly IHttpClientService _httpClientService;
   private readonly ILogger<EstablishmentService> _logger;

   public EstablishmentService(IDfeHttpClientFactory httpClientFactory,
                               ILogger<EstablishmentService> logger,
                               IHttpClientService httpClientService)
   {
      _httpClient = httpClientFactory.CreateTramsClient();
      _logger = logger;
      _httpClientService = httpClientService;
   }

   public async Task<EstablishmentResponse> GetEstablishmentByUrn(string urn)
   {
      HttpResponseMessage response = await _httpClient.GetAsync($"/v4/establishment/urn/{urn}");
      if (!response.IsSuccessStatusCode)
      {
         _logger.LogWarning("Unable to get establishment data for establishment with URN: {urn}", urn);
         return new EstablishmentResponse();
      }

      return await response.Content.ReadFromJsonAsync<EstablishmentResponse>();
   }

   public async Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery)
   {
      string path = int.TryParse(searchQuery, out int urn)
         ? $"/v4/establishments?urn={urn}"
         : $"/v4/establishments?name={searchQuery}";

      ApiResponse<IEnumerable<EstablishmentSearchResponse>> result = await _httpClientService.Get<IEnumerable<EstablishmentSearchResponse>>(_httpClient, path);

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

      return result.Body;
   }
}
