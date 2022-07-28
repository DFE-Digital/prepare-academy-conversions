using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision.Declined
{
	public class DeclinedReasonModel : DecisionBaseModel
    {
		public DeclinedReasonModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{
		}

		[BindProperty, Required] public List<AdvisoryBoardDeclinedReasons> DeclinedReasons { get; set; } 
		[BindProperty, Required] public string DeclinedOtherReasons { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			var decision = GetDecisionFromSession();
			DeclinedReasons = decision.DeclinedReasons;
			DeclinedOtherReasons = decision.DeclinedOtherReason;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id, [FromQuery(Name = "obl")] bool overideBackLink)
		{
			if (!ModelState.IsValid) return await OnGetAsync(id);

			var decision = GetDecisionFromSession();
			decision.DeclinedReasons = decision.DeclinedReasons;
			decision.DeclinedOtherReason = decision.DeclinedOtherReason;

			SetDecisionInSession(decision);

			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}