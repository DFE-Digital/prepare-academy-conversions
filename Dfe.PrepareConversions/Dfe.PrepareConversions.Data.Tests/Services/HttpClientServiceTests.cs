using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using MELT;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using System.Net.Http;

namespace Dfe.PrepareConversions.Data.Tests.Services
{
	public class HttpClientServiceTests
	{
      private readonly string _apiSaveAddress = "https://localhost/api/save";

      [Theory, AutoMoqData]
		public async Task Should_send_post_to_api([Frozen] MockHttpMessageHandler mockHandler,
			Mock<ILogger<HttpClientService>> logger,
			AdvisoryBoardDecision decision)
		{
         var payload = JsonSerializer.Serialize(decision);
			mockHandler.Expect(HttpMethod.Post, _apiSaveAddress)
				.Respond(HttpStatusCode.Created, "application/json", payload);

			var sut = new HttpClientService(logger.Object);

			var result = await sut.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

			mockHandler.VerifyNoOutstandingExpectation();
			result.Body.Should().BeEquivalentTo(decision);
		}

		[Theory, AutoMoqData]
		public async Task Should_return_null_body_when_response_not_successful_on_post([Frozen] MockHttpMessageHandler mockHandler,
			Mock<ILogger<HttpClientService>> logger,
			AdvisoryBoardDecision decision)
		{
         var responseCode = HttpStatusCode.BadRequest;
			mockHandler.Expect(_apiSaveAddress).Respond(responseCode);

			var sut = new HttpClientService(logger.Object);

			var result = await sut.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			result.Success.Should().BeFalse();
			result.Body.Should().BeNull();
		}


		[Theory, AutoMoqData]
		public async Task Should_log_when_response_not_successful_on_post([Frozen] MockHttpMessageHandler mockHandler,
			AdvisoryBoardDecision decision)
		{
			var responseCode = HttpStatusCode.BadRequest;
			var content = "{ \"error\": \"badrequest\" }";
         var logMock = TestLoggerFactory.Create();
			mockHandler.Expect(_apiSaveAddress).Respond(responseCode, "application/json", content);

			var sut = new HttpClientService(logMock.CreateLogger<HttpClientService>());

			await sut.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

			logMock.Sink.LogEntries.Should()
				.ContainSingle(l => l.Message.Contains($"Request to Api failed | StatusCode - {responseCode} | Content - {content}"));
		}

		[Theory, AutoMoqData]
		public async Task Should_send_get_to_api([Frozen] MockHttpMessageHandler mockHandler,
			Mock<ILogger<HttpClientService>> logger,
			AdvisoryBoardDecision decision)
		{
         var payload = JsonSerializer.Serialize(decision);
			mockHandler.Expect(HttpMethod.Get, _apiSaveAddress)
				.Respond(HttpStatusCode.Created, "application/json", payload);

			var sut = new HttpClientService(logger.Object);

			var result = await sut.Get<AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress);

			mockHandler.VerifyNoOutstandingExpectation();
			result.Body.Should().BeEquivalentTo(decision);
		}

		[Theory, AutoMoqData]
		public async Task Should_return_null_body_when_response_not_successful_on_get([Frozen] MockHttpMessageHandler mockHandler,
			Mock<ILogger<HttpClientService>> logger)
		{
         var responseCode = HttpStatusCode.BadRequest;
			mockHandler.Expect(_apiSaveAddress).Respond(responseCode);

			var sut = new HttpClientService(logger.Object);

			var result = await sut.Get<AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress);

			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			result.Success.Should().BeFalse();
			result.Body.Should().BeNull();
		}


		[Theory, AutoMoqData]
		public async Task Should_log_when_response_not_successful_on_get([Frozen] MockHttpMessageHandler mockHandler)
		{
			var responseCode = HttpStatusCode.BadRequest;
			var content = "{ \"error\": \"badrequest\" }";
         var logMock = TestLoggerFactory.Create();
			mockHandler.Expect(_apiSaveAddress).Respond(responseCode, "application/json", content);

			var sut = new HttpClientService(logMock.CreateLogger<HttpClientService>());

			await sut.Get<AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress);

			logMock.Sink.LogEntries.Should()
				.ContainSingle(l => l.Message.Contains($"Request to Api failed | StatusCode - {responseCode} | Content - {content}"));
		}

		[Theory, AutoMoqData]
		public async Task Should_send_put_to_api([Frozen] MockHttpMessageHandler mockHandler,
			Mock<ILogger<HttpClientService>> logger,
			AdvisoryBoardDecision decision)
		{
         var payload = JsonSerializer.Serialize(decision);
			mockHandler.Expect(HttpMethod.Put, _apiSaveAddress)
				.Respond(HttpStatusCode.Created, "application/json", payload);

			var sut = new HttpClientService(logger.Object);

			var result = await sut.Put<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

			mockHandler.VerifyNoOutstandingExpectation();
			result.Body.Should().BeEquivalentTo(decision);
		}

		[Theory, AutoMoqData]
		public async Task Should_return_null_body_when_response_not_successful_on_put([Frozen] MockHttpMessageHandler mockHandler,
			Mock<ILogger<HttpClientService>> logger,
			AdvisoryBoardDecision decision)
		{
         var responseCode = HttpStatusCode.BadRequest;
			mockHandler.Expect(_apiSaveAddress).Respond(responseCode);

			var sut = new HttpClientService(logger.Object);

			var result = await sut.Put<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			result.Success.Should().BeFalse();
			result.Body.Should().BeNull();
		}


		[Theory, AutoMoqData]
		public async Task Should_log_when_response_not_successful_on_put([Frozen] MockHttpMessageHandler mockHandler,
			AdvisoryBoardDecision decision)
		{
			var responseCode = HttpStatusCode.BadRequest;
			var content = "{ \"error\": \"badrequest\" }";
         var logMock = TestLoggerFactory.Create();
			mockHandler.Expect(_apiSaveAddress).Respond(responseCode, "application/json", content);

			var sut = new HttpClientService(logMock.CreateLogger<HttpClientService>());

			await sut.Put<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

			logMock.Sink.LogEntries.Should()
				.ContainSingle(l => l.Message.Contains($"Request to Api failed | StatusCode - {responseCode} | Content - {content}"));
		}
	}
}
