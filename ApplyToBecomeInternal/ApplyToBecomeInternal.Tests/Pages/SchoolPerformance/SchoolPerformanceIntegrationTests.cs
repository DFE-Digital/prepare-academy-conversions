using AngleSharp.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Tests.Helpers;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;
using SchoolPerformanceModel = ApplyToBecome.Data.Models.SchoolPerformance;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolPerformance
{
	public class SchoolPerformanceIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>
	{
		private readonly HttpClient _client;
		private readonly WireMockServer _server;

		public SchoolPerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
			_server = factory.WMServer;
		}

		[Fact]
		public async Task Should_navigate_to_school_performance_ofsted_information_from_task_list()
		{
			var (id, _) = SetupMockServer();

			var response = await _client.GetAsync($"/task-list/{id}");
			var document = await HtmlHelper.GetDocumentAsync(response);

			var schoolPerformancePage = await document.NavigateAsync("School performance (Ofsted information)");
			schoolPerformancePage.Url.Should().Be($"{document.Origin}/task-list/{id}/school-performance/ofsted-information");
		}

		[Fact]
		public async Task Should_navigate_back_to_task_list_from_school_performance_ofsted_information()
		{
			Debug.Print("VIV: " + System.Globalization.CultureInfo.CurrentCulture);
			Debug.Print("VIV: " + System.Globalization.CultureInfo.DefaultThreadCurrentCulture);
			Debug.Print("VIV: " + System.Globalization.CultureInfo.CurrentUICulture);
			Debug.Print("VIV: " + System.Globalization.CultureInfo.DefaultThreadCurrentUICulture);

			var (id, _) = SetupMockServer();

			var response = await _client.GetAsync($"/task-list/{id}/school-performance/ofsted-information");
			var document = await HtmlHelper.GetDocumentAsync(response);

			var taskList = await document.NavigateAsync("Back to task list");
			taskList.Url.Should().Be($"{document.Origin}/task-list/{id}");
		}

		[Fact]
		public async Task Should_display_school_performance_ofsted_information()
		{
			var (id, schoolPerformance) = SetupMockServer();

			var response = await _client.GetAsync($"/task-list/{id}/school-performance/ofsted-information");
			var document = await HtmlHelper.GetDocumentAsync(response);

			document.QuerySelector("#outcome-for-pupils").TextContent.Should().Be(schoolPerformance.PersonalDevelopment.DisplayOfstedRating());
			document.QuerySelector("#quality-of-teaching-learning-and-assessment").TextContent.Should().Be(schoolPerformance.QualityOfEducation.DisplayOfstedRating());
			document.QuerySelector("#effectiveness-of-leadership-and-management").TextContent.Should().Be(schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating());
			document.QuerySelector("#personal-development-behaviour-and-welfare").TextContent.Should().Be(schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating());
			document.QuerySelector("#early-years-provision").TextContent.Should().Be(schoolPerformance.EarlyYearsProvision.DisplayOfstedRating());
			document.QuerySelector("#overall-effectiveness").TextContent.Should().Be(schoolPerformance.OverallEffectiveness.DisplayOfstedRating());
		}

		private (int, SchoolPerformanceModel) SetupMockServer()
		{
			var fixture = new Fixture();
			var project = fixture.Create<Project>();
			fixture.Customizations.Add(new OfstedRatingSpecimenBuilder());
			var establishmentMockData = fixture.Create<EstablishmentMockData>();

			_server
				.Given(Request.Create()
					.WithPath($"/conversion-projects/{project.Id}")
					.UsingGet())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json")
					.WithBody(JsonSerializer.Serialize(project)));

			_server
				.Given(Request.Create()
					.WithPath($"/establishment/urn/{project.School.URN}")
					.UsingGet())
				.RespondWith(Response.Create()
					.WithStatusCode(200)
					.WithHeader("Content-Type", "application/json")
					.WithBody(JsonSerializer.Serialize(establishmentMockData)));

			return (project.Id, establishmentMockData.misEstablishment);
		}

		private class OfstedRatingSpecimenBuilder : ISpecimenBuilder
		{
			public object Create(object request, ISpecimenContext context)
			{
				var pi = request as PropertyInfo;
				if (pi == null || pi.PropertyType != typeof(string))
					return new NoSpecimen();

				var i = new Random().Next();

				return "12349".Substring(i % 5, 1);
			}
		}

		private class EstablishmentMockData
		{
			public SchoolPerformanceModel misEstablishment { get; set; }

			public DateTime ofstedLastInspection { get; set; }
		}
	}
}
