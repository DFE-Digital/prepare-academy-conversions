using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
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
   public BannerModel Banner { get; set; }

   public async Task<IActionResult> OnGetAsync(int id)
   {
      var result = await _repository.GetProjectById(id);

      if (result.StatusCode == HttpStatusCode.NotFound)
      {
         return NotFound();
      }

      Project = SignificantChangeProjectHelper.Build(result.Body);
      TaskList = SignificantChangeTaskListBuilder.Build(Project);

      if (Project.Decision is SignificantChangeDecisions.Approved)
      {
         Banner = new BannerModel(
               NotificationType.Warning.ToString(),
               "Important",
               [$"This project was approved on {Project.DecisionDate.ToDateString()}.", "You cannot make changes to this project in Prepare."],
               "Project approved"
         );
      }

      return Page();
   }
}
