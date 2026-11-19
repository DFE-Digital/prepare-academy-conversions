using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList;

public class SharePointLinkIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Theory]
   [InlineData("admissions-variation-details", "SupportingEvidence")]
   [InlineData("fha-risks-or-issues-raised", "SupportingEvidence")]
   [InlineData("funding", "SupportingEvidence")]
   [InlineData("hqtf-consideration", "SupportingEvidence")]
   [InlineData("land-transaction", "LandTransactionSupportingEvidence")]
   [InlineData("local-authority-objections", "LocalAuthorityObjectionsSupportingEvidenceLink")]
   [InlineData("ofsted-inspection-question", "SupportingEvidence")]
   [InlineData("planning-permission", "PlanningPermissionSupportingEvidence")]
   [InlineData("public-sector-equality-duty", "SupportingEvidence")]
   [InlineData("religious-bodies", "SupportingEvidence")]
   [InlineData("other-stakeholder-engagement", "SupportingEvidence")]
   public async Task Should_reject_invalid_evidence_and_render_an_associated_inline_error(string path, string field)
   {
      const int id = 951;
      SignificantChangeProjectResponse project = new()
      {
         Id = id,
         ApplicationId = "1234",
         ApplicationReference = "56789",
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         LocalAuthorityName = "Test local authority",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         OfstedInspection = new SignificantChangeOfstedInspectionResponse
         {
            InspectionDate = new DateTime(2025, 9, 15)
         }
      };
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, id), project);

      if (path == "ofsted-inspection-question")
      {
         await OpenAndConfirmPathAsync($"/significant-change/task-list/{id}/ofsted");
      }

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{id}/{path}");

      (string Link, string Error)[] invalidLinks =
      [
         ("http://educationgovuk.sharepoint.com/sites/evidence", "The Supporting Evidence must have the https scheme"),
         ("https://other.sharepoint.com/sites/evidence", "Enter a Supporting Evidence in the correct format. SharePoint links start with 'https://educationgovuk.sharepoint.com' or 'https://educationgovuk-my.sharepoint.com/'")
      ];

      foreach (var invalidLink in invalidLinks)
      {
         Document.QuerySelector<IHtmlInputElement>($"input[name='{field}']")!.Value = invalidLink.Link;

         await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

         Document.Url.Should().EndWith($"/significant-change/task-list/{id}/{path}");
         var input = Document.QuerySelector<IHtmlInputElement>($"input[name='{field}']")!;
         input.Value.Should().Be(invalidLink.Link);
         input.ClassList.Should().Contain("govuk-input--error");
         input.GetAttribute("aria-describedby").Should().Be($"{field}-error");
         Document.QuerySelector($"#{field}-error")!.TextContent.Should().Contain(invalidLink.Error);
      }
   }
}