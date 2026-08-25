using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class WhyDAONotIssuedModel(
   IAcademyConversionProjectRepository repository,
   ISession session,
   ErrorService errorService)
   : DecisionBaseModel(repository, session)
{
   [BindProperty]
   public string SchoolWouldNotBeViableAsAnAcademyDetails { get; set; }

   [BindProperty]
   public bool SchoolWouldNotBeViableAsAnAcademyIsChecked { get; set; }

   [BindProperty]
   public string ThereAreNoSuitableTrustOptionsDetails { get; set; }

   [BindProperty]
   public bool ThereAreNoSuitableTrustOptionsIsChecked { get; set; }

   [BindProperty]
   public string SchoolAlreadyConvertingAndSufficientlyAdvancedDetails { get; set; }

   [BindProperty]
   public bool SchoolAlreadyConvertingAndSufficientlyAdvancedIsChecked { get; set; }

   [BindProperty]
   public string OtherDetails { get; set; }

   [BindProperty]
   public bool OtherIsChecked { get; set; }

   [BindProperty]
   public bool WasReasonGiven => SchoolWouldNotBeViableAsAnAcademyIsChecked
                                 || ThereAreNoSuitableTrustOptionsIsChecked
                                 || SchoolAlreadyConvertingAndSufficientlyAdvancedIsChecked
                                 || OtherIsChecked;

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.Decision.WhoDecided, id);

      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      List<AdvisoryBoardDAONotIssuedReasonDetails> reasons = decision.DAONotIssuedReasons;
      SetReasonsModel(reasons);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);

      decision.DAONotIssuedReasons.Clear();
      decision.DAONotIssuedReasons
         .AddReasonIfValid(SchoolWouldNotBeViableAsAnAcademyIsChecked,
            AdvisoryBoardDAONotIssuedReason.SchoolWouldNotBeViableAsAnAcademy,
            SchoolWouldNotBeViableAsAnAcademyDetails,
            ModelState)
         .AddReasonIfValid(ThereAreNoSuitableTrustOptionsIsChecked,
            AdvisoryBoardDAONotIssuedReason.ThereAreNoSuitableTrustOptions,
            ThereAreNoSuitableTrustOptionsDetails,
            ModelState)
         .AddReasonIfValid(SchoolAlreadyConvertingAndSufficientlyAdvancedIsChecked,
            AdvisoryBoardDAONotIssuedReason.SchoolAlreadyConvertingAndSufficientlyAdvanced,
            SchoolAlreadyConvertingAndSufficientlyAdvancedDetails,
            ModelState)
         .AddReasonIfValid(OtherIsChecked,
            AdvisoryBoardDAONotIssuedReason.Other,
            OtherDetails,
            ModelState);

      SetDecisionInSession(id, decision);

      if (!WasReasonGiven) ModelState.AddModelError("WasReasonGiven", "Select at least one reason");

      errorService.AddErrors(ModelState.Keys, ModelState);
      if (errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.Decision.DecisionMaker.Page, LinkParameters);
   }

   private void SetReasonsModel(List<AdvisoryBoardDAONotIssuedReasonDetails> reasons)
   {
      AdvisoryBoardDAONotIssuedReasonDetails SchoolWouldNotBeViableAsAnAcademy = reasons.GetReason(AdvisoryBoardDAONotIssuedReason.SchoolWouldNotBeViableAsAnAcademy);
      SchoolWouldNotBeViableAsAnAcademyIsChecked = SchoolWouldNotBeViableAsAnAcademy != null;
      SchoolWouldNotBeViableAsAnAcademyDetails = SchoolWouldNotBeViableAsAnAcademy?.Details;

      AdvisoryBoardDAONotIssuedReasonDetails noSuitableTrustOptions = reasons.GetReason(AdvisoryBoardDAONotIssuedReason.ThereAreNoSuitableTrustOptions);
      ThereAreNoSuitableTrustOptionsIsChecked = noSuitableTrustOptions != null;
      ThereAreNoSuitableTrustOptionsDetails = noSuitableTrustOptions?.Details;

      AdvisoryBoardDAONotIssuedReasonDetails advancedInConversionProcess = reasons.GetReason(AdvisoryBoardDAONotIssuedReason.SchoolAlreadyConvertingAndSufficientlyAdvanced);
      SchoolAlreadyConvertingAndSufficientlyAdvancedIsChecked = advancedInConversionProcess != null;
      SchoolAlreadyConvertingAndSufficientlyAdvancedDetails = advancedInConversionProcess?.Details;

      AdvisoryBoardDAONotIssuedReasonDetails other = reasons.GetReason(AdvisoryBoardDAONotIssuedReason.Other);
      OtherIsChecked = other != null;
      OtherDetails = other?.Details;
   }
}