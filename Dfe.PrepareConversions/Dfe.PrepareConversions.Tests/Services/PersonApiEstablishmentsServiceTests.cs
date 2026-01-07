using Dfe.PrepareConversions.Data.Services.Person;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Services
{
    public class PersonApiEstablishmentsServiceTests
    {
      private readonly Mock<IEstablishmentsClient> _mockApiClient = new();
      private readonly Mock<ILogger<PersonApiEstablishmentsService>> _mockLogger = new();
      private readonly PersonApiEstablishmentsService _service;

      public PersonApiEstablishmentsServiceTests()
      {
         _service = new PersonApiEstablishmentsService(_mockApiClient.Object, _mockLogger.Object);
      }

      [Fact]
      public async Task GetMemberOfParliamentBySchoolUrnAsync_ReturnsSuccess_WhenApiReturnsResult()
      {
         var mp = new MemberOfParliament();
         _mockApiClient
             .Setup(x => x.GetMemberOfParliamentBySchoolUrnAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(mp);

         var result = await _service.GetMemberOfParliamentBySchoolUrnAsync(123);

         Assert.True(result.IsSuccess);
         Assert.Equal(mp, result.Value);
      }

      [Fact]
      public async Task GetMemberOfParliamentBySchoolUrnAsync_ReturnsFailure_WhenPersonsApiExceptionThrown()
      {
         // public PersonsApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
         _mockApiClient
             .Setup(x => x.GetMemberOfParliamentBySchoolUrnAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
             .ThrowsAsync(new PersonsApiException("API error", 500, null, null, null));

         var result = await _service.GetMemberOfParliamentBySchoolUrnAsync(123);

         Assert.False(result.IsSuccess);
         Assert.Contains("An error occurred with the Persons API client", result.Error);
      }

      [Fact]
      public async Task GetMemberOfParliamentBySchoolUrnAsync_ReturnsFailure_WhenAggregateExceptionThrown()
      {
         _mockApiClient
             .Setup(x => x.GetMemberOfParliamentBySchoolUrnAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
             .ThrowsAsync(new AggregateException("Aggregate error"));

         var result = await _service.GetMemberOfParliamentBySchoolUrnAsync(123);

         Assert.False(result.IsSuccess);
         Assert.Equal("An error occurred.", result.Error);
      }

      [Fact]
      public async Task GetMemberOfParliamentBySchoolUrnAsync_ReturnsFailure_WhenGeneralExceptionThrown()
      {
         _mockApiClient
             .Setup(x => x.GetMemberOfParliamentBySchoolUrnAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
             .ThrowsAsync(new Exception("General error"));

         var result = await _service.GetMemberOfParliamentBySchoolUrnAsync(123);

         Assert.False(result.IsSuccess);
         Assert.Contains("unexpected error", result.Error, StringComparison.OrdinalIgnoreCase);
      }
   }
}
