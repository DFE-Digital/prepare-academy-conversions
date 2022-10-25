using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecome.Data.Tests.TestDoubles;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecome.Data.Tests.Services
{
	public class AcademyConversionProjectRepositoryTests
	{
		private AcademyConversionProjectRepository _subject;
		private readonly MockHttpMessageHandler _messageHandler;

		public AcademyConversionProjectRepositoryTests()
		{
			_messageHandler = new MockHttpMessageHandler();
		}

		[Fact]
		public void GivenDeliveryOfficers_GetsRelevantProjects()
		{
			var path = "http://localhost/v2/conversion-projects?page=1&count=50&states=&title=&deliveryOfficers=John+Smith&deliveryOfficers=Jane+Doe";
			var project = new List<AcademyConversionProject>() { new AcademyConversionProject() { AssignedUser = new User("1", "example@email.com", "John Smith") } };
			var projects = new ApiV2Wrapper<IEnumerable<AcademyConversionProject>>() { Data = project };
			var payload = JsonSerializer.Serialize(projects);
			_messageHandler.Expect(HttpMethod.Get, path)
				.Respond(HttpStatusCode.OK, "application/json", payload);

			_subject = new AcademyConversionProjectRepository(new MockHttpClientFactory(_messageHandler));
			var deliveryOfficers = new List<string>() { "John Smith", "Jane Doe" };

			Assert.Equal(project.First().AssignedUser.FullName, _subject.GetAllProjects(1, 50, deliveryOfficers, string.Empty, string.Empty).Result.Body.Data.First().AssignedUser.FullName);
		}
		[Fact]
		public void GivenDeliveryOfficers_GetsRelevantProjects_WhenMultiple()
		{
			var path = "http://localhost/v2/conversion-projects?page=1&count=50&states=&title=&deliveryOfficers=John+Smith";
			var project = new List<AcademyConversionProject>() {
				new AcademyConversionProject() {
				AssignedUser = new User("1", "example@email.com", "John Smith")
				},
				new AcademyConversionProject() {
					AssignedUser = new User("2", "example@email.com", "John Smith")
				}
			};
			var projects = new ApiV2Wrapper<IEnumerable<AcademyConversionProject>>() { Data = project };
			var payload = JsonSerializer.Serialize(projects);
			_messageHandler.Expect(HttpMethod.Get, path)
				.Respond(HttpStatusCode.OK, "application/json", payload);

			_subject = new AcademyConversionProjectRepository(new MockHttpClientFactory(_messageHandler));
			var deliveryOfficers = new List<string>() { "John Smith" };

			Assert.Equal(2, _subject.GetAllProjects(1, 50, deliveryOfficers, string.Empty, string.Empty).Result.Body.Data.Count());
		}

		[Fact]
		public void GivenDeliveryOfficers_GetsNoProjects_WhenDeliveryOfficerHasNoneAssigned()
		{
			var path = "http://localhost/v2/conversion-projects?page=1&count=50&states=&title=%26deliveryOfficers%3dJohn+Smith";
			var project = new List<AcademyConversionProject>() { };
			var projects = new ApiV2Wrapper<IEnumerable<AcademyConversionProject>>() { Data = project };
			var payload = JsonSerializer.Serialize(projects);
			_messageHandler.Expect(HttpMethod.Get, path)
				.Respond(HttpStatusCode.OK, "application/json", payload);

			_subject = new AcademyConversionProjectRepository(new MockHttpClientFactory(_messageHandler));
			var deliveryOfficers = new List<string>() { "John Smith" };

			Assert.Equal(0, _subject.GetAllProjects(1, 50, deliveryOfficers, string.Empty, string.Empty).Result.Body.Data.Count());
		}
	}
}
