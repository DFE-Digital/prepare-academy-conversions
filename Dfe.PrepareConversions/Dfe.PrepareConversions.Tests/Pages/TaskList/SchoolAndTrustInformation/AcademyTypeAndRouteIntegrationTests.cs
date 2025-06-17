using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class AcademyTypeAndRouteIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_navigate_to_and_update_conversion_support_grant_amount()
   {
      var project = AddGetProject(x =>
      {
         x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
         x.ApplicationReceivedDate = new DateTime(2024, 12, 20, 23, 59, 59, DateTimeKind.Utc); // Before deadline
      });

      var request = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.ConversionSupportGrantAmount)
            .With(r => r.ConversionSupportGrantChangeReason)
            .With(r => r.Urn, project.Urn)
      );

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/conversion-details");
      await NavigateDataTestAsync("change-grant-funding-amount");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/route-and-grant");
      Document.QuerySelector<IHtmlInputElement>("#conversion-support-grant-amount")!.Value.Should().Be(project.ConversionSupportGrantAmount?.ToMoneyString());
      Document.QuerySelector<IHtmlTextAreaElement>("#conversion-support-grant-change-reason")!.TextContent.Should().Be(project.ConversionSupportGrantChangeReason);

      Document.QuerySelector<IHtmlInputElement>("#conversion-support-grant-amount")!.Value = request.ConversionSupportGrantAmount?.ToMoneyString()!;
      Document.QuerySelector<IHtmlTextAreaElement>("#conversion-support-grant-change-reason")!.Value = request.ConversionSupportGrantChangeReason;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
   }
}
