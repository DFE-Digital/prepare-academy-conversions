using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class RecordDecisionModelTests
{
   private const int ProjectId = 123;

   [Fact]
   public async Task OnPost_WhenProjectIsNotAssigned_AddsAnErrorAndDoesNotSaveTheDecision()
   {
      Mock<ISignificantChangeProjectRepository> repository = new();
      Mock<ISession> session = CreateSessionWithDecision();
      ErrorService errorService = new();
      RecordDecisionModel model = CreateModel(repository, session, errorService, assigned: false);

      IActionResult result = await model.OnPost(ProjectId);

      Assert.IsType<PageResult>(result);
      Assert.Collection(errorService.GetErrors(),
         error => Assert.Equal("The project must be assigned to a user before a decision can be recorded.", error.Message));
      session.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never);
   }

   [Fact]
   public async Task OnPost_WhenTasksAreIncomplete_AddsAnErrorForEachTaskAndDoesNotSaveTheDecision()
   {
      Mock<ISignificantChangeProjectRepository> repository = new();
      Mock<ISession> session = CreateSessionWithDecision();
      ErrorService errorService = new();
      RecordDecisionModel model = CreateModel(repository, session, errorService, assigned: true, tasksComplete: false);

      IActionResult result = await model.OnPost(ProjectId);

      Assert.IsType<PageResult>(result);
      Assert.Collection(errorService.GetErrors(),
         error => Assert.Contains("Project dates", error.Message),
         error => Assert.Contains("Recommendation", error.Message));
      session.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never);
   }

   private static RecordDecisionModel CreateModel(
      Mock<ISignificantChangeProjectRepository> repository,
      Mock<ISession> session,
      ErrorService errorService,
      bool assigned,
      bool tasksComplete = true)
   {
      Mock<IUrlHelper> url = new();
      url.SetupGet(helper => helper.ActionContext)
         .Returns(new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()));
      int routeCount = 0;
      url.Setup(helper => helper.RouteUrl(It.IsAny<UrlRouteContext>()))
         .Returns(() => $"/task/{++routeCount}");

      return new RecordDecisionModel(repository.Object, session.Object, errorService)
      {
         Url = url.Object,
         Id = ProjectId,
         SignificantChangeDecision = SignificantChangeDecisions.Approved,
         Project = new()
         {
            Id = ProjectId,
            SchoolName = "Test school",
            Tier = 1,
            TrustName = "Test trust",
            TrustUkprn = "12345678",
            TypeOfSignificantChange = "Route A",
            Status = "pre decision",
            StatusColour = "blue",
            AssignedUser = assigned ? new User("user-id", "assigned.user@test.local", "Assigned User") : null
         },
         TaskList = new()
         {
            Sections =
            [
               new()
               {
                  Key = "section",
                  Title = "Section",
                  Tasks =
                  [
                     new()
                     {
                        Key = "project-dates",
                        Title = "Project dates",
                        Page = "/project-dates",
                        Status = tasksComplete ? TaskListItemViewModel.Completed : TaskListItemViewModel.InProgress
                     },
                     new()
                     {
                        Key = "recommendation",
                        Title = "Recommendation",
                        Page = "/recommendation",
                        Status = tasksComplete ? TaskListItemViewModel.Completed : TaskListItemViewModel.NotStarted
                     }
                  ]
               }
            ]
         }
      };
   }

   private static Mock<ISession> CreateSessionWithDecision()
   {
      Mock<ISession> session = new();
      byte[] decision = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(
         new SignificantChangeDecision { Decision = SignificantChangeDecisions.Approved }));
      session.Setup(s => s.TryGetValue($"{SignificantChangeDecisionModelKey}_{ProjectId}", out decision))
         .Returns(true);
      return session;
   }

   private const string SignificantChangeDecisionModelKey = "SignificantChangeDecision";
}
