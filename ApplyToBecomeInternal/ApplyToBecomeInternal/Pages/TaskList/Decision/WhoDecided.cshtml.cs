using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using System.ComponentModel.DataAnnotations;
using ApplyToBecomeInternal.Services;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhoDecidedModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		public WhoDecidedModel(IAcademyConversionProjectRepository repository, ISession session,
			ErrorService errorService)
			: base(repository, session)
		{
			_errorService = errorService;
		}

		[BindProperty, Required(ErrorMessage = "Please select who made the decision")]
		public DecisionMadeBy? DecisionMadeBy { get; set; }

		public IEnumerable<DecisionMadeBy> DecisionMadeByOptions => Enum.GetValues(typeof(DecisionMadeBy))
																	.Cast<DecisionMadeBy>();

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			SetBackLinkModel(Links.Decision.RecordDecision, id);
			DecisionMadeBy = GetDecisionFromSession(id)?.DecisionMadeBy;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id, [FromQuery(Name = "obl")] bool overideBackLink)
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(new[] { "DecisionMadeBy" }, ModelState);
				return await OnGetAsync(id);
			}

			var decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
			decision.DecisionMadeBy = DecisionMadeBy;

			SetDecisionInSession(id, decision);

			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

			if (decision.Decision.ToString() == "Approved")
			{
				return RedirectToPage(Links.Decision.AnyConditions.Page, new { id });
			}
			else if (decision.Decision.ToString() == "Deferred")
			{
				return RedirectToPage(Links.Decision.WhyDeferred.Page, new { id });
			}
			else
			{
				return RedirectToPage(Links.Decision.RecordDecision.Page, new { id });
			}

		}		
	}
}
