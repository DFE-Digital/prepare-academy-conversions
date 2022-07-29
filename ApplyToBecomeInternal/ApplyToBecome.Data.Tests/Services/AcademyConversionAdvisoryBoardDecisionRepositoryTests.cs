using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Tests.TestDoubles;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using MELT;
using ApplyToBecome.Data.Tests.AutoFixture;

namespace ApplyToBecome.Data.Tests.Services
{
	public class AcademyConversionAdvisoryBoardDecisionRepositoryTests
	{		
		[Theory, AutoMoqData]
		public async Task Should_send_post_to_academisation_api([Frozen] MockHttpMessageHandler mockHandler,
			Mock<ILogger<AcademyConversionAdvisoryBoardDecisionRepository>> logger,
			AdvisoryBoardDecision decision)
		{
			var payload = JsonSerializer.Serialize(decision);
			mockHandler.Expect("/conversion-project/advisory-board-decision")
				.Respond(HttpStatusCode.Created, "application/json", payload);

			var sut = new AcademyConversionAdvisoryBoardDecisionRepository(new MockHttpClientFactory(mockHandler), logger.Object);

			await sut.Create(decision);

			mockHandler.VerifyNoOutstandingExpectation();
		}

		[Theory, AutoMoqData]
		public void Should_throw_when_response_not_successful([Frozen] MockHttpMessageHandler mockHandler,
			Mock<ILogger<AcademyConversionAdvisoryBoardDecisionRepository>> logger,
			AdvisoryBoardDecision decision)
		{
			var responseCode = HttpStatusCode.BadRequest;
			mockHandler.Expect("/conversion-project/advisory-board-decision").Respond(responseCode);
			var sut = new AcademyConversionAdvisoryBoardDecisionRepository(new MockHttpClientFactory(mockHandler), logger.Object);

			sut.Invoking(async s => await s.Create(decision))
				.Should().Throw<Exception>().WithMessage($"Request to AcademisationApi failed | StatusCode - {responseCode}");
		}


		[Theory, AutoMoqData]
		public async Task Should_log_when_response_not_successful([Frozen] MockHttpMessageHandler mockHandler,			
			AdvisoryBoardDecision decision)
		{
			var responseCode = HttpStatusCode.BadRequest;
			var content = "{ \"error\": \"badrequest\" }";
			var logMock = TestLoggerFactory.Create();
			mockHandler.Expect("/conversion-project/advisory-board-decision").Respond(responseCode, "application/json", content);
			
			var sut = new AcademyConversionAdvisoryBoardDecisionRepository(new MockHttpClientFactory(mockHandler), logMock.CreateLogger<AcademyConversionAdvisoryBoardDecisionRepository>());

			await sut.Invoking(s => s.Create(decision)).Should().ThrowAsync<Exception>();

			logMock.Sink.LogEntries.Should()
				.ContainSingle(l => l.Message.Contains($"Request to AcademisationApi failed | StatusCode - {responseCode} | Content - {content}"));
		}
	}
}
