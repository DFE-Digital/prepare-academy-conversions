using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhyDeferredModel : DecisionBaseModel
	{
		public WhyDeferredModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{
		}

		[BindProperty, Required]
		public List<AdvisoryBoardDeferredReasons> DeferredReasons { get; set; }

		public IEnumerable<AdvisoryBoardDeferredReasons> DecisionMadeByOptions => Enum.GetValues(typeof(AdvisoryBoardDeferredReasons))
																                      .Cast<AdvisoryBoardDeferredReasons>();

		public async Task<IActionResult> OnGetAsync(int id)
		{

		    await SetDefaults(id);
			SetBackLinkModel(Links.Decision.WhoDecided, id);
			DeferredReasons = GetDecisionFromSession(id)?.DeferredReasons;

			return Page();
		}

		public IActionResult OnPostAsync(int id, List<AdvisoryBoardDeferredReasons> dvisoryBoardDeferredReasons, [FromQuery(Name = "obl")] bool overideBackLink)
		{
			var deferredReason = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
			deferredReason.DeferredReasons = DeferredReasons;

			SetDecisionInSession(id, deferredReason);

			if(ModelState.IsValid)
			{
			    if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

				//TODO: DeferredDate
				//return RedirectToPage(Links.Decision.DeferredDate.Page, new { id });
				return RedirectToPage(Links.Decision.WhyDeferred.Page, new { id });
			}

			return RedirectToPage(Links.Decision.WhyDeferred.Page, new { id });
		}
	}
}
