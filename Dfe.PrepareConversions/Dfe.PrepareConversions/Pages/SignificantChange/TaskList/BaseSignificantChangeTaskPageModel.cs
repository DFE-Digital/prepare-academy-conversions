using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList;

public abstract class BaseSignificantChangeTaskPageModel(ISignificantChangeProjectRepository repository) : PageModel
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   public SignificantChangeProjectViewBaseModel Project { get; private set; }

   protected abstract string TaskTitle { get; }

   public virtual async Task<IActionResult> OnGetAsync(int id)
   {
      return await SetProjectAndMetadata(id);
   }

   protected async Task<IActionResult> SetProjectAndMetadata(int id)
   {
      var result = await _repository.GetProjectById(id);

      if (result.StatusCode == HttpStatusCode.NotFound || result.Body == null)
         return NotFound();

      Project = SignificantChangeProjectListHelper.Build(result.Body);

      ViewData["SchoolName"] = Project.SchoolName;
      ViewData["TaskTitle"] = TaskTitle;
      ViewData["Title"] = (ViewData.ModelState.IsValid ? string.Empty : "Error: ") + TaskTitle;

      return Page();
   }

   protected IActionResult RedirectToTaskList(int id)
   {
      return RedirectToPage(Links.SignificantChange.SignificantChangeTaskList.Page, new { id });
   }
}
