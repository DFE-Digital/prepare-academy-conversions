using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class Form7ReceivedDateIntegrationTests : BaseIntegrationTests
{
   private static readonly DateTime Tomorrow = DateTime.Today.Add(TimeSpan.FromDays(1));
   private static readonly DateTime Yesterday = DateTime.Today.Subtract(TimeSpan.FromDays(1));

   public Form7ReceivedDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   private void FillInDateWith(DateTime? date)
   {
      InputWithId("form-7-received-date-day").Value = date?.Day.ToString() ?? string.Empty;
      InputWithId("form-7-received-date-month").Value = date?.Month.ToString() ?? string.Empty;
      InputWithId("form-7-received-date-year").Value = date?.Year.ToString() ?? string.Empty;
   }

   [Fact]
   public async Task Should_navigate_to_and_update_form_7_received_date()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.Form7ReceivedDate = null;
      });

      ExpectPatchProjectMatching(project, update => update.Form7ReceivedDate == Yesterday &&
                                                    update.Urn.Equals(project.Urn));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      FillInDateWith(Yesterday);

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
   }

   [Fact]
   public async Task Should_accept_a_blank_value_and_return_to_the_project_dates_page()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.Form7ReceivedDate = null;
      });

      ExpectPatchProjectMatching(project, update => update.Form7ReceivedDate is null);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      FillInDateWith(null);

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
   }

   [Fact]
   public async Task Should_display_an_error_if_the_save_api_call_fails()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.Form7ReceivedDate = null;
      });

      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      FillInDateWith(Yesterday);

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("There is a system problem and we could not save your changes.");
   }

   [Fact]
   public async Task Should_report_an_error_if_the_day_value_is_not_specified()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.Form7ReceivedDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      FillInDateWith(Yesterday);

      InputWithId("form-7-received-date-day").Value = default!;

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("must include a day");
   }

   [Fact]
   public async Task Should_report_an_error_if_the_month_component_is_not_specified()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.Form7ReceivedDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      FillInDateWith(Yesterday);
      InputWithId("form-7-received-date-month").Value = default!;

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("must include a month");
   }

   [Fact]
   public async Task Should_report_an_error_if_the_year_component_is_not_specified()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.Form7ReceivedDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      FillInDateWith(Yesterday);
      InputWithId("form-7-received-date-year").Value = default!;

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("must include a year");
   }

   [Fact]
   public async Task Should_report_an_error_if_the_date_provided_is_in_the_future()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.Form7ReceivedDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      FillInDateWith(Tomorrow);

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("date must be in the past");
   }

   [Fact]
   public async Task Should_navigate_back()
   {
      AcademyConversionProject project = AddGetProject(p => p.ApplicationReceivedDate = null);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/form-7-received-date");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
   }
}
