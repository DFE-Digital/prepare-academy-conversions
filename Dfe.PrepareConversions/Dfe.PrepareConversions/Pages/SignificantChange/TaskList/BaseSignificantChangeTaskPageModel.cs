using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList;

public abstract class BaseSignificantChangeTaskPageModel(ISignificantChangeProjectRepository repository) : PageModel
{
   public SignificantChangeProjectViewBaseModel Project { get; private set; }

   public string ReturnPage => Request.Query.TryGetValue("return", out StringValues returnPage)
      ? returnPage.ToString()
      : null;

   protected abstract string TaskTitle { get; }

   public virtual async Task<IActionResult> OnGetAsync(int id)
   {
      return await SetProjectAndMetadata(id);
   }

   protected async Task<IActionResult> SetProjectAndMetadata(int id)
   {
      var result = await repository.GetProjectById(id);

      if (result.StatusCode == HttpStatusCode.NotFound || result.Body == null)
         return NotFound();

      Project = SignificantChangeProjectHelper.Build(result.Body);

      ViewData["SchoolName"] = Project.SchoolName;
      ViewData["TaskTitle"] = TaskTitle;
      ViewData["Title"] = (ViewData.ModelState.IsValid ? string.Empty : "Error: ") + TaskTitle;

      return Page();
   }

   protected IActionResult RedirectToTaskListOrReturnPage(int id)
   {
      if (ReturnPage != null)
      {
         return RedirectToPage(ReturnPage, null, new { id });
      }

      return RedirectToPage(Links.SignificantChange.SignificantChangeTaskList.Page, new { id });
   }
}
