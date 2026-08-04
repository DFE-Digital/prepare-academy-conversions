using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList;

public class SignificantChangeTaskListModelTests
{
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
}