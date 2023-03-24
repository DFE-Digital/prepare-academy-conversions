using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class AcademyConversionAdvisoryBoardDecisionRepositoryTests
{
   [Theory]
   [AutoMoqData]
   public async Task Should_send_post_to_academisation_api([Frozen] Mock<IHttpClientService> httpHelper,
                                                           AcademyConversionAdvisoryBoardDecisionRepository sut,
                                                           AdvisoryBoardDecision decision)
   {
      ApiResponse<AdvisoryBoardDecision> apiResponse = new(HttpStatusCode.Created, decision);
      httpHelper.Setup(m =>
            m.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(It.IsAny<HttpClient>(), "/conversion-project/advisory-board-decision", decision))
         .ReturnsAsync(apiResponse);

      await sut.Create(decision);

      httpHelper.Verify(m => m
         .Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(It.IsAny<HttpClient>(), "/conversion-project/advisory-board-decision", decision), Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_throw_when_response_not_successful([Frozen] Mock<IHttpClientService> httpHelper,
                                                               AcademyConversionAdvisoryBoardDecisionRepository sut,
                                                               AdvisoryBoardDecision decision)
   {
      HttpStatusCode responseCode = HttpStatusCode.BadRequest;
      httpHelper.Setup(m =>
            m.Post<AdvisoryBoardDecision, AdvisoryBoardDecision>(It.IsAny<HttpClient>(), "/conversion-project/advisory-board-decision", decision))
         .ReturnsAsync(new ApiResponse<AdvisoryBoardDecision>(responseCode, null));

      await sut.Invoking(async s => await s.Create(decision))
         .Should().ThrowAsync<Exception>().WithMessage($"Request to Api failed | StatusCode - {responseCode}");
   }
}
