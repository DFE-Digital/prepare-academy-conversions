using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.ProjectNotes
{
	public class ProjectNotesIntegrationTests : BaseIntegrationTests
	{
		public ProjectNotesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_project_notes_from_submenu_and_back_to_project_list()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("Project notes");

			Document.Url.Should().BeUrl($"/project-notes/{project.Id}");

			await NavigateAsync("Back to all conversion projects");

			Document.Url.Should().BeUrl($"/project-list");
		}

		[Fact]
		public async Task Should_display_project_notes()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("Project notes");

			var firstProjectNote = project.ProjectNotes.First();
			Document.QuerySelector("#project-note-subject-0").TextContent.Trim().Should().Be(firstProjectNote.Subject);
			Document.QuerySelector("#project-note-body-0").TextContent.Should().Be(firstProjectNote.Note);
			var expectedFirstProjectNoteAuthorAndDate = $"{firstProjectNote.Author}, {firstProjectNote.Date.Day} {firstProjectNote.Date:Y} at {firstProjectNote.Date:t}{firstProjectNote.Date.ToString("tt").ToLower()}";
			Document.QuerySelector("#project-note-date-0").TextContent.Trim().Should().Be(expectedFirstProjectNoteAuthorAndDate);

			var secondProjectNote = project.ProjectNotes.ElementAt(1);
			Document.QuerySelector("#project-note-subject-1").TextContent.Trim().Should().Be(secondProjectNote.Subject);
			Document.QuerySelector("#project-note-body-1").TextContent.Should().Be(secondProjectNote.Note);
			var expectedSecondProjectNoteAuthorAndDate = $"{secondProjectNote.Author}, {secondProjectNote.Date.Day} {secondProjectNote.Date:Y} at {secondProjectNote.Date:t}{secondProjectNote.Date.ToString("tt").ToLower()}";
			Document.QuerySelector("#project-note-date-1").TextContent.Trim().Should().Be(expectedSecondProjectNoteAuthorAndDate);
		}
	}
}
