using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ProjectNotes;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   public IndexModel(IAcademyConversionProjectRepository repository) : base(repository)
   {
   }

   public IEnumerable<ProjectNoteViewModel> ProjectNotes =>
      Project.Notes?.OrderByDescending(x => x.Date).Select(note => new ProjectNoteViewModel(note)) ?? Enumerable.Empty<ProjectNoteViewModel>();

   public bool NewNote { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      NewNote = (bool)(TempData["newNote"] ?? false);
      await base.OnGetAsync(id);

      return Page();
   }
}