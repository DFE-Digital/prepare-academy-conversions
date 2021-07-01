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
		private readonly IProjectNotesService _service;

		public IEnumerable<ProjectNoteViewModel> ProjectNotes;

		public IndexModel(IProjectNotesService service, IAcademyConversionProjectRepository repository) : base(repository)
		{
			_service = service;
		}

		public bool NewNote { get; set; }

		public override async Task<IActionResult> OnGetAsync(int id)
        {
	        NewNote = (bool)(TempData["newNote"] ?? false);
	        await base.OnGetAsync(id);
	        var response = await _service.GetProjectNotesById(Project.AcademyConversionProjectId);
	        ProjectNotes = response.Body.Select(pn => new ProjectNoteViewModel(pn));
	        return Page();
        }
	}
}
