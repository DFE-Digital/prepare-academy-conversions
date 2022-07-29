using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhoDecidedModel : DecisionBaseModel
	{		
		public WhoDecidedModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{			
		}

		[BindProperty]
		public DecisionMadeBy DecisionMadeBy { get; set; }

		public IEnumerable<DecisionMadeBy> DecisionMadeByOptions => Enum.GetValues(DecisionMadeBy.GetType())
																	.Cast<DecisionMadeBy>();
																	
		public async Task<IActionResult> OnGet(int id)
		{
			await SetDefaults(id);
			SetBackLinkModel(Links.Decision.RecordDecision, id);
			DecisionMadeBy = GetDecisionFromSession(id)?.DecisionMadeBy ?? DecisionMadeBy.RegionalDirectorForRegion;

			return Page();
		}

		public IActionResult OnPostAsync(int id, [FromQuery(Name = "obl")] bool overideBackLink)
		{
			var decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
			decision.DecisionMadeBy = DecisionMadeBy;

			SetDecisionInSession(id, decision);

			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

			return RedirectToPage(Links.Decision.AnyConditions.Page, new { id });
		}		
	}
}
