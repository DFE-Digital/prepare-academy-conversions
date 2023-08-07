using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolOverview;

public class PublishedAdmissionNumberIntegrationTests : BaseIntegrationTests
{
   public PublishedAdmissionNumberIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_published_admission_number()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetEstablishmentResponse(project.Urn.ToString());
      UpdateAcademyConversionProject request = AddPatchConfiguredProject(project, x =>
      {
         x.PublishedAdmissionNumber = _fixture.Create<string>();
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-overview");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-overview/published-admission-number");
      Document.QuerySelector<IHtmlInputElement>("#published-admission-number")!.Value.Should().Be(project.PublishedAdmissionNumber);

      Document.QuerySelector<IHtmlInputElement>("#published-admission-number")!.Value = request.PublishedAdmissionNumber;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-overview/published-admission-number");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_published_admission_number()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-overview/published-admission-number");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");
   }
}
