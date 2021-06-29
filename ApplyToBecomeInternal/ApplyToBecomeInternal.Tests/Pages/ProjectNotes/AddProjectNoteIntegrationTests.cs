using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using AutoFixture;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.ProjectNotes
{
	public class AddProjectNoteIntegrationTests : BaseIntegrationTests
	{
		public AddProjectNoteIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_add_note_from_project_notes_and_back_to_project_list()
		{
			var project = AddGetProject();

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

			var projectNote = new UpdateProjectNote {Subject = _fixture.Create<string>(), Note = _fixture.Create<string>(), Author = "" };
			AddPatchProject(project, r => r.ProjectNote, projectNote);

			await OpenUrlAsync($"/project-notes/{project.Id}");
			await NavigateAsync("Add note");

			Document.QuerySelector<IHtmlTextAreaElement>("#project-note-subject").Value = projectNote.Subject;
			Document.QuerySelector<IHtmlTextAreaElement>("#project-note-body").Value = projectNote.Note;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/project-notes/{project.Id}");
		}

		[Fact]
		public async Task Should_send_note_to_api_as_null_when_both_subject_and_note_fields_are_null()
		{
			var project = AddGetProject();

			AddPatchProject(project, r => r.ProjectNote, null);

			await OpenUrlAsync($"/project-notes/{project.Id}");
			await NavigateAsync("Add note");

			Document.QuerySelector<IHtmlTextAreaElement>("#project-note-subject").Value = null;
			Document.QuerySelector<IHtmlTextAreaElement>("#project-note-body").Value = null;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/project-notes/{project.Id}");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}
	}
}
