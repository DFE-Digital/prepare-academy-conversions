using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.AdmissionsVariationRecommendation;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService)
   : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   [BindProperty]
   public AdmissionsVariationRecommendationAnswer? AdmissionsVariationRecommendationAnswer { get; set; }

   [BindProperty]
   public string FurtherInformation { get; set; }

   protected override string TaskTitle => "Recommendation";

   public IEnumerable<AdmissionsVariationRecommendationAnswer> RecommendationOptions =>
      Enum.GetValues(typeof(AdmissionsVariationRecommendationAnswer))
         .Cast<AdmissionsVariationRecommendationAnswer>();

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      AdmissionsVariationRecommendationAnswer = Project.AdmissionsVariationRecommendationAnswer;
      FurtherInformation = Project.AdmissionsVariationRecommendationFurtherInformation;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Validate();

      if (!ModelState.IsValid)
      {
         errorService.AddErrors([nameof(AdmissionsVariationRecommendationAnswer)], ModelState);

         return Page();
      }

      SetAdmissionsVariationRecommendationCommand command = new(AdmissionsVariationRecommendationAnswer!.Value, FurtherInformation);

      await _repository.SetAdmissionsVariationRecommendation(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!AdmissionsVariationRecommendationAnswer.HasValue)
         ModelState.AddModelError(nameof(AdmissionsVariationRecommendationAnswer), "Select an option");
   }
}
