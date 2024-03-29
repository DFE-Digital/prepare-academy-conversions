using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.ProjectNotes;

public class AddProjectNoteIntegrationTests : BaseIntegrationTests
{
   public AddProjectNoteIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_add_note_from_project_notes_and_back_to_project_notes()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetProjectNotes(project.Id);

      await OpenAndConfirmPathAsync($"/project-notes/{project.Id}");
      await NavigateAsync("Add note");

      Document.Url.Should().BeUrl($"/project-notes/{project.Id}/new-note");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/project-notes/{project.Id}");
   }

   [Fact]
   public async Task Should_add_new_note_and_redirect_to_project_notes()
   {
      AcademyConversionProject project = AddGetProject(x => x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary);

      string projectNotesPage = $"/project-notes/{project.Id}";

      DateTime expected = DateTime.Now.ToUkDateTime();
      DateTimeSource.UkTime = () => expected;

      AddProjectNote projectNote = new() { Subject = _fixture.Create<string>(), Note = _fixture.Create<string>(), Author = string.Empty, Date = expected };
      AddPostProjectNote(project.Id, projectNote);

      await OpenAndConfirmPathAsync(projectNotesPage);
      await NavigateAsync("Add note");
      
      Document.QuerySelector<IHtmlTextAreaElement>("#project-note-subject")!.Value = projectNote.Subject;
      Document.QuerySelector<IHtmlTextAreaElement>("#project-note-body")!.Value = projectNote.Note;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();
      Document.Url.Should().BeUrl(projectNotesPage);

      Document.QuerySelector("#project-note-added")!.TextContent.Should().NotBeNull();
   }

   [Fact]
   public async Task Should_not_add_note_when_both_subject_and_note_fields_are_null()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetProjectNotes(project.Id);

      AddProjectNote projectNote = new() { Subject = null, Note = null, Author = "" };
      AddPostProjectNote(project.Id, projectNote);

      await OpenAndConfirmPathAsync($"/project-notes/{project.Id}");
      await NavigateAsync("Add note");

      Document.QuerySelector<IHtmlTextAreaElement>("#project-note-subject")!.Value = null!;
      Document.QuerySelector<IHtmlTextAreaElement>("#project-note-body")!.Value = null!;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/project-notes/{project.Id}/new-note");
      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
      Document.QuerySelector(".govuk-error-summary")!.TextContent.Should().Contain("Enter");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddProjectNotePostError(project.Id);

      await OpenAndConfirmPathAsync($"/project-notes/{project.Id}/new-note");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }
}
