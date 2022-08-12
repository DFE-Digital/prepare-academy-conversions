using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class AnyConditionsModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		[BindProperty, Required(ErrorMessage = "Please choose an option")]
		public bool? ApprovedConditionsSet { get; set; }

		public AnyConditionsModel(IAcademyConversionProjectRepository repository, ISession session, 
			ErrorService errorService) 
			: base(repository, session)
		{
			_errorService = errorService;
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			SetBackLinkModel(Links.Decision.WhoDecided, id);
			ApprovedConditionsSet = GetDecisionFromSession(id).ApprovedConditionsSet;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(new[] { "ApprovedConditionsSet" }, ModelState);
				return await OnGetAsync(id);
			}

			var decision = GetDecisionFromSession(id);
			decision.ApprovedConditionsSet = ApprovedConditionsSet.Value;
			
			ClearConditionDetailsIfAppropriate(decision);

			SetDecisionInSession(id, decision);

			return RedirectToPage(GetRedirectPageName(), new { id });
		}

		private static void ClearConditionDetailsIfAppropriate(AdvisoryBoardDecision decision)
		{
			if (!decision.ApprovedConditionsSet.Value) decision.ApprovedConditionsDetails = string.Empty;
		}

		private string GetRedirectPageName()
		{
			return ApprovedConditionsSet switch
			{
				true => Links.Decision.WhatConditions.Page,
				_ => Links.Decision.DecisionDate.Page
			};
		}
	}
}
