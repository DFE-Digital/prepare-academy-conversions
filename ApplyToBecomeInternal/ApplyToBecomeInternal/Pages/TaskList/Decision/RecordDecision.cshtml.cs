using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.Decision
{
	public class RecordDecisionModel : DecisionBaseModel
	{		

		public RecordDecisionModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{		
		}
					
		[BindProperty]
		public AdvisoryBoardDecisions AdvisoryBoardDecision { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			AdvisoryBoardDecision = GetDecisionFromSession()?.Decision ?? AdvisoryBoardDecisions.Approved;
			SetBackLinkModel(Links.TaskList.Index, id);

			return Page();
		}

		public IActionResult OnPostAsync(int id, [FromQuery(Name = "obl")] bool overideBackLink)
		{
			var decision = GetDecisionFromSession() ?? new AdvisoryBoardDecision();
			decision.Decision = AdvisoryBoardDecision;
			SetDecisionInSession(decision);

			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

			return RedirectToPage(Links.Decision.WhoDecided.Page, new { id });
		}
	}
}