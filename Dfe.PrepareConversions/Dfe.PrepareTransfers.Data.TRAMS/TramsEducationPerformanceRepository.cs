using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Data.TRAMS.Models.EducationPerformance;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Dfe.PrepareTransfers.Data.TRAMS
{
   public class TramsEducationPerformanceRepository : IEducationPerformance
   {
      private readonly IMapper<TramsEducationPerformance, EducationPerformance> _educationPerformanceMapper;
      private readonly IDistributedCache _distributedCache;

      public TramsEducationPerformanceRepository(IDfeHttpClientFactory httpClientFactory, IMapper<TramsEducationPerformance, EducationPerformance> educationPerformanceMapper, IDistributedCache distributedCache)
      {
         _httpClientFactory = httpClientFactory; ;
         _educationPerformanceMapper = educationPerformanceMapper;
         _distributedCache = distributedCache;
      }

      private readonly IDfeHttpClientFactory _httpClientFactory;
      private HttpClient TramsClient => _httpClientFactory.CreateTramsClient();

      public async Task<RepositoryResult<EducationPerformance>> GetByAcademyUrn(string urn)
      {
         var cacheKey = $"GetPerformanceByAcademy_{urn}";
         var cachedString = await _distributedCache.GetStringAsync(cacheKey);
         //Check for information in cache
         if (!string.IsNullOrWhiteSpace(cachedString))
         {
            return JsonConvert.DeserializeObject<RepositoryResult<EducationPerformance>>(cachedString);
         }

         using var response = await TramsClient.GetAsync($"educationPerformance/{urn}");

         if (response.IsSuccessStatusCode)
         {
            var apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TramsEducationPerformance>(apiResponse);
            var mappedResult = new RepositoryResult<EducationPerformance>
            {
               Result = _educationPerformanceMapper.Map(result)
            };

            var cacheOptions = new DistributedCacheEntryOptions
            {
               AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
            await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(mappedResult), cacheOptions);
            return mappedResult;
         }

         if (response.StatusCode == HttpStatusCode.NotFound)
         {
            return new RepositoryResult<EducationPerformance>()
            {
               Result = new EducationPerformance()
            };
         }

         throw new TramsApiException(response);
      }
   }
}