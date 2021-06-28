using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectNotes
{
	public class NewNoteModel : UpdateAcademyConversionProjectPageModel
	{
		public NewNoteModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository, errorService)
		{
		}

		public override async Task<IActionResult> OnPostAsync(int id)
		{
			TempData["newNote"] = true;
			return await base.OnPostAsync(id);
		}
	}
}
