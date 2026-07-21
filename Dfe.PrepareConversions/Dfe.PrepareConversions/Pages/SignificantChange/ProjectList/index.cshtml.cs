using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.ProjectList;

public class IndexModel(ISignificantChangeProjectRepository significantChangeProjectRepository) : PageModel
{
   public ISignificantChangeProjectRepository SignificantChangeProjectRepository { get; } = significantChangeProjectRepository;
   public List<SignificantChangeProjectListViewModel> Projects { get; private set; } = [];

   public int ProjectCount => Projects.Count;

   public int TotalProjects => ProjectCount;
   
   public async Task OnGetAsync()
   {
      var response = await SignificantChangeProjectRepository.GetAllProjects(1, 100);
      Projects = response.Body?.Data.Select(SignificantChangeProjectListHelper.Build).ToList();
   }
}
