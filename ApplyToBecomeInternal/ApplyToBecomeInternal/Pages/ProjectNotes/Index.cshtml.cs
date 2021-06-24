using ApplyToBecome.Data;
using ApplyToBecome.Data.Models.ProjectNotes;
using ApplyToBecome.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectNotes
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		private readonly IProjectNotes _projectNotes;

		public IndexModel(IAcademyConversionProjectRepository repository, IProjectNotes projectNotes) : base(repository)
		{
			_projectNotes = projectNotes;
		}

		public bool NewNote { get; set; }
		public IEnumerable<ProjectNote> Notes { get; set; }

		public override async Task<IActionResult> OnGetAsync(int id)
        {
			var result = await base.OnGetAsync(id);

			NewNote = (bool)(TempData["newNote"] ?? false);
			Notes = _projectNotes.GetNotesForProject(id);

			return result;
		}
    }
}
