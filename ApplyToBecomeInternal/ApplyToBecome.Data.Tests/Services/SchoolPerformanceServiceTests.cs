using Microsoft.Extensions.Logging;
using MELT;
using RichardSzalay.MockHttp;
using Xunit;
using ApplyToBecome.Data.Models;
using System;
using AutoFixture.Xunit2;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Tests.TestDoubles;
using Newtonsoft.Json;

namespace ApplyToBecome.Data.Tests.Services
{
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
					_testLogger.CreateLogger<SchoolPerformanceService>()
				)
			);
		}

		[Theory]
		[AutoData]
		public async Task Should_get_school_performance_by_urn(AcademyConversionProject project, EstablishmentMockData establishmentMockData)
		{
			establishmentMockData.misEstablishment.OfstedLastInspection = establishmentMockData.misEstablishment.OfstedLastInspection.Value.Date;
			establishmentMockData.ofstedLastInspection = establishmentMockData.misEstablishment.OfstedLastInspection.Value;

			_mockHandler
				.Expect($"/establishment/urn/{project.Urn}")
				.Respond("application/json", JsonConvert.SerializeObject(establishmentMockData));

			var schoolPerformance = await _schoolPerformanceService.GetSchoolPerformanceByUrn(project.Urn.ToString());

			schoolPerformance.Should().BeEquivalentTo(establishmentMockData.misEstablishment);
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
				.ContainSingle(log => log.Message.Equals($"Unable to get school performance information for establishment with URN: {project.Urn}"));
		}

		public class EstablishmentMockData
		{
			public SchoolPerformance misEstablishment { get; set; }

			public DateTime ofstedLastInspection { get; set; }
		}
	}
}
