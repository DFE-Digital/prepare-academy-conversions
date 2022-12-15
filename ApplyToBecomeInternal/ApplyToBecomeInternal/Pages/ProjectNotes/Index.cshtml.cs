using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectNotes;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   public IndexModel(IAcademyConversionProjectRepository repository) : base(repository)
   {
   }

   public IEnumerable<ProjectNoteViewModel> ProjectNotes =>
      Project.Notes?.Select(note => new ProjectNoteViewModel(note)) ?? Enumerable.Empty<ProjectNoteViewModel>();

   public bool NewNote { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      NewNote = (bool)(TempData["newNote"] ?? false);
      await base.OnGetAsync(id);

      return Page();
   }
}
