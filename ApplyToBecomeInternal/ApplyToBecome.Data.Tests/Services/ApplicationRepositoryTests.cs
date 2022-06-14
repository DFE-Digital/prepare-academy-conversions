using Microsoft.Extensions.Logging;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Tests.TestDoubles;
using MELT;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Newtonsoft.Json;
using AutoFixture.Xunit2;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models.Application;
using FluentAssertions;
using System.Net;

namespace ApplyToBecome.Data.Tests.Services
{
	public class ApplicationRepositoryTests
	{
		private readonly MockHttpMessageHandler _mockHandler;
		private readonly ITestLoggerFactory _testLogger;
		private readonly ApplicationRepository _applicationRepository;

		public ApplicationRepositoryTests()
		{
			_mockHandler = new MockHttpMessageHandler();
			_testLogger = TestLoggerFactory.Create();
			_applicationRepository = new ApplicationRepository(
					new MockHttpClientFactory(_mockHandler),
					_testLogger .CreateLogger<ApplicationRepository>()
				);
		}

		[Theory]
		[AutoData]
		public async Task Should_get_application_data_by_application_reference(Application applicationMockData)
		{
			var responseObject = new ApiV2Wrapper<Application>
			{
				Data = applicationMockData
			};

			_mockHandler.Expect($"/v2/apply-to-become/application/{applicationMockData.ApplicationId}")
				.Respond("application/json", JsonConvert.SerializeObject(responseObject));

			var applicationResponse = await _applicationRepository.GetApplicationByReference(applicationMockData.ApplicationId);

			applicationResponse.Success.Should().BeTrue();
			applicationResponse.Body.Should().BeEquivalentTo(applicationMockData);
		}

		[Fact]
		public async Task Should_return_not_success_when_application_not_found()
		{
			// API responses might need more than just Success and !Success 
			var applicationReference = "123a"; 
			_mockHandler.Expect($"/v2/apply-to-become/application/{applicationReference}")
				.Respond(HttpStatusCode.NotFound);

			var applicationResponse = await _applicationRepository.GetApplicationByReference(applicationReference);

			applicationResponse.Success.Should().BeFalse();
		}
	}
}
