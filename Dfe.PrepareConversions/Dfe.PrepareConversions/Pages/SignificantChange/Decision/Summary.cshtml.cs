using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision;

public class SummaryModel(ISignificantChangeProjectRepository repository,
                          ISession session) : SignificantChangeDecisionBaseModel(repository, session)
{
   public SignificantChangeDecision Decision { get; set; }

   public string DecisionText => Decision.Decision.ToDescription().ToLowerInvariant();

   public IActionResult OnGet(int id)
   {
      Decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(Decision, id);
      if (redirect != null) return redirect;

      SetBackLinkModel(Links.SignificantChangeDecision.DecisionDate, id);

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      SignificantChangeDecision decision = GetDecisionFromSession(id);

      IActionResult redirect = RedirectToStartIfNoDecision(decision, id);
      if (redirect != null) return redirect;

      decision.SignificantChangeProjectId = id;

      var savedDecisionResponse = await _repository.GetDecision(id);
      bool hasExistingDecision = false;

      if (savedDecisionResponse.Success && savedDecisionResponse.Body != null)
      {
         hasExistingDecision = true;
      }

      await CreateOrUpdateDecision(hasExistingDecision, decision);

      SetDecisionInSession(id, null);

      TempData.SetNotification(NotificationType.Success, "Done", "Decision recorded");

      return RedirectToPage(Links.SignificantChange.SignificantChangeTaskList.Page, new { id });
   }

   private async Task CreateOrUpdateDecision(bool hasExisting, SignificantChangeDecision newDecision)
   {
      if (!hasExisting)
      {
         await _repository.RecordDecision(newDecision);
      }
      else
      {
         await _repository.UpdateDecision(newDecision);
      }
   }
}