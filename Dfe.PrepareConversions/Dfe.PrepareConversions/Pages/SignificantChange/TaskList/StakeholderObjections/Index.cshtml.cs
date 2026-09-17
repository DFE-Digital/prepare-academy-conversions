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
   public SignificantChangeStakeholderObjection? StakeholderObjections { get; set; }

   [BindProperty]
   public string StakeholderObjectionsComment { get; set; }
   
   protected override string TaskTitle => "Stakeholder objections";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      StakeholderObjections = Project.StakeholderObjections;
      StakeholderObjectionsComment = Project.StakeholderObjectionsComment;

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
            [nameof(StakeholderObjections), nameof(StakeholderObjectionsComment)],
            ModelState);

         return Page();
      }

      SetSignificantChangeStakeholderObjectionsCommand command = new(
         StakeholderObjections!.Value,
         StakeholderObjectionsComment);

        await _repository.SetStakeholderObjections(id, command);

        return RedirectToTaskList(id);
    }

    public void Validate()
    {
        if (StakeholderObjections is null)
        {
            ModelState.AddModelError(nameof(StakeholderObjections), "Select whether there are any stakeholder objections");
        }

        if (StakeholderObjections == SignificantChangeStakeholderObjection.YesNoFurtherInformationProvided && string.IsNullOrWhiteSpace(StakeholderObjectionsComment))
        {
            ModelState.AddModelError(nameof(StakeholderObjectionsComment), "Enter the additional information provided");
        }
    }
}