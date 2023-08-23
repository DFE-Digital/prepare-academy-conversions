using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.ProjectNotes;

public class ProjectNotesIntegrationTests : BaseIntegrationTests
{
   public ProjectNotesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_project_notes_from_submenu_and_back_to_project_list()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetProjectNotes(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Project notes");

      Document.Url.Should().BeUrl($"/project-notes/{project.Id}");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl("/project-list");
   }

   [Fact]
   public async Task Should_display_project_notes()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Project notes");

      ProjectNote firstProjectNote = project.Notes.OrderByDescending(x => x.Date).First();
      Document.QuerySelector("#project-note-subject-0")!.TextContent.Trim().Should().Be(firstProjectNote.Subject);
      Document.QuerySelector("#project-note-body-0")!.TextContent.Should().Be(firstProjectNote.Note);
      string expectedFirstProjectNoteAuthorAndDate =
         $"{firstProjectNote.Author}, {firstProjectNote.Date.Day} {firstProjectNote.Date:MMMM} {firstProjectNote.Date.Year} at {firstProjectNote.Date:hh:mm}{firstProjectNote.Date.ToString("tt").ToLower()}";
      Document.QuerySelector("#project-note-date-0")!.TextContent.Trim().Should().Be(expectedFirstProjectNoteAuthorAndDate);

      ProjectNote secondProjectNote = project.Notes.OrderByDescending(x => x.Date).ElementAt(1);
      Document.QuerySelector("#project-note-subject-1")!.TextContent.Trim().Should().Be(secondProjectNote.Subject);
      Document.QuerySelector("#project-note-body-1")!.TextContent.Should().Be(secondProjectNote.Note);
      string expectedSecondProjectNoteAuthorAndDate =
         $"{secondProjectNote.Author}, {secondProjectNote.Date.Day} {secondProjectNote.Date:MMMM} {secondProjectNote.Date.Year} at {secondProjectNote.Date:hh:mm}{secondProjectNote.Date.ToString("tt").ToLower()}";
      Document.QuerySelector("#project-note-date-1")!.TextContent.Trim().Should().Be(expectedSecondProjectNoteAuthorAndDate);
   }
}
