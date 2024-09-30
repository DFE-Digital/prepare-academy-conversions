using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dfe.Academies.Contracts.V4;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareTransfers.Data.Models;
using Newtonsoft.Json;
using Dfe.Prepare.Data;

namespace Dfe.PrepareTransfers.Data.TRAMS
{
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
            var url = $"v4/trusts?groupName={searchQuery}&ukprn={searchQuery}&companiesHouseNumber={searchQuery}";
            using var response = await TramsClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new TramsApiException(response);
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedDataResponse<TrustDto>>(apiResponse);

            var mappedResult = result.Data.Where(t => !string.IsNullOrEmpty(t.Ukprn) &&
                                                 t.Ukprn != outgoingTrustId)
                .Select(r => _trustMapper.Map(r)).ToList();

            return mappedResult;
        }

        public async Task<Trust> GetByUkprn(string ukprn)
        {
            var url = $"v4/trust/{ukprn}";
            using var response = await TramsClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new TramsApiException(response);
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TrustDto>(apiResponse);
            var trust = _trustMapper.Map(result);

            return trust;
        }
    }
}