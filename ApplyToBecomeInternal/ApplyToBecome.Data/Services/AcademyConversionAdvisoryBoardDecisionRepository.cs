using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class AcademyConversionAdvisoryBoardDecisionRepository : IAcademyConversionAdvisoryBoardDecisionRepository
	{
		private readonly HttpClient _client;
		private readonly ILogger<AcademyConversionAdvisoryBoardDecisionRepository> _logger;

		public AcademyConversionAdvisoryBoardDecisionRepository(IHttpClientFactory httpClientFactory, ILogger<AcademyConversionAdvisoryBoardDecisionRepository> logger)
		{
			_client = httpClientFactory.CreateClient("AcademisationClient");
			_logger = logger;
		}

		public async Task Create(AdvisoryBoardDecision decision)
		{			
			var requestPayload = JsonContent.Create(decision);
			var result = await _client.PostAsync("/api/conversion-project/advisory-board-decision", requestPayload);

			if (!result.IsSuccessStatusCode)
			{
				var content = "";
				if (result.Content != null) content = await result.Content.ReadAsStringAsync();

				_logger.LogError($"Request to AcademisationApi failed | StatusCode - {result.StatusCode} | Content - {content}");

				throw new Exception($"Request to AcademisationApi failed | StatusCode - {result.StatusCode}");
			}
		}
	}
}
