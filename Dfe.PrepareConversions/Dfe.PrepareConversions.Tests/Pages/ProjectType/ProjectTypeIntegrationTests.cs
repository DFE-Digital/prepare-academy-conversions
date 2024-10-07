using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.ProjectType;

public class ProjectTypeIntegrationTests : BaseIntegrationTests
{
   public ProjectTypeIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }


   [Fact]
   public async Task Should_redirect_to_project_list_page()
   {
      await OpenAndConfirmPathAsync("/project-type", "/project-list");
   }

}
