﻿using Dfe.PrepareConversions.Data.Exceptions;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services
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

			if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
		}

		public async Task Update(AdvisoryBoardDecision decision)
		{
			var result = await _httpClientHelper
				.Put<AdvisoryBoardDecision, AdvisoryBoardDecision>("/conversion-project/advisory-board-decision", decision);

			if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");
		}

		public async Task<ApiResponse<AdvisoryBoardDecision>> Get(int id)
		{
			return await _httpClientHelper
				.Get<AdvisoryBoardDecision>($"/conversion-project/advisory-board-decision/{id}");
		}
	}
}