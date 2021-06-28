using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectNotes
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		public IndexModel(IAcademyConversionProjectRepository repository) : base(repository)
		{
		}

		public bool NewNote { get; set; }

		public override async Task<IActionResult> OnGetAsync(int id)
        {
	        NewNote = (bool)(TempData["newNote"] ?? false);
	        return await base.OnGetAsync(id);
		}
    }
}
