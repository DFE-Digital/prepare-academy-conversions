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
   public bool? TrustConsultedStakeholders { get; set; }

   [BindProperty]
   public string TrustConsultedStakeholdersNotConsultedReason { get; set; }

   [BindProperty]
   public bool? WereThereAnyObjectionsRaised { get; set; }

   [BindProperty]
   public string StakeholderObjectionsDetails { get; set; }

   [BindProperty]
   public YesNoNotApplicableAnswer? RelevantStakeholdersNotifiedOfAdmissionsChanges { get; set; }

   [BindProperty]
   public string RelevantStakeholdersNotifiedOfAdmissionsChangesReason { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Other stakeholder engagement";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      TrustConsultedStakeholders = Project.OtherStakeholderEngagementTrustConsultedStakeholders;
      TrustConsultedStakeholdersNotConsultedReason = Project.OtherStakeholderEngagementTrustConsultedStakeholdersNotConsultedReason;
      WereThereAnyObjectionsRaised = Project.WereThereAnyObjectionsRaised;
      StakeholderObjectionsDetails = Project.StakeholderObjectionsDetails;
      RelevantStakeholdersNotifiedOfAdmissionsChanges = Project.RelevantStakeholdersNotifiedOfAdmissionsChanges;
      RelevantStakeholdersNotifiedOfAdmissionsChangesReason = Project.RelevantStakeholdersNotifiedOfAdmissionsChangesReason;
      SupportingEvidence = Project.OtherStakeholderEngagementSupportingEvidence;

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
            [
               nameof(TrustConsultedStakeholders),
               nameof(TrustConsultedStakeholdersNotConsultedReason),
               nameof(WereThereAnyObjectionsRaised),
               nameof(StakeholderObjectionsDetails),
               nameof(RelevantStakeholdersNotifiedOfAdmissionsChanges),
               nameof(RelevantStakeholdersNotifiedOfAdmissionsChangesReason)
            ],
            ModelState);

         return Page();
      }

      SetSignificantChangeOtherStakeholderEngagementCommand command = new(
         TrustConsultedStakeholders,
         TrustConsultedStakeholders == false ? TrustConsultedStakeholdersNotConsultedReason : null,
         WereThereAnyObjectionsRaised,
         WereThereAnyObjectionsRaised == true ? StakeholderObjectionsDetails : null,
         RelevantStakeholdersNotifiedOfAdmissionsChanges,
         RelevantStakeholdersNotifiedOfAdmissionsChanges == YesNoNotApplicableAnswer.No
            ? RelevantStakeholdersNotifiedOfAdmissionsChangesReason
            : null,
         SupportingEvidence);

      await _repository.SetOtherStakeholderEngagement(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!TrustConsultedStakeholders.HasValue)
         ModelState.AddModelError(nameof(TrustConsultedStakeholders), "Select an option");

      if (TrustConsultedStakeholders == false && string.IsNullOrWhiteSpace(TrustConsultedStakeholdersNotConsultedReason))
         ModelState.AddModelError(nameof(TrustConsultedStakeholdersNotConsultedReason), "Add a reason");

      if (!WereThereAnyObjectionsRaised.HasValue)
         ModelState.AddModelError(nameof(WereThereAnyObjectionsRaised), "Select an option");

      if (WereThereAnyObjectionsRaised == true && string.IsNullOrWhiteSpace(StakeholderObjectionsDetails))
         ModelState.AddModelError(nameof(StakeholderObjectionsDetails), "Add details");

      if (!RelevantStakeholdersNotifiedOfAdmissionsChanges.HasValue)
         ModelState.AddModelError(nameof(RelevantStakeholdersNotifiedOfAdmissionsChanges), "Select an option");

      if (RelevantStakeholdersNotifiedOfAdmissionsChanges == YesNoNotApplicableAnswer.No
          && string.IsNullOrWhiteSpace(RelevantStakeholdersNotifiedOfAdmissionsChangesReason))
         ModelState.AddModelError(nameof(RelevantStakeholdersNotifiedOfAdmissionsChangesReason), "Add a reason");
   }
}