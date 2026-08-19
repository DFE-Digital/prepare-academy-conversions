using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList;

public class SignificantChangeTaskListModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_Builds_task_list_and_returns_page()
   {
      const int id = 123;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(r => r.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id, "Route B")));

      IndexModel model = new(repository.Object);

      IActionResult result = await model.OnGetAsync(id);

      Assert.IsType<PageResult>(result);
      Assert.NotNull(model.TaskList);
      Assert.Single(model.TaskList.Sections);
      repository.Verify(r => r.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnGetAsync_WhenProjectIsNotFound_ReturnsNotFoundResult()
   {
      const int id = 123;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(r => r.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.NotFound, null));

      IndexModel model = new(repository.Object);

      IActionResult result = await model.OnGetAsync(id);

      Assert.IsType<NotFoundResult>(result);
      repository.Verify(r => r.GetProjectById(id), Times.Once);
   }

   private static SignificantChangeProjectResponse BuildProject(int id, string route)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Example trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = route,
         Status = "pre decision"
      };
   }
}
