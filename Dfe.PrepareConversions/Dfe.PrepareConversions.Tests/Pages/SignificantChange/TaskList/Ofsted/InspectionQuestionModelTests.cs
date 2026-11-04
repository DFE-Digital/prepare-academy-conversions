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
using System.Threading.Tasks;
using Xunit;
using static Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Ofsted.OfstedPageModelTestHelpers;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Ofsted;

public class InspectionQuestionModelTests
{
   [Fact]
   public async Task OnGetAsync_populates_question_values_from_session()
   {
      const int id = 601;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      MemorySession session = new();
      SetSignificantChangeOfstedInspectionCommand inspection = new(
         new DateTime(2025, 9, 15),
         false,
         "Improvement actions are underway",
         "School improvement plan");
      SetInspection(session, id, inspection);
      InspectionQuestionModel sut = BuildModel(repository.Object, session);

      IActionResult result = await sut.OnGetAsync(id);

      Assert.IsType<PageResult>(result);
      sut.DateOfInspection.Should().Be(inspection.InspectionDate);
      sut.MeetsOfstedStandards.Should().BeFalse();
      sut.AdditionalInformation.Should().Be(inspection.AdditionalInformation);
      sut.SupportingEvidence.Should().Be(inspection.SupportingEvidence);
   }

   [Fact]
   public async Task OnGetAsync_without_inspection_date_redirects_to_date_page()
   {
      const int id = 602;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      MemorySession session = new();
      SetInspection(session, id, new SetSignificantChangeOfstedInspectionCommand(null, null, null, null));
      InspectionQuestionModel sut = BuildModel(repository.Object, session);

      IActionResult result = await sut.OnGetAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.OfstedInspection.Page);
      redirect.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(id);
   }

   [Fact]
   public async Task OnPostAsync_with_yes_clears_additional_information_saves_and_removes_session()
   {
      const int id = 603;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetOfstedInspection(id, It.IsAny<SetSignificantChangeOfstedInspectionCommand>()))
         .Returns(Task.CompletedTask);
      MemorySession session = new();
      SetSignificantChangeOfstedInspectionCommand inspection = new(
         new DateTime(2025, 9, 15),
         null,
         "Previous information",
         "Inspection report");
      SetInspection(session, id, inspection);
      InspectionQuestionModel sut = BuildModel(repository.Object, session);
      sut.MeetsOfstedStandards = true;
      sut.AdditionalInformation = "This should be cleared";
      sut.SupportingEvidence = "Updated evidence";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      repository.Verify(
         x => x.SetOfstedInspection(
            id,
            It.Is<SetSignificantChangeOfstedInspectionCommand>(command =>
               command.InspectionDate == inspection.InspectionDate
               && command.MeetsOfstedStandards == true
               && command.AdditionalInformation == null
               && command.SupportingEvidence == "Updated evidence")),
         Times.Once);
      GetInspection(session, id).Should().BeNull();
   }

   [Fact]
   public async Task OnPostAsync_with_no_saves_additional_information_and_redirects_to_task_list()
   {
      const int id = 604;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetOfstedInspection(id, It.IsAny<SetSignificantChangeOfstedInspectionCommand>()))
         .Returns(Task.CompletedTask);
      MemorySession session = new();
      SetSignificantChangeOfstedInspectionCommand inspection = new(
         new DateTime(2025, 9, 15),
         null,
         null,
         null);
      SetInspection(session, id, inspection);
      InspectionQuestionModel sut = BuildModel(repository.Object, session);
      sut.MeetsOfstedStandards = false;
      sut.AdditionalInformation = "Improvement actions are underway";
      sut.SupportingEvidence = "School improvement plan";

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<RedirectToPageResult>(result);
      repository.Verify(
         x => x.SetOfstedInspection(
            id,
            It.Is<SetSignificantChangeOfstedInspectionCommand>(command =>
               command.InspectionDate == inspection.InspectionDate
               && command.MeetsOfstedStandards == false
               && command.AdditionalInformation == "Improvement actions are underway"
               && command.SupportingEvidence == "School improvement plan")),
         Times.Once);
      GetInspection(session, id).Should().BeNull();
   }

   [Fact]
   public async Task OnPostAsync_without_selection_returns_page_with_validation_error_and_does_not_save()
   {
      const int id = 605;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      MemorySession session = new();
      SetInspection(session, id, new SetSignificantChangeOfstedInspectionCommand(
         new DateTime(2025, 9, 15),
         null,
         null,
         null));
      InspectionQuestionModel sut = BuildModel(repository.Object, session);

      IActionResult result = await sut.OnPostAsync(id);

      Assert.IsType<PageResult>(result);
      sut.ModelState[nameof(InspectionQuestionModel.MeetsOfstedStandards)]!.Errors[0].ErrorMessage
         .Should().Be("Select an option");
      repository.Verify(x => x.SetOfstedInspection(id, It.IsAny<SetSignificantChangeOfstedInspectionCommand>()), Times.Never);
   }

   [Fact]
   public async Task OnPostAsync_without_inspection_date_redirects_to_date_page_without_saving()
   {
      const int id = 606;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      MemorySession session = new();
      InspectionQuestionModel sut = BuildModel(repository.Object, session);
      sut.MeetsOfstedStandards = true;

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.OfstedInspection.Page);
      repository.Verify(x => x.SetOfstedInspection(id, It.IsAny<SetSignificantChangeOfstedInspectionCommand>()), Times.Never);
   }

   private static InspectionQuestionModel BuildModel(
      ISignificantChangeProjectRepository repository,
      MemorySession session)
   {
      InspectionQuestionModel model = new(repository, session, new ErrorService());
      SetPageContext(model);
      return model;
   }
}
