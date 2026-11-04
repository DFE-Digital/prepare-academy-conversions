using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.PublicSectorEqualityDuty;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   [BindProperty]
   public bool? EqualitiesImpactAssessmentCompleted { get; set; }

   [BindProperty]
   public string EqualitiesImpactAssessmentNoReason { get; set; }

   [BindProperty]
   public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }

   [BindProperty]
   public string LikelyDetails { get; set; }

   [BindProperty]
   public string SomeImpactDetails { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Public Sector Equality Duty";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      EqualitiesImpactAssessmentCompleted = Project.EqualitiesImpactAssessmentCompleted;
      EqualitiesImpactIdentified = Project.EqualitiesImpactIdentified;
      LikelyDetails = Project.EqualitiesLikelyDetails;
      SomeImpactDetails = Project.EqualitiesSomeImpactDetails;
      SupportingEvidence = Project.EqualitiesImpactSupportingEvidence;
      EqualitiesImpactAssessmentNoReason = Project.EqualitiesImpactAssessmentNoReason;

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
         errorService.AddErrors(
            [nameof(EqualitiesImpactIdentified), nameof(LikelyDetails), nameof(SomeImpactDetails), nameof(EqualitiesImpactAssessmentNoReason)],
            ModelState);

         return Page();
      }

      SetSignificantChangeEqualitiesImpactAssessmentCommand command = new(
         EqualitiesImpactAssessmentCompleted,
         EqualitiesImpactAssessmentCompleted == false ? EqualitiesImpactAssessmentNoReason : null,
         EqualitiesImpactIdentified,
         EqualitiesImpactIdentified == EqualitiesImpact.Likely ? LikelyDetails : null,
         EqualitiesImpactIdentified == EqualitiesImpact.SomeImpact ? SomeImpactDetails : null,
         SupportingEvidence);

      await _repository.SetEqualitiesImpactAssessment(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (EqualitiesImpactIdentified == EqualitiesImpact.Likely && string.IsNullOrWhiteSpace(LikelyDetails))
         ModelState.AddModelError(nameof(LikelyDetails), "Add additional information");

      if (EqualitiesImpactIdentified == EqualitiesImpact.SomeImpact && string.IsNullOrWhiteSpace(SomeImpactDetails))
         ModelState.AddModelError(nameof(SomeImpactDetails), "Add additional information");

      if (EqualitiesImpactAssessmentCompleted == false && string.IsNullOrWhiteSpace(EqualitiesImpactAssessmentNoReason))
         ModelState.AddModelError(nameof(EqualitiesImpactAssessmentNoReason), "Add additional information");
   }
}
