using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.TestDoubles;
using FluentAssertions;
using MELT;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class SchoolPerformanceServiceTests
{
   private readonly MockHttpMessageHandler _mockHandler;
   private readonly SchoolPerformanceService _schoolPerformanceService;
   private readonly ITestLoggerFactory _testLogger;

   public SchoolPerformanceServiceTests()
   {
      _mockHandler = new MockHttpMessageHandler();
      _testLogger = TestLoggerFactory.Create();
      _schoolPerformanceService = new SchoolPerformanceService(
         new EstablishmentService(
            new MockHttpClientFactory(_mockHandler),
            _testLogger.CreateLogger<EstablishmentService>(),
            Mock.Of<IHttpClientService>()
         )
      );
   }

   [Theory]
   [AutoData]
   public async Task Should_get_school_performance_by_urn(AcademyConversionProject project, EstablishmentMockData establishmentMockData)
   {
      establishmentMockData.MisEstablishment.InspectionEndDate = "15/01/2020";
      establishmentMockData.MisEstablishment.DateOfLatestSection8Inspection = "15/01/2020";
      _mockHandler
         .Expect($"/establishment/urn/{project.Urn}")
         .Respond("application/json", JsonConvert.SerializeObject(establishmentMockData));

      SchoolPerformance schoolPerformance = await _schoolPerformanceService.GetSchoolPerformanceByUrn(project.Urn.ToString());

      schoolPerformance.Should().BeEquivalentTo(establishmentMockData.MisEstablishment, options =>
      {
         options.Excluding(response => response.InspectionEndDate);
         options.Excluding(response => response.DateOfLatestSection8Inspection);
         options.Excluding(response => response.Weblink);
         return options;
      });
      schoolPerformance.InspectionEndDate.Should().Be(DateTime.Parse("15/01/2020", CultureInfo.CreateSpecificCulture("en-GB")));
      schoolPerformance.DateOfLatestSection8Inspection.Should().Be(DateTime.Parse("15/01/2020", CultureInfo.CreateSpecificCulture("en-GB")));
      schoolPerformance.OfstedReport.Should().Be(establishmentMockData.MisEstablishment.Weblink);
   }

   [Theory]
   [AutoData]
   public async Task Should_log_warning_when_school_performance_not_found(AcademyConversionProject project)
   {
      _mockHandler
         .Expect($"/establishment/urn/{project.Urn}")
         .Respond(HttpStatusCode.NotFound);

      await _schoolPerformanceService.GetSchoolPerformanceByUrn(project.Urn.ToString());

      _testLogger.Sink.LogEntries
         .Should()
         .ContainSingle(log => log.Message.Equals($"Unable to get establishment data for establishment with URN: {project.Urn}"));
   }

   public class EstablishmentMockData
   {
      public MisEstablishmentResponse MisEstablishment { get; set; }
   }
}
