using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.Decision
{
	public class RecordDecisionModel : DecisionBaseModel
	{		

		public RecordDecisionModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{		
		}
					
		[BindProperty, Required]
		public AdvisoryBoardDecisions? AdvisoryBoardDecision { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			AdvisoryBoardDecision = GetDecisionFromSession(id)?.Decision;
			SetBackLinkModel(Links.TaskList.Index, id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id, [FromQuery(Name = "obl")] bool overideBackLink)
		{
			if (!ModelState.IsValid) return await OnGetAsync(id);

			var decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
			decision.Decision = AdvisoryBoardDecision.Value;
			SetDecisionInSession(id, decision);

			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

			if(decision.Decision.ToString() == "Approved")
			{ 
				return RedirectToPage(Links.Decision.WhoDecided.Page, new { id });
			}
			//else if (decision.Decision.ToString() == "Declined")
			//{
			//	// TODO: Declined
			//}
			else if (decision.Decision.ToString() == "Deferred")
			{
				return RedirectToPage(Links.Decision.WhoDecidedDeferred.Page, new { id });
			}
			else
			{
				return RedirectToPage(Links.Decision.RecordDecision.Page, new { id });
			}
	        
		}
	}
}