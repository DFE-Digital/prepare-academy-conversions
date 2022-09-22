using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList.Decision.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class RecordDecisionModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		public RecordDecisionModel(IAcademyConversionProjectRepository repository, ISession session, ErrorService errorService)
			: base(repository, session)
		{
			_errorService = errorService;
			PropagateBackLinkOverride = false;
		}

		[BindProperty, Required(ErrorMessage = "Select a decision")]
		public AdvisoryBoardDecisions? AdvisoryBoardDecision { get; set; }

		public IActionResult OnGet(int id)
		{
			AdvisoryBoardDecision = GetDecisionFromSession(id)?.Decision;
			SetBackLinkModel(Links.TaskList.Index, id);

			return Page();
		}

		public IActionResult OnPost(int id)
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(new[] { "AdvisoryBoardDecision" }, ModelState);
				return OnGet(id);
			}

			var decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
			decision.Decision = AdvisoryBoardDecision.Value;
			SetDecisionInSession(id, decision);

			return RedirectToPage(Links.Decision.WhoDecided.Page, LinkParameters);
		}
	}
}