using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
			DecisionMadeBy = GetDecisionFromSession()?.DecisionMadeBy ?? DecisionMadeBy.RegionalDirectorForRegion;

			return Page();
		}

		public IActionResult OnPostAsync(int id)
		{
			var decision = GetDecisionFromSession() ?? new AdvisoryBoardDecision();
			decision.DecisionMadeBy = DecisionMadeBy;

			SetDecisionInSession(decision);

			return RedirectToPage(Links.Decision.AnyConditions.Page, new { id });
		}		
	}
}
