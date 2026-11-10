using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList;

public class BaseSignificantChangeTaskPageModelTests
{
   [Theory]
   [InlineData("")]
   [InlineData("?unrelated=value")]
   public void RedirectToTaskListOrReturnPage_WhenNoReturnPageIsSpecified_RedirectsToTaskList(string query)
   {
      TestTaskPageModel model = BuildModel(query);

      RedirectToPageResult result = Assert.IsType<RedirectToPageResult>(model.CompleteTask(123));

      Assert.Equal(Links.SignificantChange.SignificantChangeTaskList.Page, result.PageName);
      Assert.Equal(123, result.RouteValues["id"]);
      Assert.Single(result.RouteValues);
   }

   [Fact]
   public void RedirectToTaskListOrReturnPage_WhenReturningToRecordDecision_RedirectsWithProjectId()
   {
      TestTaskPageModel model = BuildModel(
         "?return=%2FSignificantChange%2FDecision%2FRecordDecision");

      RedirectToPageResult result = Assert.IsType<RedirectToPageResult>(model.CompleteTask(456));

      Assert.Equal(Links.SignificantChangeDecision.RecordDecision.Page, result.PageName);
      Assert.Equal(456, result.RouteValues["id"]);
      Assert.Single(result.RouteValues);
   }

   private static TestTaskPageModel BuildModel(string query)
   {
      DefaultHttpContext httpContext = new();
      httpContext.Request.QueryString = new QueryString(query);
      return new TestTaskPageModel
      {
         PageContext = new PageContext { HttpContext = httpContext }
      };
   }

   private sealed class TestTaskPageModel()
      : BaseSignificantChangeTaskPageModel(Mock.Of<ISignificantChangeProjectRepository>())
   {
      protected override string TaskTitle => "Test task";

      public IActionResult CompleteTask(int id) => RedirectToTaskListOrReturnPage(id);
   }
}
