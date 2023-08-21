using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList;

public class IndexIntegrationTests : BaseIntegrationTests, IAsyncLifetime
{
   private AcademyConversionProject _project;

   public IndexIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   private IHtmlElement DecisionMadeByElement => Document.QuerySelector<IHtmlElement>("#decision-made-by");
   private IHtmlElement DecisionElement => Document.QuerySelector<IHtmlElement>("#decision");
   private IHtmlElement ConditionsSetElement => Document.QuerySelector<IHtmlElement>("#condition-set");
   private IHtmlElement DecisionDateElement => Document.QuerySelector<IHtmlElement>("#decision-date");

   [Fact]
   public async Task Should_redirect_to_record_decision()
   {
      await OpenAndConfirmPathAsync($"/task-list/{_project.Id}/record-a-decision");

      await NavigateAsync("Record a decision", 1);

      Document.Url.Should().Contain($"/task-list/{_project.Id}/decision/record-decision");
   }

   [Fact]
   public async Task Should_show_choices_from_api()
   {
      AdvisoryBoardDecision response = new()
      {
         Decision = AdvisoryBoardDecisions.Approved,
         AdvisoryBoardDecisionDate = new DateTime(2021, 01, 02),
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "The conditions are ......",
         DecisionMadeBy = DecisionMadeBy.OtherRegionalDirector,
         ConversionProjectId = _project.Id
      };

      _factory.AddGetWithJsonResponse($"/conversion-project/advisory-board-decision/{_project.Id}", response);

      await OpenAndConfirmPathAsync($"/task-list/{_project.Id}/record-a-decision");

      DecisionElement.Text().Should()
         .Be("Approved with Conditions");
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
      _project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);
      return Task.CompletedTask;
   }

   public Task DisposeAsync()
   {
      return Task.CompletedTask;
   }

   #endregion
}
