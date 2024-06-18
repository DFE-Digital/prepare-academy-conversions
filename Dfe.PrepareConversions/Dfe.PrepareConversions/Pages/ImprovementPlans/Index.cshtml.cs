using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ImprovementPlans;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   public string ReturnPage { get; set; }
   public string ReturnId { get; set; }

   public IndexModel(IAcademyConversionProjectRepository repository) : base(repository)
   {
   }

   public IEnumerable<ProjectNoteViewModel> ProjectNotes =>
      Project.Notes?.OrderByDescending(x => x.Date).Select(note => new ProjectNoteViewModel(note)) ?? Enumerable.Empty<ProjectNoteViewModel>();

   public bool NewNote { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      NewNote = (bool)(TempData["newNote"] ?? false);

      ReturnPage = @Links.ProjectList.Index.Page;
      var returnToFormAMatMenu = TempData["returnToFormAMatMenu"] as bool?;

      if (Project.IsFormAMat && returnToFormAMatMenu.HasValue && returnToFormAMatMenu.Value)
      {
         ReturnId = Project.FormAMatProjectId.ToString();
         ReturnPage = @Links.FormAMat.OtherSchoolsInMat.Page;
         TempData["returnToFormAMatMenu"] = true;
      }

      return Page();
   }
}
