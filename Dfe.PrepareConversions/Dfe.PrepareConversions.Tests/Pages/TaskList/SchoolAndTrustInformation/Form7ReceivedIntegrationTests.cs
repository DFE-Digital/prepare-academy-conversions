using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class Form7ReceivedIntegrationTests : BaseIntegrationTests
{
   public Form7ReceivedIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_form_7_received()
   {
      AcademyConversionProject project = AddGetProject(p => p.ApplicationReceivedDate = null);
      AddPatchConfiguredProject(project, x =>
      {
         x.Form7Received = "Yes";
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received");
      // Do the input boxes on the page default to none selected when coming from an empty value
      // Yes 
      Document.QuerySelector<IHtmlInputElement>("#form-7-received")!.IsChecked.Should().BeFalse();
      // No
      Document.QuerySelector<IHtmlInputElement>("#form-7-received-2")!.IsChecked.Should().BeFalse();
      // Not sure
      Document.QuerySelector<IHtmlInputElement>("#form-7-received-3")!.IsChecked.Should().BeFalse();
      // Not applicable
      Document.QuerySelector<IHtmlInputElement>("#form-7-received-4")!.IsChecked.Should().BeFalse();

      // Click "Yes" input box
      Document.QuerySelector<IHtmlInputElement>("#form-7-received")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#form-7-received")!.IsChecked.Should().BeTrue();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject(p => p.ApplicationReceivedDate = null);
      AddPatchConfiguredProject(project, x =>
      {
         x.Form7Received = "Yes";
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received");
      // Do the input boxes on the page default to none selected when coming from an empty value
      // Yes 
      Document.QuerySelector<IHtmlInputElement>("#form-7-received")!.IsChecked.Should().BeFalse();
      // No
      Document.QuerySelector<IHtmlInputElement>("#form-7-received-2")!.IsChecked.Should().BeFalse();
      // Not sure
      Document.QuerySelector<IHtmlInputElement>("#form-7-received-3")!.IsChecked.Should().BeFalse();
      // Not applicable
      Document.QuerySelector<IHtmlInputElement>("#form-7-received-4")!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received");
   }

   [Fact]
   public async Task Should_navigate_back()
   {
      AcademyConversionProject project = AddGetProject(p => p.ApplicationReceivedDate = null);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
   }
}
