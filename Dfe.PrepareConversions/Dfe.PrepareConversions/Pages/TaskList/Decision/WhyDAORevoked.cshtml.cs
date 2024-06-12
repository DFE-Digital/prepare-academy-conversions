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

public class WhyDAORevokedModel : DecisionBaseModel
{
   private readonly ErrorService _errorService;

   public WhyDAORevokedModel(IAcademyConversionProjectRepository repository,
                          ISession session,
                          ErrorService errorService
      )
     : base(repository, session)
   {
      _errorService = errorService;
   }


   [BindProperty]
   public string SchoolClosedOrClosingDetails { get; set; }

   [BindProperty]
   public bool SchoolClosedOrClosingIsChecked { get; set; }

   [BindProperty]
   public string SchoolRatedGoodOrOutstandingDetails { get; set; }

   [BindProperty]
   public bool SchoolRatedGoodOrOutstandingIsChecked { get; set; }

   [BindProperty]
   public string SafeguardingConcernsAddressedDetails { get; set; }

   [BindProperty]
   public bool SafeguardingConcernsAddressedIsChecked { get; set; }

   [BindProperty]
   public bool WasReasonGiven => SchoolClosedOrClosingIsChecked || SchoolRatedGoodOrOutstandingIsChecked || SafeguardingConcernsAddressedIsChecked;

   public string DecisionText { get; set; }

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.Decision.WhoDecided, id);

      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      DecisionText = decision.Decision.ToDescription().ToLowerInvariant();

      List<AdvisoryBoardDAORevokedReasonDetails> reasons = decision.DAORevokedReasons;
      SetReasonsModel(reasons);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);

      decision.DAORevokedReasons.Clear();
      decision.DAORevokedReasons
         .AddReasonIfValid(SchoolClosedOrClosingIsChecked, AdvisoryBoardDAORevokedReason.SchoolClosedOrClosing, SchoolClosedOrClosingDetails, ModelState)
         .AddReasonIfValid(SafeguardingConcernsAddressedIsChecked, AdvisoryBoardDAORevokedReason.SafeguardingConcernsAddressed, SafeguardingConcernsAddressedDetails, ModelState)
         .AddReasonIfValid(SchoolRatedGoodOrOutstandingIsChecked, AdvisoryBoardDAORevokedReason.SchoolRatedGoodOrOutstanding, SchoolRatedGoodOrOutstandingDetails, ModelState);

      SetDecisionInSession(id, decision);

      if (!WasReasonGiven) ModelState.AddModelError("WasReasonGiven", "Select at least one reason");

      _errorService.AddErrors(ModelState.Keys, ModelState);
      if (_errorService.HasErrors()) return OnGet(id);

      return RedirectToPage(Links.Decision.DecisionMaker.Page, LinkParameters);
   }

   private void SetReasonsModel(List<AdvisoryBoardDAORevokedReasonDetails> reasons)
   {
      AdvisoryBoardDAORevokedReasonDetails schoolClosedOrClosing = reasons.GetReason(AdvisoryBoardDAORevokedReason.SchoolClosedOrClosing);
      SchoolClosedOrClosingIsChecked = schoolClosedOrClosing != null;
      SchoolClosedOrClosingDetails = schoolClosedOrClosing?.Details;

      AdvisoryBoardDAORevokedReasonDetails safeguardingConcernsAddressed = reasons.GetReason(AdvisoryBoardDAORevokedReason.SafeguardingConcernsAddressed);
      SafeguardingConcernsAddressedIsChecked = safeguardingConcernsAddressed != null;
      SafeguardingConcernsAddressedDetails = safeguardingConcernsAddressed?.Details;

      AdvisoryBoardDAORevokedReasonDetails schoolRatedGoodOrOutstanding = reasons.GetReason(AdvisoryBoardDAORevokedReason.SchoolRatedGoodOrOutstanding);
      SchoolRatedGoodOrOutstandingIsChecked = schoolRatedGoodOrOutstanding != null;
      SchoolRatedGoodOrOutstandingDetails = schoolRatedGoodOrOutstanding?.Details;

   }
}
