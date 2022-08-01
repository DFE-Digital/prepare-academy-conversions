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

		[BindProperty, Required(ErrorMessage = "Please enter the conditions for approval")] 
		public string ApprovedConditionsDetails { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			ApprovedConditionsDetails = GetDecisionFromSession(id)?.ApprovedConditionsDetails ?? string.Empty;
			SetBackLinkModel(Links.Decision.AnyConditions, id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id, [FromQuery(Name = "obl")] bool overideBackLink)
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(Request.Form.Keys, ModelState);
				return await OnGetAsync(id);
			}

			var decision = GetDecisionFromSession(id);
			decision.ApprovedConditionsDetails = ApprovedConditionsDetails;

			SetDecisionInSession(id, decision);

			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

			return RedirectToPage(Links.Decision.ApprovalDate.Page, new { id });
		}
	}
}
