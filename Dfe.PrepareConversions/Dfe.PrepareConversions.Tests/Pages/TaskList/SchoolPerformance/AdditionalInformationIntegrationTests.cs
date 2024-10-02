using AngleSharp.Dom;
using AngleSharp.Html.Dom; 
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolPerformance;

public class AdditionalInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_navigate_to_and_update_additional_information()
   {
      var project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-performance-ofsted-information");
      VerifyNullElement("Change");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      var project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-performance-ofsted-information/additional-information");

      await Document.QuerySelector<IHtmlFormElement>("form")?.SubmitAsync()!;

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_confirm_school_performance()
   {
      var project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-performance-ofsted-information/additional-information");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information");
   }
}
