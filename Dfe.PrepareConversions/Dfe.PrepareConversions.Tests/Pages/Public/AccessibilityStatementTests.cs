using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.Public;

public class AccessibilityStatementTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_have_correct_accessibility_statement_link()
   {
      AcademyConversionProject project = AddGetProject();
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      var linkElement = GetDataTestAnchorElement("accessibility-statement");

      linkElement.Should().NotBeNull();
      linkElement.Href.Should().Contain("https://accessibility-statements.education.gov.uk");
      linkElement.Target.Should().Be("_blank");   
   }
}
