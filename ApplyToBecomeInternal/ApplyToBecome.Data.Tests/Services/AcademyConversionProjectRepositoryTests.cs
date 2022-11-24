using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Tests.TestDoubles;
using FluentAssertions;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;

namespace ApplyToBecome.Data.Tests.Services
{
	public class AcademyConversionProjectRepositoryTests
	{
		private readonly MockHttpMessageHandler _messageHandler;
		private AcademyConversionProjectRepository _subject;

		public AcademyConversionProjectRepositoryTests()
		{
			_messageHandler = new MockHttpMessageHandler();
		}

		[Fact]
		public void GivenDeliveryOfficers_GetsRelevantProjects()
		{
			const string path = "http://localhost/v2/conversion-projects";

			List<AcademyConversionProject> project =
				new List<AcademyConversionProject> { new AcademyConversionProject { AssignedUser = new User("1", "example@email.com", "John Smith") } };
			ApiV2Wrapper<IEnumerable<AcademyConversionProject>> projects = new ApiV2Wrapper<IEnumerable<AcademyConversionProject>> { Data = project };

			_messageHandler.Expect(HttpMethod.Post, path)
				.Respond(HttpStatusCode.OK, "application/json", JsonSerializer.Serialize(projects));

			_subject = new AcademyConversionProjectRepository(new MockHttpClientFactory(_messageHandler));
			List<string> deliveryOfficers = new List<string> { "John Smith", "Jane Doe" };

			string firstAssignedFullName = _subject.GetAllProjects(1, 50, string.Empty, default, deliveryOfficers).Result.Body.Data.First().AssignedUser.FullName;

			firstAssignedFullName.Should().Be(project.First().AssignedUser.FullName);
		}

		[Fact]
		public void GivenDeliveryOfficers_GetsRelevantProjects_WhenMultiple()
		{
			const string path = "http://localhost/v2/conversion-projects";

			List<AcademyConversionProject> project = new List<AcademyConversionProject>
			{
				new AcademyConversionProject { AssignedUser = new User("1", "example@email.com", "John Smith") },
				new AcademyConversionProject { AssignedUser = new User("2", "example@email.com", "John Smith") }
			};
			ApiV2Wrapper<IEnumerable<AcademyConversionProject>> projects = new ApiV2Wrapper<IEnumerable<AcademyConversionProject>> { Data = project };

			_messageHandler.Expect(HttpMethod.Post, path)
				.Respond(HttpStatusCode.OK, "application/json", JsonSerializer.Serialize(projects));

			_subject = new AcademyConversionProjectRepository(new MockHttpClientFactory(_messageHandler));
			List<string> deliveryOfficers = new List<string> { "John Smith" };

			int projectCountReceived = _subject.GetAllProjects(1, 50, string.Empty, default, deliveryOfficers).Result.Body.Data.Count();

			projectCountReceived.Should().Be(project.Count);
		}

		[Fact]
		public void GivenDeliveryOfficers_GetsNoProjects_WhenDeliveryOfficerHasNoneAssigned()
		{
			const string path = "http://localhost/v2/conversion-projects";

			List<AcademyConversionProject> project = new List<AcademyConversionProject>();
			ApiV2Wrapper<IEnumerable<AcademyConversionProject>> projects = new ApiV2Wrapper<IEnumerable<AcademyConversionProject>> { Data = project };

			_messageHandler.Expect(HttpMethod.Post, path)
				.Respond(HttpStatusCode.OK, "application/json", JsonSerializer.Serialize(projects));

			_subject = new AcademyConversionProjectRepository(new MockHttpClientFactory(_messageHandler));
			List<string> deliveryOfficers = new List<string> { "John Smith" };

			IEnumerable<AcademyConversionProject> data = _subject.GetAllProjects(1, 50, string.Empty, default, deliveryOfficers).Result.Body.Data;

			data.Should().BeEmpty();
		}
	}
}
