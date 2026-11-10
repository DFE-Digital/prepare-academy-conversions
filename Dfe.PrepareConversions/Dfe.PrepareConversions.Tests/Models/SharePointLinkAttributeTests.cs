using System.ComponentModel.DataAnnotations;
using Dfe.PrepareConversions.Models;
using FluentAssertions;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models;

public class SharePointLinkAttributeTests
{
   [Theory]
   [InlineData(null)]
   [InlineData("")]
   [InlineData("https://educationgovuk.sharepoint.com")]
   [InlineData("https://educationgovuk.sharepoint.com/sites/evidence/document.docx?web=1")]
   [InlineData("https://educationgovuk-my.sharepoint.com/personal/user/document.docx")]
   public void GetValidationResult_Allows_optional_values_and_sharepoint_links(string link)
   {
      var result = Validate(link, nameof(EvidenceModel.SupportingEvidence));

      result.Should().Be(ValidationResult.Success);
   }

   [Theory]
   [InlineData("http://educationgovuk.sharepoint.com/sites/evidence")]
   [InlineData("educationgovuk.sharepoint.com/sites/evidence")]
   [InlineData("ftp://educationgovuk.sharepoint.com/sites/evidence")]
   [InlineData(" ")]
   public void GetValidationResult_Requires_https(string link)
   {
      var result = Validate(link, nameof(EvidenceModel.SupportingEvidence));

      result.ErrorMessage.Should().Be("The Supporting Evidence must have the https scheme");
   }

   [Theory]
   [InlineData("https://example.com/evidence")]
   [InlineData("https://sharepoint.com/evidence")]
   [InlineData("https://other.sharepoint.com/evidence")]
   public void GetValidationResult_Requires_a_department_sharepoint_link(string link)
   {
      var result = Validate(link, nameof(EvidenceModel.SupportingEvidence));

      result.ErrorMessage.Should().Be("Enter a Supporting Evidence in the correct format. SharePoint links start with 'https://educationgovuk.sharepoint.com' or 'https://educationgovuk-my.sharepoint.com/'");
   }

   [Fact]
   public void GetValidationResult_Uses_an_for_an_evidence_display_name()
   {
      var result = Validate("https://example.com", nameof(EvidenceModel.Evidence));

      result.ErrorMessage.Should().Be("Enter an Evidence link in the correct format. SharePoint links start with 'https://educationgovuk.sharepoint.com' or 'https://educationgovuk-my.sharepoint.com/'");
   }

   [Fact]
   public void GetValidationResult_Falls_back_to_the_validation_context_display_name()
   {
      var context = new ValidationContext(new EvidenceModel())
      {
         MemberName = nameof(EvidenceModel.Link),
         DisplayName = "Fallback link"
      };

      var result = new SharePointLinkAttribute().GetValidationResult("http://example.com", context);

      result.ErrorMessage.Should().Be("The Fallback link must have the https scheme");
   }

   private static ValidationResult Validate(string link, string memberName) =>
      new SharePointLinkAttribute().GetValidationResult(link, new ValidationContext(new EvidenceModel())
      {
         MemberName = memberName
      });

   private sealed class EvidenceModel
   {
      [Display(Name = "Supporting Evidence")]
      public string SupportingEvidence { get; set; }

      [Display(Name = "Evidence link")]
      public string Evidence { get; set; }

      public string Link { get; set; }
   }
}