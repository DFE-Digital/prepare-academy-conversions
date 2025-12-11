using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolAndTrustInformation;

public class ProjectRecommendationModel : UpdateAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public ProjectRecommendationModel(IAcademyConversionProjectRepository repository, ErrorService errorService)
      : base(repository, errorService)
   {
      _errorService = errorService;
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      if (string.IsNullOrWhiteSpace(AcademyConversionProject.RecommendationForProject))
      {
         ModelState.AddModelError("project-recommendation", "Select a recommendation for this project");
         _errorService.AddError("project-recommendation", "Select a recommendation for this project");
      }

      if (string.IsNullOrWhiteSpace(AcademyConversionProject.RecommendationNotesForProject))
      {
         ModelState.AddModelError("project-recommendation-notes", "Enter recommendation notes");
      }

      return await base.OnPostAsync(id);
   }
}

