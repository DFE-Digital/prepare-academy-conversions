using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareTransfers.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareTransfers.Data.Services.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dfe.Prepare.Data;

namespace Dfe.PrepareTransfers.Data.TRAMS;

public class AcademyTransfersAdvisoryBoardDecisionRepository : IAcademyTransfersAdvisoryBoardDecisionRepository
{

   public AcademyTransfersAdvisoryBoardDecisionRepository(IDfeHttpClientFactory httpClientFactory)
   {
      _httpClientFactory = httpClientFactory;
   }

   private readonly IDfeHttpClientFactory _httpClientFactory;


   private HttpClient AcademisationClient => _httpClientFactory.CreateAcademisationClient();

   public async Task Create(AdvisoryBoardDecision decision)
   {
      var content = new StringContent(JsonConvert.SerializeObject(decision), Encoding.Default,
              "application/json");

      HttpResponseMessage response = await AcademisationClient.PostAsync("/conversion-project/advisory-board-decision", content);

      if (!response.IsSuccessStatusCode)
      {
         throw new TramsApiException(response);
      }
   }

   public async Task Update(AdvisoryBoardDecision decision)
   {
      var content = new StringContent(JsonConvert.SerializeObject(decision), Encoding.Default,
              "application/json");

      HttpResponseMessage response = await AcademisationClient.PutAsync($"/conversion-project/advisory-board-decision", content);
      if (!response.IsSuccessStatusCode)
      {
         throw new TramsApiException(response);
      }
   }

   public async Task<RepositoryResult<AdvisoryBoardDecision>> Get(int projectId)
   {
      HttpResponseMessage response = await AcademisationClient.GetAsync($"/transfer-project/advisory-board-decision/{projectId}");

      if (!response.IsSuccessStatusCode)
      {
         if (response.StatusCode == HttpStatusCode.NotFound)
         {
            return new RepositoryResult<AdvisoryBoardDecision>
            {
               Result = null
            };

            throw new TramsApiException(response);
         }
      }

      var apiResponse = await response.Content.ReadAsStringAsync();
      var decision = JsonConvert.DeserializeObject<AdvisoryBoardDecision>(apiResponse);

      return new RepositoryResult<AdvisoryBoardDecision>
      {
         Result = decision
      };
   }
}
