using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.StakeholderObjections;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;
   private readonly ErrorService _errorService = errorService;

   [BindProperty]
   public SignificantChangeStakeholderObjection? StakeholderObjection { get; set; }

   [BindProperty]
   public string StakeHolderObjectionsAdditionalComments { get; set; }

   protected override string TaskTitle => "Stakeholder objections";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      StakeholderObjection = Project.StakeholderObjection;
      StakeHolderObjectionsAdditionalComments = Project.StakeHolderObjectionsAdditionalComments;

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
         _errorService.AddErrors(
            [nameof(StakeholderObjection), nameof(StakeHolderObjectionsAdditionalComments)],
            ModelState);

         return Page();
      }

      SetSignificantChangeStakeholderObjectionsCommand command = new(
         StakeholderObjection!.Value,
         StakeHolderObjectionsAdditionalComments);

        await _repository.SetStakeholderObjections(id, command);

        return RedirectToTaskList(id);
    }

    public void Validate()
    {
        if (StakeholderObjection is null)
        {
            ModelState.AddModelError(nameof(StakeholderObjection), "Select whether there are any stakeholder objections");
        }

        if (StakeholderObjection == SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided && string.IsNullOrWhiteSpace(StakeHolderObjectionsAdditionalComments))
        {
            ModelState.AddModelError(nameof(StakeHolderObjectionsAdditionalComments), "Enter the additional information provided");
        }
    }
}