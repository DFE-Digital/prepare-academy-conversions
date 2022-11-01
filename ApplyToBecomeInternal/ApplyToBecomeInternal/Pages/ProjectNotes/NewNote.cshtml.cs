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
		private readonly IProjectNotesRepository _projectNotesRepository;

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

		public NewNoteModel(IProjectNotesRepository projectNotesRepository, ErrorService errorService, IAcademyConversionProjectRepository repository) : base(repository)
		{
			_projectNotesRepository = projectNotesRepository;
			_errorService = errorService;
		}

		[BindProperty(Name = "project-note-subject")]
		public string ProjectNoteSubject { get; set; }

		[BindProperty(Name = "project-note-body")]
		public string ProjectNoteBody { get; set; }

		public override async Task<IActionResult> OnPostAsync(int id)
		{
			var projectNote = new AddProjectNote {Subject = ProjectNoteSubject, Note = ProjectNoteBody, Author = ""};

			if (string.IsNullOrEmpty(ProjectNoteSubject) && string.IsNullOrEmpty(ProjectNoteBody))
			{
				_errorService.AddError("project-note-body", "Enter a subject and project note");
				await SetProject(id);
				return Page();
			}

			var response = await _projectNotesRepository.AddProjectNote(id, projectNote);
			if (!response.Success)
			{
				_errorService.AddTramsError();
				await SetProject(id);
				return Page();
			}

			TempData["newNote"] = true;

			return RedirectToPage(SuccessPage, new { id });
		}
	}
}
