using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class TrustsRepository : ITrustsRepository
{
   private readonly HttpClient _httpClient;
   private readonly IHttpClientService _httpClientService;

   public TrustsRepository(
      IDfeHttpClientFactory httpClientFactory,
      IHttpClientService httpClientService)
   {
      _httpClient = httpClientFactory.CreateTramsClient();
      _httpClientService = httpClientService;
   }

   public async Task<TrustDtoResponse> SearchTrusts(string searchQuery)
   {
      string trustPath = $"/v4/trusts?groupName={searchQuery}&ukPrn={searchQuery}&companiesHouseNumber={searchQuery}&page=1&count=1000000";

      ApiResponse<TrustDtoResponse> result = await _httpClientService.Get<TrustDtoResponse>(_httpClient, trustPath);

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

      return result.Body;
   }

   public async Task<TrustDto> GetTrustByUkprn(string ukprn)
   {
      if (string.IsNullOrWhiteSpace(ukprn)) return default;
      string[] array = new string[] { ukprn.Trim() };
      string path = $@"/v4/trusts/bulk?Ukprns={array[0]}";
      ApiResponse<List<TrustDto>> result = await _httpClientService.Get<List<TrustDto>>(_httpClient, path);
      return result.Body.FirstOrDefault();
   }
}
