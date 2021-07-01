using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectNotes
{
	public class NewNoteModel : BaseAcademyConversionProjectPageModel
	{
		private readonly ErrorService _errorService;
		private readonly IProjectNotesService _service;

		public bool ShowError => _errorService.HasErrors();

		public string SuccessPage
		{
			get
			{
				return TempData[nameof(SuccessPage)].ToString();
			}
			set
			{
				TempData[nameof(SuccessPage)] = value;
			}
		}

		public NewNoteModel(IProjectNotesService service, ErrorService errorService, IAcademyConversionProjectRepository repository) : base(repository)
		{
			_service = service;
			_errorService = errorService;
		}

		[BindProperty(Name = "project-note-subject")]
		public string ProjectNoteSubject { get; set; }

		[BindProperty(Name = "project-note-body")]
		public string ProjectNoteBody { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			TempData["newNote"] = true;

			var projectNote = new AddProjectNote {Subject = ProjectNoteSubject, Note = ProjectNoteBody, Author = ""};

			if (string.IsNullOrEmpty(ProjectNoteSubject) && string.IsNullOrEmpty(ProjectNoteBody))
			{
				_errorService.AddError("project-note-body", "Enter a project note");
				await SetProject(id);
				return Page();
			}

			var response = await _service.AddProjectNote(id, projectNote);
			if (!response.Success)
			{
				_errorService.AddTramsError();
				await SetProject(id);
				return Page();
			}

			return RedirectToPage(SuccessPage, new { id });
		}
	}
}
