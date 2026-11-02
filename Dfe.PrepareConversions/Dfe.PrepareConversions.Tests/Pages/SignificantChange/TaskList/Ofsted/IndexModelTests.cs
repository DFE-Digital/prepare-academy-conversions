using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Ofsted;
using Dfe.PrepareConversions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Ofsted.OfstedPageModelTestHelpers;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Ofsted;

public class IndexModelTests
{
   [Fact]
   public async Task OnGetAsync_populates_inspection_date_from_project()
   {
      const int id = 501;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.OfstedInspection.InspectionDate = new DateTime(2025, 9, 15);
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);
      MemorySession session = new();
      IndexModel sut = BuildModel(repository.Object, session);

      IActionResult result = await sut.OnGetAsync(id);

      Assert.IsType<PageResult>(result);
      sut.InspectionDate.Should().Be(project.OfstedInspection.InspectionDate);
      GetInspection(session, id).Should().BeEquivalentTo(new SetSignificantChangeOfstedInspectionCommand(
         project.OfstedInspection.InspectionDate,
         project.OfstedInspection.MeetsOfstedStandards,
         project.OfstedInspection.AdditionalInformation,
         project.OfstedInspection.SupportingEvidence));
   }

   [Fact]
   public async Task OnPostAsync_updates_date_preserving_other_session_values_and_redirects_to_question()
   {
      const int id = 502;
      SignificantChangeProjectResponse project = BuildProject(id);
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);
      MemorySession session = new();
      SetSignificantChangeOfstedInspectionCommand existingInspection = new(
         new DateTime(2024, 8, 10),
         false,
         "Improvement plan",
         "Inspection report");
      SetInspection(session, id, existingInspection);
      IndexModel sut = BuildModel(repository.Object, session);
      sut.InspectionDate = new DateTime(2025, 9, 15);

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.OfstedInspectionQuestion.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);
      GetInspection(session, id).Should().BeEquivalentTo(existingInspection with
      {
         InspectionDate = sut.InspectionDate
      });
      repository.Verify(x => x.SetOfstedInspection(id, It.IsAny<SetSignificantChangeOfstedInspectionCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_without_existing_session_initializes_inspection_from_project()
   {
      const int id = 503;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.OfstedInspection.InspectionDate = new DateTime(2024, 8, 10);
      project.OfstedInspection.MeetsOfstedStandards = true;
      project.OfstedInspection.SupportingEvidence = "Inspection report";
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);
      MemorySession session = new();
      IndexModel sut = BuildModel(repository.Object, session);
      sut.InspectionDate = new DateTime(2025, 9, 15);

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<RedirectToPageResult>(result);
      GetInspection(session, id).Should().BeEquivalentTo(new SetSignificantChangeOfstedInspectionCommand(
         sut.InspectionDate,
         true,
         string.Empty,
         "Inspection report"));
   }

   [Fact]
   public async Task OnPostAsync_with_invalid_model_state_returns_page_without_changing_session()
   {
      const int id = 504;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      MemorySession session = new();
      SetSignificantChangeOfstedInspectionCommand existingInspection = new(
         new DateTime(2024, 8, 10),
         false,
         "Improvement plan",
         "Inspection report");
      SetInspection(session, id, existingInspection);
      IndexModel sut = BuildModel(repository.Object, session);
      sut.InspectionDate = new DateTime(2025, 9, 15);
      sut.ModelState.AddModelError(nameof(IndexModel.InspectionDate), "Invalid date");

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<PageResult>(result);
      GetInspection(session, id).Should().BeEquivalentTo(existingInspection);
      repository.Verify(x => x.SetOfstedInspection(id, It.IsAny<SetSignificantChangeOfstedInspectionCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnGetAsync_when_project_is_not_found_returns_not_found()
   {
      const int id = 505;
      Mock<ISignificantChangeProjectRepository> repository =
         BuildRepository(id, null, HttpStatusCode.NotFound);
      IndexModel sut = BuildModel(repository.Object, new MemorySession());

      IActionResult result = await sut.OnGetAsync(id);

      Assert.IsType<NotFoundResult>(result);
   }

   private static IndexModel BuildModel(
      ISignificantChangeProjectRepository repository,
      MemorySession session)
   {
      IndexModel model = new(repository, session, new ErrorService());
      SetPageContext(model);
      return model;
   }
}
