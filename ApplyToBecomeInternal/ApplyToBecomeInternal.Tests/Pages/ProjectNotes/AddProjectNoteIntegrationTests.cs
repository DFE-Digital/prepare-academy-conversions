using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using AutoFixture;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.ProjectNotes
{
	public class AddProjectNoteIntegrationTests : BaseIntegrationTests
	{
		public AddProjectNoteIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_add_note_from_project_notes_and_back_to_project_notes()
		{
			var project = AddGetProject();
			AddGetProjectNotes(project.Id);

			await OpenUrlAsync($"/project-notes/{project.Id}");
			await NavigateAsync("Add note");

			Document.Url.Should().BeUrl($"/project-notes/{project.Id}/new-note");

			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/project-notes/{project.Id}");
		}

		[Fact]
		public async Task Should_add_new_note_and_redirect_to_project_notes()
		{
			var project = AddGetProject();
         string projectNotesPage = $"/project-notes/{project.Id}";

         DateTime expected = DateTime.UtcNow;
         DateTimeSource.UtcNow = () => expected;

         var projectNote = new AddProjectNote { Subject = _fixture.Create<string>(), Note = _fixture.Create<string>(), Author = string.Empty, Date = expected };
         AddPostProjectNote(project.Id, projectNote);

         await OpenUrlAsync(projectNotesPage);
			await NavigateAsync("Add note");

			Document.QuerySelector<IHtmlTextAreaElement>("#project-note-subject").Value = projectNote.Subject;
			Document.QuerySelector<IHtmlTextAreaElement>("#project-note-body").Value = projectNote.Note;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl(projectNotesPage);

			Document.QuerySelector("#project-note-added").TextContent.Should().NotBeNull();;
		}

		[Fact]
		public async Task Should_not_add_note_when_both_subject_and_note_fields_are_null()
		{
			var project = AddGetProject();
			AddGetProjectNotes(project.Id);

			var projectNote = new AddProjectNote {Subject = null, Note = null, Author = "" };
			AddPostProjectNote(project.Id, projectNote);

			await OpenUrlAsync($"/project-notes/{project.Id}");
			await NavigateAsync("Add note");

			Document.QuerySelector<IHtmlTextAreaElement>("#project-note-subject").Value = null;
			Document.QuerySelector<IHtmlTextAreaElement>("#project-note-body").Value = null;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/project-notes/{project.Id}/new-note");
			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Enter");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddProjectNotePostError(project.Id);

			await OpenUrlAsync($"/project-notes/{project.Id}/new-note");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}
	}
}
