using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class AnyConditionsModel : DecisionBaseModel
    {				
		[BindProperty]
		public bool? AnyConditions { get; set; }

		public AnyConditionsModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{				
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			AnyConditions = GetDecisionFromSession()?.AnyConditions ?? true;			

			return Page();
		}

		public IActionResult OnPostAsync(int id)
		{
			var decision = GetDecisionFromSession() ?? new AdvisoryBoardDecision();
			decision.AnyConditions = AnyConditions.Value;
			SetDecisionInSession(decision);

			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}
