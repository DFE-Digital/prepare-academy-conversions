using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList;

public class IndexModel(ISignificantChangeProjectRepository repository) : PageModel
{
   protected readonly ISignificantChangeProjectRepository _repository = repository;

   public SignificantChangeProjectViewBaseModel Project { get; set; }
   public SignificantChangeTaskListViewModel TaskList { get; set; }

   public async Task<IActionResult> OnGetAsync(int id)
   {
      var result = await _repository.GetProjectById(id);
      if (result.StatusCode == HttpStatusCode.NotFound)
      {
         return NotFound();
      }

      Project = SignificantChangeProjectListHelper.Build(result.Body);
      TaskList = SignificantChangeTaskListBuilder.Build(Project);
      return Page();
   }
}
