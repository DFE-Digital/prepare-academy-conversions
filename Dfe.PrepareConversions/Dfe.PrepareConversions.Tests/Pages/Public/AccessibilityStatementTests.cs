using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.Public;

public class AccessibilityStatementTests : BaseIntegrationTests
{
   public AccessibilityStatementTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact]
   public async Task Should_navigate_to_the_accessibility_statement_from_the_link()
   {
      AcademyConversionProject project = AddGetProject();
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateDataTestAsync("accessibility-statement");

      Document.Url.Should().Contain("/public/accessibility");
   }
}
