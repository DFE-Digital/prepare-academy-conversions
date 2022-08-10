using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.Decision
{
	public class RecordDecisionModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		public RecordDecisionModel(IAcademyConversionProjectRepository repository, ISession session, ErrorService errorService) 
			: base(repository, session)
		{
			_errorService = errorService;
		}
					
		[BindProperty, Required(ErrorMessage = "Please select the result of the decision")]
		public AdvisoryBoardDecisions? AdvisoryBoardDecision { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			AdvisoryBoardDecision = GetDecisionFromSession(id)?.Decision;
			SetBackLinkModel(Links.TaskList.Index, id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(new[] { "AdvisoryBoardDecision" }, ModelState);
				return await OnGetAsync(id);
			}

			var decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
			decision.Decision = AdvisoryBoardDecision.Value;
			SetDecisionInSession(id, decision);

			return RedirectToPage(Links.Decision.WhoDecided.Page, new { id });
		}
	}
}