using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.PrepareTransfers.Data.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Dfe.Prepare.Data;

namespace Dfe.PrepareTransfers.Data.TRAMS
{
   public class TramsEstablishmentRepository : IAcademies
   {
      private readonly IMapper<EstablishmentDto, Academy> _academyMapper;
      private readonly IDistributedCache _distributedCache;

      public TramsEstablishmentRepository(
          IMapper<EstablishmentDto, Academy> academyMapper, IDistributedCache distributedCache)
      {
        
         _academyMapper = academyMapper;
         _distributedCache = distributedCache;
      }

      private readonly IDfeHttpClientFactory _httpClientFactory;


      private HttpClient TramsClient => _httpClientFactory.CreateTramsClient();

      public async Task<Academy> GetAcademyByUkprn(string ukprn)
      {
         var cacheKey = $"GetAcademyByUkprn_{ukprn}";
         var cachedString = await _distributedCache.GetStringAsync(cacheKey);
         if (!string.IsNullOrWhiteSpace(cachedString))
         {
            return JsonConvert.DeserializeObject<Academy>(cachedString);
         }

         using var response = await TramsClient.GetAsync($"v4/establishment/{ukprn}");

         if (!response.IsSuccessStatusCode)
         {
            throw new TramsApiException(response);
         }

         var apiResponse = await response.Content.ReadAsStringAsync();
         var result = JsonConvert.DeserializeObject<EstablishmentDto>(apiResponse);
         var mappedResult = _academyMapper.Map(result);
         var cacheOptions = new DistributedCacheEntryOptions
         {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
         };
         await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(mappedResult), cacheOptions);
         return mappedResult;
      }

      public async Task<List<Academy>> GetAcademiesByTrustUkprn(string ukprn)
      {
         var url = $"v4/establishments/trust?trustUkprn={ukprn}";
         using var response = await TramsClient.GetAsync(url);

         if (!response.IsSuccessStatusCode)
         {
            throw new TramsApiException(response);
         }

         var apiResponse = await response.Content.ReadAsStringAsync();
         var result = JsonConvert.DeserializeObject<List<EstablishmentDto>>(apiResponse);
         var academies = result.Select(x => _academyMapper.Map(x)).ToList();

         return academies;
      }
   }
}