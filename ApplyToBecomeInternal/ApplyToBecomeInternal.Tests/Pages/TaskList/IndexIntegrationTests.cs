using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecomeInternal.Tests.PageObjects;
using FluentAssertions;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList
{
	public class IndexIntegrationTests : BaseIntegrationTests
	{
		public IndexIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task Should_display_selected_schoolname()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/decision/what-conditions");

			var selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span").Text();

			selectedSchool.Should().Be(project.SchoolName);
		}


		[Fact]
		public async Task Should_redirect_to_record_decision()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}?rd=x");

			await NavigateAsync("Record a decision");

			Document.Url.Should().Contain($"/task-list/{project.Id}/decision/record-decision");
		}


		[Fact]
		public async Task Should_show_choices_from_session()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var request = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 01),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "bills need to be paid",
				DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
				ConversionProjectId = project.Id
			};			

			await OpenUrlAsync($"/task-list/{project.Id}/decision/record-decision");

			await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

			await OpenUrlAsync($"/task-list/{project.Id}?rd=x");

			Document.QuerySelector<IHtmlElement>("#decision").Text().Should()
			.Be("APPROVED WITH CONDITIONS");
			Document.QuerySelector<IHtmlElement>("#decision-made-by").Text().Should()
				.Be("Director General");
			Document.QuerySelector<IHtmlElement>("#condition-set").Text().Trim().Should()
				.Be("Yes");
			Document.QuerySelector<IHtmlElement>("#condition-details").Text().Trim().Should()
				.Be(request.ApprovedConditionsDetails);

			var dateWithoutWhitespace = Regex.Replace(Document.QuerySelector<IHtmlElement>("#decision-date").Text(), @"\s+", "");
			dateWithoutWhitespace.Should().Be("01012021");
			
		   Document.QuerySelector<IHtmlAnchorElement>("#record-decision-link").Text().Trim().Should()
			   .Be("Change your decision");
		}


		[Fact]
		public async Task Should_show_choices_from_api()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);

			var response = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 02),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "The conditions are ......",
				DecisionMadeBy = DecisionMadeBy.OtherRegionalDirector,
				ConversionProjectId = project.Id
			};

			_factory.AddGetWithJsonResponse($"/conversion-project/advisory-board-decision/{project.Id}", response);

			await OpenUrlAsync($"/task-list/{project.Id}?rd=x");

			Document.QuerySelector<IHtmlElement>("#decision").Text().Should()
			.Be("APPROVED WITH CONDITIONS");
			Document.QuerySelector<IHtmlElement>("#decision-made-by").Text().Should()
				.Be("A different Regional Director");
			Document.QuerySelector<IHtmlElement>("#condition-set").Text().Trim().Should()
				.Be("Yes");
			Document.QuerySelector<IHtmlElement>("#condition-details").Text().Trim().Should()
				.Be(response.ApprovedConditionsDetails);

			var dateWithoutWhitespace = Regex.Replace(Document.QuerySelector<IHtmlElement>("#decision-date").Text(), @"\s+", "");
			dateWithoutWhitespace.Should().Be("02012021");
		}
	}
}
