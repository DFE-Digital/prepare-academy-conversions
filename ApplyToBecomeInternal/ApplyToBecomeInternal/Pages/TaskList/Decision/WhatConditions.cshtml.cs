using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhatConditionsModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		public WhatConditionsModel(IAcademyConversionProjectRepository repository, ISession session,
			ErrorService errorService) 
			: base(repository, session)
		{
			_errorService = errorService;
		}

		[BindProperty, Required(ErrorMessage = "Add the conditions that were set")] 
		public string ApprovedConditionsDetails { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			ApprovedConditionsDetails = GetDecisionFromSession(id)?.ApprovedConditionsDetails ?? string.Empty;
			SetBackLinkModel(Links.Decision.AnyConditions, id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(Request.Form.Keys, ModelState);
				return await OnGetAsync(id);
			}

			var decision = GetDecisionFromSession(id);
			decision.ApprovedConditionsDetails = ApprovedConditionsDetails;

			SetDecisionInSession(id, decision);

			return RedirectToPage(Links.Decision.DecisionDate.Page, new { id });
		}
	}
}
