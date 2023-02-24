using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation
{
   public class Form7ReceivedDateIntegrationTests : BaseIntegrationTests
   {
      public Form7ReceivedDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

      [Fact]
      public async Task Should_navigate_to_and_update_form_7_received_date()
      {
         _fixture.Customizations.Add(new RandomDateTimeSequenceGenerator(
            minDate: DateTime.Now.AddYears(-2),
            maxDate: DateTime.Now.AddDays(-1)
            ));
         var project = AddGetProject(p =>
         {
            p.ApplicationReceivedDate = null;
            p.Form7Received = "Yes";
         });
         var request = AddPatchConfiguredProject(project, x =>
         {
            x.Form7ReceivedDate = _fixture.Create<DateTime?>();
            x.Urn = project.Urn;
         });

         await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
         await NavigateAsync("Change", 4);

         Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

         Document.QuerySelector<IHtmlInputElement>("#form-7-received-date-day").Value = request.Form7ReceivedDate.Value.Day.ToString();
         Document.QuerySelector<IHtmlInputElement>("#form-7-received-date-month").Value = request.Form7ReceivedDate.Value.Month.ToString();
         Document.QuerySelector<IHtmlInputElement>("#form-7-received-date-year").Value = request.Form7ReceivedDate.Value.Year.ToString();

         await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

         Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
      }

      [Fact]
      public async Task Should_show_error_summary_when_there_is_an_API_error()
      {
         var project = AddGetProject(p => p.ApplicationReceivedDate = null);
         AddPatchConfiguredProject(project, x =>
         {
            x.Form7Received = "Yes";
            x.Urn = project.Urn;
         });

         await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
         await NavigateAsync("Change", 3);

         Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received");
         // Do the input boxes on the page default to none selected when coming from an empty value
         // Yes 
         Document.QuerySelector<IHtmlInputElement>("#form-7-received").IsChecked.Should().BeFalse();
         // No
         Document.QuerySelector<IHtmlInputElement>("#form-7-received-2").IsChecked.Should().BeFalse();
         // Not sure
         Document.QuerySelector<IHtmlInputElement>("#form-7-received-3").IsChecked.Should().BeFalse();
         // Not applicable
         Document.QuerySelector<IHtmlInputElement>("#form-7-received-4").IsChecked.Should().BeFalse();

         await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

         Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received");
      }

      [Fact]
      public async Task Should_navigate_back()
      {
         var project = AddGetProject(p => p.ApplicationReceivedDate = null);

         await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received");
         await NavigateAsync("Back");

         Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
      }
   }
}

