using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class DaoPackSentDateIntegrationTests : BaseIntegrationTests
{
   private static readonly DateTime Tomorrow = DateTime.Today.Add(TimeSpan.FromDays(1));
   private static readonly DateTime Yesterday = DateTime.Today.Subtract(TimeSpan.FromDays(1));

   public DaoPackSentDateIntegrationTests(IntegrationTestingWebApplicationFactory factory)
      : base(factory)
   {
   }

   private void FillInDateWith(DateTime? date)
   {
      InputWithId("dao-pack-sent-date-day").Value = date?.Day.ToString() ?? string.Empty;
      InputWithId("dao-pack-sent-date-month").Value = date?.Month.ToString() ?? string.Empty;
      InputWithId("dao-pack-sent-date-year").Value = date?.Year.ToString() ?? string.Empty;
   }

   [Fact]
   public async Task Should_be_able_to_edit_the_dao_pack_sent_date()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.DaoPackSentDate = null;
      });

      ExpectPatchProjectMatching(project, update => update.DaoPackSentDate == Yesterday &&
                                                    update.Urn.Equals(project.Urn));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      FillInDateWith(Yesterday);

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
   }

   [Fact]
   public async Task Should_accept_a_blank_value_and_return_to_the_project_dates_page()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.DaoPackSentDate = null;
      });

      ExpectPatchProjectMatching(project, update => update.DaoPackSentDate is null);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      FillInDateWith(null);

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
   }

   [Fact]
   public async Task Should_display_an_error_if_the_save_api_call_fails()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.DaoPackSentDate = null;
      });

      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      FillInDateWith(Yesterday);

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("There is a system problem and we could not save your changes.");
   }

   [Fact]
   public async Task Should_report_an_error_if_the_day_value_is_not_specified()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.DaoPackSentDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      FillInDateWith(Yesterday);

      InputWithId("dao-pack-sent-date-day").Value = default!;

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("DAO Pack sent must include a day");
   }

   [Fact]
   public async Task Should_report_an_error_if_the_month_component_is_not_specified()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.DaoPackSentDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      FillInDateWith(Yesterday);
      InputWithId("dao-pack-sent-date-month").Value = default!;

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("DAO Pack sent must include a month");
   }

   [Fact]
   public async Task Should_report_an_error_if_the_year_component_is_not_specified()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.DaoPackSentDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      FillInDateWith(Yesterday);
      InputWithId("dao-pack-sent-date-year").Value = default!;

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("DAO Pack sent must include a year");
   }

   [Fact]
   public async Task Should_report_an_error_if_the_date_provided_is_in_the_future()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.DaoPackSentDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      FillInDateWith(Tomorrow);

      await ClickCommonSubmitButtonAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/dao-pack-sent-date");

      Document.QuerySelector("[data-cy=\"error-summary\"]")!.TextContent
         .Should().Contain("DAO Pack sent date must be in the past");
   }
}
