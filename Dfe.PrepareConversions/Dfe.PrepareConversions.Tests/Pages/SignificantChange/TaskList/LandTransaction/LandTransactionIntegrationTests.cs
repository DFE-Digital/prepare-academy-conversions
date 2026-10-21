using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.LandTransaction;

public class LandTransactionIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_land_transaction_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 801);
      project.LandTransaction.LandTransactionApplication = SignificantChangeGenericYesNoNa.No;
      project.LandTransaction.LandTransactionApplicationAdditionalInfo = "Land transaction application is pending";
      project.LandTransaction.LandTransactionConsent = SignificantChangeGenericYesNoNa.Yes;
      project.LandTransaction.LandTransactionConsentAdditionalInfo = "Consent has been granted";
      project.LandTransaction.LandTransactionSupportingEvidence = "https://educationgovuk.sharepoint.com/sites/land-transaction/reference/12345";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/land-transaction");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Land Transaction Application");
      Document.QuerySelector<IHtmlInputElement>("#land-transaction-application-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("#LandTransactionApplicationAdditionalInfo")!.Value
         .Should().Be("Land transaction application is pending");
      Document.QuerySelector<IHtmlInputElement>("#land-transaction-consent-yes")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("#LandTransactionConsentAdditionalInfo")!.Value
         .Should().Be("Consent has been granted");
      Document.QuerySelector<IHtmlInputElement>("input[name='LandTransactionSupportingEvidence']")!.Value
         .Should().Be("https://educationgovuk.sharepoint.com/sites/land-transaction/reference/12345");
   }

   [Fact]
   public async Task Should_save_answers_with_additional_information_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 802);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string applicationAdditionalInfo = "Land transaction application is awaiting completion";
      const string consentAdditionalInfo = "Land Transaction Team consent is pending";
      const string supportingEvidence = "https://educationgovuk.sharepoint.com/sites/land-transaction/application/67890";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeLandTransaction, project.Id),
         new SetSignificantChangeLandTransactionCommand(
            SignificantChangeGenericYesNoNa.No,
            applicationAdditionalInfo,
            SignificantChangeGenericYesNoNa.No,
            consentAdditionalInfo,
            supportingEvidence),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/land-transaction");

      Document.QuerySelector<IHtmlInputElement>("#land-transaction-application-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("#LandTransactionApplicationAdditionalInfo")!.Value = applicationAdditionalInfo;
      Document.QuerySelector<IHtmlInputElement>("#land-transaction-consent-awaiting-ltt")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("#LandTransactionConsentAdditionalInfo")!.Value = consentAdditionalInfo;
      Document.QuerySelector<IHtmlInputElement>("input[name='LandTransactionSupportingEvidence']")!.Value = supportingEvidence;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         ApplicationId = "12345",
         ApplicationReference = "12345",
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         LocalAuthorityName = "Test local authority",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         LandTransaction = new SignificantChangeLandTransactionResponse()
      };
   }
}
