using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhatConditionsModel : DecisionBaseModel
	{
		public WhatConditionsModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{
		}

		[BindProperty, Required] public string ApprovedConditionsDetails { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			ApprovedConditionsDetails = GetDecisionFromSession(id)?.ApprovedConditionsDetails ?? string.Empty;
			SetBackLinkModel(Links.Decision.AnyConditions, id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id, [FromQuery(Name = "obl")] bool overideBackLink)
		{
			if (!ModelState.IsValid) return await OnGetAsync(id);

			var decision = GetDecisionFromSession(id);
			decision.ApprovedConditionsDetails = ApprovedConditionsDetails;

			SetDecisionInSession(id, decision);

			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

			return RedirectToPage(Links.Decision.ApprovalDate.Page, new { id });
		}
	}
}
