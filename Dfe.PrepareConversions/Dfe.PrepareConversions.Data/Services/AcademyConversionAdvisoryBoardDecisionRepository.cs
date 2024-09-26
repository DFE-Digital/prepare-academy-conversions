using Dfe.Prepare.Data;
using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class AcademyConversionAdvisoryBoardDecisionRepository : IAcademyConversionAdvisoryBoardDecisionRepository
{
   private readonly HttpClient _httpClient;
   private readonly IHttpClientService _httpClientHelper;

   public AcademyConversionAdvisoryBoardDecisionRepository(IDfeHttpClientFactory httpClientFactory, IHttpClientService httpClientHelper)
   {
      _httpClient = httpClientFactory.CreateAcademisationClient();
      _httpClientHelper = httpClientHelper;
   }

   public async Task Create(AdvisoryBoardDecision decision)
   {
      ApiResponse<AdvisoryBoardDecision> result = await _httpClientHelper
         .Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(_httpClient, "/conversion-project/advisory-board-decision", decision);

      if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task Update(AdvisoryBoardDecision decision)
   {
      ApiResponse<AdvisoryBoardDecision> result = await _httpClientHelper
         .Put<AdvisoryBoardDecision, AdvisoryBoardDecision>(_httpClient, "/conversion-project/advisory-board-decision", decision);

      if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
   }

   public async Task<ApiResponse<AdvisoryBoardDecision>> Get(int id)
   {
      return await _httpClientHelper
         .Get<AdvisoryBoardDecision>(_httpClient, $"/conversion-project/advisory-board-decision/{id}");
   }
}
