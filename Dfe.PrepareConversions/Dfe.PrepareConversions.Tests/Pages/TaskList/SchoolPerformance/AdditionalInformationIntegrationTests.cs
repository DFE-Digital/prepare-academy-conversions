using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolPerformance;

public class AdditionalInformationIntegrationTests : BaseIntegrationTests
{
   public AdditionalInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_additional_information()
   {
      AcademyConversionProject project = AddGetProject();
      UpdateAcademyConversionProject request = AddPatchConfiguredProject(project, x =>
      {
         x.SchoolPerformanceAdditionalInformation = _fixture.Create<string>();
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-performance-ofsted-information");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information/additional-information");
      IHtmlTextAreaElement textArea = Document.QuerySelector<IHtmlTextAreaElement>("#additional-information");
      textArea?.TextContent.Should().Be(project.SchoolPerformanceAdditionalInformation);

      textArea!.Value = request.SchoolPerformanceAdditionalInformation;
      await Document.QuerySelector<IHtmlFormElement>("form")?.SubmitAsync()!;

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-performance-ofsted-information/additional-information");

      await Document.QuerySelector<IHtmlFormElement>("form")?.SubmitAsync()!;

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_confirm_school_performance()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-performance-ofsted-information/additional-information");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information");
   }
}
