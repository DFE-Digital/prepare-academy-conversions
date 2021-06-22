using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolPerformance
{
	public class ConfirmSchoolPerformanceIntegrationTests : BaseIntegrationTests
	{
		public ConfirmSchoolPerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_reference_only_and_display_school_performance()
		{
			var project = AddGetProject();
			var schoolPerformance = AddGetSchoolPerformance(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-performance-status").TextContent.Trim().Should().Be("Reference only");
			Document.QuerySelector("#school-performance-status").ClassName.Should().Contain("grey");

			await NavigateAsync("School performance (Ofsted information)");

			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.SchoolPerformanceAdditionalInformation);

			Document.QuerySelector("#ofsted-inspection-date").TextContent.Should().Be(schoolPerformance.OfstedLastInspection.ToDateString());
			Document.QuerySelector("#overall-effectiveness").TextContent.Should().Be(schoolPerformance.OverallEffectiveness.DisplayOfstedRating());
			Document.QuerySelector("#quality-of-teaching-learning-and-assessment").TextContent.Should().Be(schoolPerformance.QualityOfEducation.DisplayOfstedRating());
			Document.QuerySelector("#effectiveness-of-leadership-and-management").TextContent.Should().Be(schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating());
			Document.QuerySelector("#personal-development-behaviour-and-welfare").TextContent.Should().Be(schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating());
			Document.QuerySelector("#outcome-for-pupils").TextContent.Should().Be(schoolPerformance.PersonalDevelopment.DisplayOfstedRating());
			Document.QuerySelector("#early-years-provision").TextContent.Should().Be(schoolPerformance.EarlyYearsProvision.DisplayOfstedRating());
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_confirm_school_performance()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("School performance (Ofsted information)");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
