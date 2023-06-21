using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class ProposedAcademyOpeningDateIntegrationTests : BaseIntegrationTests
{
   public ProposedAcademyOpeningDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_proposed_academy_opening_date()
   {
      string[] dates = Enumerable.Range(1, 12).Select(i => DateTime.Today.FirstOfMonth(i).ToDateString(true)).ToArray();
      (RadioButton selected, RadioButton toSelect) = RandomRadioButtons("proposed-academy-opening-date", dates);
      AcademyConversionProject project = AddGetProject(p => p.OpeningDate = DateTime.Parse(selected.Value));
      AddPatchConfiguredProject(project, x =>
      {
         x.OpeningDate = DateTime.Parse(toSelect.Value);
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
      await NavigateDataTestAsync("change-proposed-academy-opening-date");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/proposed-academy-opening-date");
      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked.Should().BeFalse();
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked.Should().BeTrue();

      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked = false;

      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");  

      Document.QuerySelector("#proposed-academy-opening-date").TextContent.Should().Contain(selected.Value);
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/proposed-academy-opening-date");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_proposed_academy_opening_date()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/proposed-academy-opening-date");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
   }
}
