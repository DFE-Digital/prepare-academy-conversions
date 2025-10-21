using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareTransfers.Data.Models;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Data.TRAMS;

public class TramsTrustsRepository : ITrusts
{
   private readonly IMapper<TrustDto, Trust> _trustMapper;

   public TramsTrustsRepository(IDfeHttpClientFactory httpClientFactory,
      IMapper<TrustDto, Trust> trustMapper
   )
   {
      _httpClientFactory = httpClientFactory;
      _trustMapper = trustMapper;
   }

   private readonly IDfeHttpClientFactory _httpClientFactory;


   private HttpClient TramsClient => _httpClientFactory.CreateTramsClient();

   public async Task<List<Trust>> SearchTrusts(string searchQuery = "",
      string outgoingTrustId = "")
   {
      string url = $"v4/trusts?groupName={searchQuery}&ukprn={searchQuery}&companiesHouseNumber={searchQuery}";
      using HttpResponseMessage response = await TramsClient.GetAsync(url);

      if (!response.IsSuccessStatusCode)
      {
         throw new TramsApiException(response);
      }

      string apiResponse = await response.Content.ReadAsStringAsync();
      PagedDataResponse<TrustDto> result = JsonConvert.DeserializeObject<PagedDataResponse<TrustDto>>(apiResponse);

      List<Trust> mappedResult = result.Data.Where(t => !string.IsNullOrEmpty(t.Ukprn) &&
                                                        t.Ukprn != outgoingTrustId)
         .Select(r => _trustMapper.Map(r)).ToList();

      return mappedResult;
   }

   public async Task<Trust> GetByUkprn(string ukprn)
   {
      string url = $"v4/trust/{ukprn}";
      using HttpResponseMessage response = await TramsClient.GetAsync(url);

      if (!response.IsSuccessStatusCode)
      {
         throw new TramsApiException(response);
      }

      string apiResponse = await response.Content.ReadAsStringAsync();
      TrustDto result = JsonConvert.DeserializeObject<TrustDto>(apiResponse);
      Trust trust = _trustMapper.Map(result);

      return trust;
   }
}