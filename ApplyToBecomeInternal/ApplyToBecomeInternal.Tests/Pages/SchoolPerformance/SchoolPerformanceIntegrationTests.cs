using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using SchoolPerformanceModel = ApplyToBecome.Data.Models.SchoolPerformance;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolPerformance
{
	public class SchoolPerformanceIntegrationTests : BaseIntegrationTests
	{
		public SchoolPerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_school_performance_ofsted_information_from_task_list()
		{
			var (id, _) = SetupMockServer();

			await OpenUrlAsync($"/task-list/{id}");
			await NavigateAsync("School performance (Ofsted information)");
			
			Document.Url.Should().BeUrl($"/task-list/{id}/school-performance/ofsted-information");
		}

		[Fact]
		public async Task Should_navigate_back_to_task_list_from_school_performance_ofsted_information()
		{
			var (id, _) = SetupMockServer();

			await OpenUrlAsync($"/task-list/{id}/school-performance/ofsted-information");
			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{id}");
		}

		[Fact]
		public async Task Should_display_school_performance_ofsted_information()
		{
			var (id, schoolPerformance) = SetupMockServer();

			await OpenUrlAsync($"/task-list/{id}/school-performance/ofsted-information");

			Document.QuerySelector("#outcome-for-pupils").TextContent.Should().Be(schoolPerformance.PersonalDevelopment.DisplayOfstedRating());
			Document.QuerySelector("#quality-of-teaching-learning-and-assessment").TextContent.Should().Be(schoolPerformance.QualityOfEducation.DisplayOfstedRating());
			Document.QuerySelector("#effectiveness-of-leadership-and-management").TextContent.Should().Be(schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating());
			Document.QuerySelector("#personal-development-behaviour-and-welfare").TextContent.Should().Be(schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating());
			Document.QuerySelector("#early-years-provision").TextContent.Should().Be(schoolPerformance.EarlyYearsProvision.DisplayOfstedRating());
			Document.QuerySelector("#overall-effectiveness").TextContent.Should().Be(schoolPerformance.OverallEffectiveness.DisplayOfstedRating());
		}

		private (int, SchoolPerformanceModel) SetupMockServer()
		{
			var project = AddGetProject();
			var schoolPerformance = AddGetSchoolPerformance(project.Urn.ToString());

			return (project.Id, schoolPerformance);
		}
	}
}
