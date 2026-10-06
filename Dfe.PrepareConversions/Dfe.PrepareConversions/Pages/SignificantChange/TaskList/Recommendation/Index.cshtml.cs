using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Recommendation;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;
   private readonly ErrorService _errorService = errorService;

   [BindProperty]
   public string MoreInformation { get; set; }

   [BindProperty]
   [Required(ErrorMessage = "Select a recommendation")]
   public SignificantChangeRecommendation? Recommendation { get; set; }

   public IEnumerable<SignificantChangeRecommendation> RecommendationOptions =>
      Enum.GetValues<SignificantChangeRecommendation>();

   protected override string TaskTitle => "Significant change recommendation";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Recommendation = Project.Recommendation;
      MoreInformation = Project.RecommendationMoreInformation;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(["Recommendation"], ModelState);
         return await OnGetAsync(id);
      }

      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      SetSignificantChangeRecommendationCommand command = new(
         Recommendation.Value,
         MoreInformation);

      await _repository.SetRecommendation(id, command);

      return RedirectToTaskList(id);
   }
}