using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareTransfers.Data.Models;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Data.TRAMS;

public class TramsEstablishmentRepository : IAcademies
{
   private readonly IMapper<EstablishmentDto, Academy> _academyMapper;
   private readonly IDistributedCache _distributedCache;

   public TramsEstablishmentRepository(IDfeHttpClientFactory httpClientFactory,
      IMapper<EstablishmentDto, Academy> academyMapper, IDistributedCache distributedCache)
   {
      _httpClientFactory = httpClientFactory;
      _academyMapper = academyMapper;
      _distributedCache = distributedCache;
   }

   private readonly IDfeHttpClientFactory _httpClientFactory;


   private HttpClient TramsClient => _httpClientFactory.CreateTramsClient();

   public async Task<Academy> GetAcademyByUkprn(string ukprn)
   {
      string cacheKey = $"GetAcademyByUkprn_{ukprn}";
      string cachedString = await _distributedCache.GetStringAsync(cacheKey);
      if (!string.IsNullOrWhiteSpace(cachedString))
      {
         return JsonConvert.DeserializeObject<Academy>(cachedString);
      }

      using HttpResponseMessage response = await TramsClient.GetAsync($"v4/establishment/{ukprn}");

      if (!response.IsSuccessStatusCode)
      {
         throw new TramsApiException(response);
      }

      string apiResponse = await response.Content.ReadAsStringAsync();
      EstablishmentDto result = JsonConvert.DeserializeObject<EstablishmentDto>(apiResponse);
      Academy mappedResult = _academyMapper.Map(result);
      DistributedCacheEntryOptions cacheOptions = new() { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) };
      await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(mappedResult), cacheOptions);
      return mappedResult;
   }

   public async Task<List<Academy>> GetAcademiesByTrustUkprn(string ukprn)
   {
      string url = $"v4/establishments/trust?trustUkprn={ukprn}";
      using HttpResponseMessage response = await TramsClient.GetAsync(url);

      if (!response.IsSuccessStatusCode)
      {
         throw new TramsApiException(response);
      }

      string apiResponse = await response.Content.ReadAsStringAsync();
      List<EstablishmentDto> result = JsonConvert.DeserializeObject<List<EstablishmentDto>>(apiResponse);
      List<Academy> academies = result.Select(x => _academyMapper.Map(x)).ToList();

      return academies;
   }
}