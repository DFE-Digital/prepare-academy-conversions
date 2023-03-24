using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using FluentAssertions;
using MELT;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class HttpClientServiceTests
{
   private readonly string _apiSaveAddress = "https://localhost/api/save";

   [Theory]
   [AutoMoqData]
   public async Task Should_send_post_to_api([Frozen] MockHttpMessageHandler mockHandler,
                                             Mock<ILogger<HttpClientService>> logger,
                                             AdvisoryBoardDecision decision)
   {
      string payload = JsonSerializer.Serialize(decision);
      mockHandler.Expect(HttpMethod.Post, _apiSaveAddress)
         .Respond(HttpStatusCode.Created, "application/json", payload);

      HttpClientService sut = new(logger.Object);

      ApiResponse<AdvisoryBoardDecision> result = await sut.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

      mockHandler.VerifyNoOutstandingExpectation();
      result.Body.Should().BeEquivalentTo(decision);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_return_null_body_when_response_not_successful_on_post([Frozen] MockHttpMessageHandler mockHandler,
                                                                                  Mock<ILogger<HttpClientService>> logger,
                                                                                  AdvisoryBoardDecision decision)
   {
      HttpStatusCode responseCode = HttpStatusCode.BadRequest;
      mockHandler.Expect(_apiSaveAddress).Respond(responseCode);

      HttpClientService sut = new(logger.Object);

      ApiResponse<AdvisoryBoardDecision> result = await sut.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

      result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Success.Should().BeFalse();
      result.Body.Should().BeNull();
   }


   [Theory]
   [AutoMoqData]
   public async Task Should_log_when_response_not_successful_on_post([Frozen] MockHttpMessageHandler mockHandler,
                                                                     AdvisoryBoardDecision decision)
   {
      HttpStatusCode responseCode = HttpStatusCode.BadRequest;
      string content = "{ \"error\": \"badrequest\" }";
      ITestLoggerFactory logMock = TestLoggerFactory.Create();
      mockHandler.Expect(_apiSaveAddress).Respond(responseCode, "application/json", content);

      HttpClientService sut = new(logMock.CreateLogger<HttpClientService>());

      await sut.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

      logMock.Sink.LogEntries.Should()
         .ContainSingle(l => l.Message.Contains($"Request to Api failed | StatusCode - {responseCode} | Content - {content}"));
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_send_get_to_api([Frozen] MockHttpMessageHandler mockHandler,
                                            Mock<ILogger<HttpClientService>> logger,
                                            AdvisoryBoardDecision decision)
   {
      string payload = JsonSerializer.Serialize(decision);
      mockHandler.Expect(HttpMethod.Get, _apiSaveAddress)
         .Respond(HttpStatusCode.Created, "application/json", payload);

      HttpClientService sut = new(logger.Object);

      ApiResponse<AdvisoryBoardDecision> result = await sut.Get<AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress);

      mockHandler.VerifyNoOutstandingExpectation();
      result.Body.Should().BeEquivalentTo(decision);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_return_null_body_when_response_not_successful_on_get([Frozen] MockHttpMessageHandler mockHandler,
                                                                                 Mock<ILogger<HttpClientService>> logger)
   {
      HttpStatusCode responseCode = HttpStatusCode.BadRequest;
      mockHandler.Expect(_apiSaveAddress).Respond(responseCode);

      HttpClientService sut = new(logger.Object);

      ApiResponse<AdvisoryBoardDecision> result = await sut.Get<AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress);

      result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Success.Should().BeFalse();
      result.Body.Should().BeNull();
   }


   [Theory]
   [AutoMoqData]
   public async Task Should_log_when_response_not_successful_on_get([Frozen] MockHttpMessageHandler mockHandler)
   {
      HttpStatusCode responseCode = HttpStatusCode.BadRequest;
      string content = "{ \"error\": \"badrequest\" }";
      ITestLoggerFactory logMock = TestLoggerFactory.Create();
      mockHandler.Expect(_apiSaveAddress).Respond(responseCode, "application/json", content);

      HttpClientService sut = new(logMock.CreateLogger<HttpClientService>());

      await sut.Get<AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress);

      logMock.Sink.LogEntries.Should()
         .ContainSingle(l => l.Message.Contains($"Request to Api failed | StatusCode - {responseCode} | Content - {content}"));
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_send_put_to_api([Frozen] MockHttpMessageHandler mockHandler,
                                            Mock<ILogger<HttpClientService>> logger,
                                            AdvisoryBoardDecision decision)
   {
      string payload = JsonSerializer.Serialize(decision);
      mockHandler.Expect(HttpMethod.Put, _apiSaveAddress)
         .Respond(HttpStatusCode.Created, "application/json", payload);

      HttpClientService sut = new(logger.Object);

      ApiResponse<AdvisoryBoardDecision> result = await sut.Put<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

      mockHandler.VerifyNoOutstandingExpectation();
      result.Body.Should().BeEquivalentTo(decision);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_return_null_body_when_response_not_successful_on_put([Frozen] MockHttpMessageHandler mockHandler,
                                                                                 Mock<ILogger<HttpClientService>> logger,
                                                                                 AdvisoryBoardDecision decision)
   {
      HttpStatusCode responseCode = HttpStatusCode.BadRequest;
      mockHandler.Expect(_apiSaveAddress).Respond(responseCode);

      HttpClientService sut = new(logger.Object);

      ApiResponse<AdvisoryBoardDecision> result = await sut.Put<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

      result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.Success.Should().BeFalse();
      result.Body.Should().BeNull();
   }


   [Theory]
   [AutoMoqData]
   public async Task Should_log_when_response_not_successful_on_put([Frozen] MockHttpMessageHandler mockHandler,
                                                                    AdvisoryBoardDecision decision)
   {
      HttpStatusCode responseCode = HttpStatusCode.BadRequest;
      string content = "{ \"error\": \"badrequest\" }";
      ITestLoggerFactory logMock = TestLoggerFactory.Create();
      mockHandler.Expect(_apiSaveAddress).Respond(responseCode, "application/json", content);

      HttpClientService sut = new(logMock.CreateLogger<HttpClientService>());

      await sut.Put<AdvisoryBoardDecision, AdvisoryBoardDecision>(mockHandler.ToHttpClient(), _apiSaveAddress, decision);

      logMock.Sink.LogEntries.Should()
         .ContainSingle(l => l.Message.Contains($"Request to Api failed | StatusCode - {responseCode} | Content - {content}"));
   }
}
