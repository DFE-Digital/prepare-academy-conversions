using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Pages.SignificantChange.ProjectList;

public class IndexModel : PageModel
{
   public List<SignificantChangeProjectListViewModel> Projects { get; private set; } = [];

   public int ProjectCount => Projects.Count;

   public int TotalProjects => ProjectCount;

   public void OnGet()
   {
      Projects = [];
   }
}
