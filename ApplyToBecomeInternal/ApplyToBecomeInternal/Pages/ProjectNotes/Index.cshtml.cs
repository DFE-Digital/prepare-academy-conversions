using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectNotes
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		private readonly IProjectNotesRepository _projectNotesRepository;		

		public IndexModel(IProjectNotesRepository projectNotesRepository, IAcademyConversionProjectRepository repository) : base(repository)
		{
			_projectNotesRepository = projectNotesRepository;
		}

		public IEnumerable<ProjectNoteViewModel> ProjectNotes { get; set; }
		public bool NewNote { get; set; }

		public override async Task<IActionResult> OnGetAsync(int id)
        {
	        NewNote = (bool)(TempData["newNote"] ?? false);
	        await base.OnGetAsync(id);
	        var response = await _projectNotesRepository.GetProjectNotesById(id);
	        ProjectNotes = response.Body.Select(pn => new ProjectNoteViewModel(pn));
	        return Page();
        }
	}
}
