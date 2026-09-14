using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ConfirmProjectDates;
using Dfe.PrepareConversions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.ConfirmProjectDates;

public class ConfirmProjectDatesModelTests
{
   [Fact]
   public async Task OnGetAsync_WhenProjectExists_ShouldPopulateValuesAndReturnPage()
   {
      const int id = 401;
      var proposedDecisionDate = new DateTime(2024, 12, 15);
      var proposedChangeDate = new DateTime(2025, 01, 20);

      SignificantChangeProjectResponse project = BuildProject(id);
      project.ProjectDates = new SignificantChangeProjectDatesResponse
      {
         ProposedDecisionDate = proposedDecisionDate,
         ProposedChangeDate = proposedChangeDate,
         Status = SignificantChangeTaskStatus.Completed
      };

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ProposedDecisionDate.Should().Be(proposedDecisionDate);
      sut.ProposedChangeDate.Should().Be(proposedChangeDate);
      repository.Verify(x => x.GetProjectById(id), Times.Once);
   }

   [Fact]
   public async Task OnGetAsync_WhenProjectDoesNotExist_ShouldReturnNotFound()
   {
      const int id = 402;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.NotFound, null));

      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<NotFoundResult>();
   }

   [Fact]
   public async Task OnPostAsync_WhenBothDatesAreProvided_ShouldSaveAndRedirect()
   {
      const int id = 403;
      var proposedDecisionDate = new DateTime(2024, 12, 15);
      var proposedChangeDate = new DateTime(2025, 01, 20);

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));
      repository
         .Setup(x => x.SetProjectDates(id, It.IsAny<SetSignificantChangeProjectDatesCommand>()))
         .Returns(Task.CompletedTask);

      IndexModel sut = BuildModel(repository.Object);
      sut.ProposedDecisionDate = proposedDecisionDate;
      sut.ProposedChangeDate = proposedChangeDate;

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetProjectDates(
         id,
         It.Is<SetSignificantChangeProjectDatesCommand>(command =>
            command.ProposedDecisionDate == proposedDecisionDate
            && command.ProposedChangeDate == proposedChangeDate)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenProposedDecisionDateIsMissing_ShouldSaveAndRedirect()
   {
      const int id = 404;
      var proposedChangeDate = new DateTime(2025, 01, 20);

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.ProposedDecisionDate = null;
      sut.ProposedChangeDate = proposedChangeDate;

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetProjectDates(
         id,
         It.Is<SetSignificantChangeProjectDatesCommand>(command =>
            command.ProposedDecisionDate == null
            && command.ProposedChangeDate == proposedChangeDate)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenProposedChangeDateIsMissing_ShouldSaveAndRedirect()
   {
      const int id = 405;
      var proposedDecisionDate = new DateTime(2024, 12, 15);

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.ProposedDecisionDate = proposedDecisionDate;
      sut.ProposedChangeDate = null;

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetProjectDates(
         id,
         It.Is<SetSignificantChangeProjectDatesCommand>(command =>
            command.ProposedDecisionDate == proposedDecisionDate
            && command.ProposedChangeDate == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenBothDatesAreMissing_ShouldSaveAndRedirect()
   {
      const int id = 406;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, BuildProject(id)));

      IndexModel sut = BuildModel(repository.Object);
      sut.ProposedDecisionDate = null;
      sut.ProposedChangeDate = null;

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);

      repository.Verify(x => x.SetProjectDates(
         id,
         It.Is<SetSignificantChangeProjectDatesCommand>(command =>
            command.ProposedDecisionDate == null
            && command.ProposedChangeDate == null)), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_WhenProjectNotFound_ShouldReturnNotFound()
   {
      const int id = 407;

      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.NotFound, null));

      IndexModel sut = BuildModel(repository.Object);
      sut.ProposedDecisionDate = new DateTime(2024, 12, 15);
      sut.ProposedChangeDate = new DateTime(2025, 01, 20);

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<NotFoundResult>();
      repository.Verify(x => x.SetProjectDates(id, It.IsAny<SetSignificantChangeProjectDatesCommand>()), Times.Never);
   }

   private static IndexModel BuildModel(ISignificantChangeProjectRepository repository)
   {
      DefaultHttpContext httpContext = new();
      ModelStateDictionary modelState = new();
      ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor(), modelState);
      PageContext pageContext = new(actionContext)
      {
         ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState)
      };

      return new IndexModel(repository, new ErrorService())
      {
         PageContext = pageContext
      };
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Example trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse(),
         ProjectDates = new SignificantChangeProjectDatesResponse()
      };
   }
}
