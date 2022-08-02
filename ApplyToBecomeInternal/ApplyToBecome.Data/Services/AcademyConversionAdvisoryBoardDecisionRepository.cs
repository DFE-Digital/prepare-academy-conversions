using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class AcademyConversionAdvisoryBoardDecisionRepository : IAcademyConversionAdvisoryBoardDecisionRepository
	{
		private readonly IHttpClientService _httpClientHelper;

		public AcademyConversionAdvisoryBoardDecisionRepository(IHttpClientService httpClientHelper)
		{	
			_httpClientHelper = httpClientHelper;
		}

		public async Task Create(AdvisoryBoardDecision decision)
		{
			var result = await _httpClientHelper
				.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>("/conversion-project/advisory-board-decision", decision);

			if (!result.Success) throw new Exception($"Request to Api failed | StatusCode - {result.StatusCode}");
		}

		public async Task<ApiResponse<AdvisoryBoardDecision>> Get(int id)
		{
			return new ApiResponse<AdvisoryBoardDecision>(HttpStatusCode.OK, 
				new AdvisoryBoardDecision 
				{ 
					AdvisoryBoardDecisionDate = DateTime.Now,
					ApprovedConditionsDetails = "All bills have been paid",
					DecisionMadeBy = DecisionMadeBy.RegionalDirectorForRegion,
					ApprovedConditionsSet = true,
					Decision = AdvisoryBoardDecisions.Approved
				});

			//return await _httpClientHelper
			//	.Get<AdvisoryBoardDecision>($"/conversion-project/advisory-board-decision?id={id}");
		}
	}
}
