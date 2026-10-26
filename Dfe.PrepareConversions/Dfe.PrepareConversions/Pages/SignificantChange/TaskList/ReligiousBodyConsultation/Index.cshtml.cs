using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ReligiousBodyConsultation;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;
   private readonly ErrorService _errorService = errorService;

   [BindProperty]
   public YesNoNotApplicableAnswer? TrustConsultedReligiousBody { get; set; }

   [BindProperty]
   public string TrustConsultedReligiousBodyNotConsultedReason { get; set; }

   [BindProperty]
   public YesNoNotApplicableAnswer? ReligiousBodyRaisedObjection { get; set; }

   [BindProperty]
   public string ReligiousBodyRaisedObjectionReason { get; set; }

   [BindProperty]
   public YesNoNotApplicableAnswer? ReligiousBodyConsentGranted { get; set; }

   [BindProperty]
   public string ReligiousBodyConsentNotGrantedReason { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Religious bodies";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      TrustConsultedReligiousBody = Project.ReligiousBodyConsultedAnswer;
      TrustConsultedReligiousBodyNotConsultedReason = Project.ReligiousBodyConsultedReason;
      ReligiousBodyRaisedObjection = Project.ReligiousBodyRaisedObjection;
      ReligiousBodyRaisedObjectionReason = Project.ReligiousBodyRaisedObjectionReason;
      ReligiousBodyConsentGranted = Project.ReligiousBodyConsentGranted;
      ReligiousBodyConsentNotGrantedReason = Project.ReligiousBodyConsentNotGrantedReason;
      SupportingEvidence = Project.ReligiousBodySupportingEvidence;

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
               nameof(TrustConsultedReligiousBody),
               nameof(TrustConsultedReligiousBodyNotConsultedReason),
               nameof(ReligiousBodyRaisedObjection),
               nameof(ReligiousBodyRaisedObjectionReason),
               nameof(ReligiousBodyConsentGranted),
               nameof(ReligiousBodyConsentNotGrantedReason)
            ],
            ModelState);

         return Page();
      }

      SetSignificantChangeReligiousBodyConsultationCommand command = new(
         TrustConsultedReligiousBody,
         TrustConsultedReligiousBody == YesNoNotApplicableAnswer.No ? TrustConsultedReligiousBodyNotConsultedReason : null,
         ReligiousBodyRaisedObjection,
         ReligiousBodyRaisedObjection == YesNoNotApplicableAnswer.No ? ReligiousBodyRaisedObjectionReason : null,
         ReligiousBodyConsentGranted,
         ReligiousBodyConsentGranted == YesNoNotApplicableAnswer.No ? ReligiousBodyConsentNotGrantedReason : null,
         SupportingEvidence);

      await _repository.SetReligiousBodyConsultation(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      ValidateAnswer(TrustConsultedReligiousBody, nameof(TrustConsultedReligiousBody), TrustConsultedReligiousBodyNotConsultedReason, nameof(TrustConsultedReligiousBodyNotConsultedReason));
      ValidateAnswer(ReligiousBodyRaisedObjection, nameof(ReligiousBodyRaisedObjection), ReligiousBodyRaisedObjectionReason, nameof(ReligiousBodyRaisedObjectionReason));
      ValidateAnswer(ReligiousBodyConsentGranted, nameof(ReligiousBodyConsentGranted), ReligiousBodyConsentNotGrantedReason, nameof(ReligiousBodyConsentNotGrantedReason));
   }

   private void ValidateAnswer(YesNoNotApplicableAnswer? answer, string answerField, string reason, string reasonField)
   {
      if (!answer.HasValue)
         ModelState.AddModelError(answerField, "Select an option");

      if (answer == YesNoNotApplicableAnswer.No && string.IsNullOrWhiteSpace(reason))
         ModelState.AddModelError(reasonField, "Add a reason");
   }
}
