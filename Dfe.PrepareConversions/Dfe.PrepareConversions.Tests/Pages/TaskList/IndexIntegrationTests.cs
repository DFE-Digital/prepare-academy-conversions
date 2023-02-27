using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Tests.PageObjects;
using FluentAssertions;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList
{
	public class IndexIntegrationTests : BaseIntegrationTests, IAsyncLifetime
	{
		private AcademyConversionProject _project;
		private RecordDecisionWizard _wizard;

		public IndexIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		public IHtmlElement DecisionMadeByElement => Document.QuerySelector<IHtmlElement>("#decision-made-by");
		public IHtmlElement DecisionElement => Document.QuerySelector<IHtmlElement>("#decision");
		public IHtmlElement ConditionsSetElement => Document.QuerySelector<IHtmlElement>("#condition-set");
		public IHtmlElement DecisionDateElement => Document.QuerySelector<IHtmlElement>("#decision-date");

		[Fact]
		public async Task Should_redirect_to_record_decision()
		{
			await OpenAndConfirmPathAsync($"/task-list/{_project.Id}");

			await NavigateAsync("Record a decision");

			Document.Url.Should().Contain($"/task-list/{_project.Id}/decision/record-decision");
		}

      [Fact]
		public async Task Should_show_choices_from_api()
		{
			AdvisoryBoardDecision response = new AdvisoryBoardDecision
			{
				Decision = AdvisoryBoardDecisions.Approved,
				AdvisoryBoardDecisionDate = new DateTime(2021, 01, 02),
				ApprovedConditionsSet = true,
				ApprovedConditionsDetails = "The conditions are ......",
				DecisionMadeBy = DecisionMadeBy.OtherRegionalDirector,
				ConversionProjectId = _project.Id
			};

			_factory.AddGetWithJsonResponse($"/conversion-project/advisory-board-decision/{_project.Id}", response);

			await OpenAndConfirmPathAsync($"/task-list/{_project.Id}");

			DecisionElement.Text().Should()
				.Be("APPROVED WITH CONDITIONS");
			DecisionMadeByElement.Text().Should()
				.Be("A different Regional Director");
			ConditionsSetElement.Text().Trim()
				.Should().Contain("Yes");
			ConditionsSetElement.Text().Trim()
				.Should().Contain(response.ApprovedConditionsDetails);
			DecisionDateElement.Text().Trim().Should()
				.Be("02 January 2021");
		}

		#region IAsyncLifetime implementation

		public Task InitializeAsync()
		{
			_project = AddGetProject(p => p.GeneralInformationSectionComplete = false);
			_wizard = new RecordDecisionWizard(Context);
			return Task.CompletedTask;
		}

		public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

		#endregion
	}
}