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
   public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }

   [BindProperty]
   public string EqualitiesImpactIdentifiedMitigation { get; set; }

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
      EqualitiesImpactIdentifiedMitigation = Project.EqualitiesImpactIdentifiedMitigation;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      SetSignificantChangeEqualitiesImpactAssessmentCommand command = new(
         EqualitiesImpactAssessmentCompleted,
         EqualitiesImpactIdentified,
         EqualitiesImpactIdentified == EqualitiesImpact.ImpactsIdentified ? EqualitiesImpactIdentifiedMitigation : null);

      await _repository.SetEqualitiesImpactAssessment(id, command);

      return RedirectToTaskList(id);
   }
}
