using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services.Interfaces;
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

   public async Task<TrustSummaryResponse> SearchTrusts(string searchQuery)
   {
      string path = $"/v2/trusts?groupName={searchQuery}&ukPrn={searchQuery}&companiesHouseNumber={searchQuery}&page=1&count=1000000";

      ApiResponse<TrustSummaryResponse> result = await _httpClientService.Get<TrustSummaryResponse>(_httpClient, path);

      if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

      return result.Body;
   }

   public async Task<TrustDetail> GetTrustByUkprn(string ukprn)
   {
      if (string.IsNullOrWhiteSpace(ukprn)) return default;

      string path = $@"/v2/trusts/bulk?Ukprn={ukprn.Trim()}&Establishments=false";

      ApiResponse<TrustDetailResponse> result = await _httpClientService.Get<TrustDetailResponse>(_httpClient, path);

      return result.Body.Data.FirstOrDefault();
   }
}
