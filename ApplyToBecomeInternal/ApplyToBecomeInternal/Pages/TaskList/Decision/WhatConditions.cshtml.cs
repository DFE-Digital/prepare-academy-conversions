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
			ApprovedConditionsDetails = GetDecisionFromSession()?.ApprovedConditionsDetails ?? string.Empty;
			SetBackLinkModel(Links.Decision.AnyConditions, id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid) return await OnGetAsync(id);

			var decision = GetDecisionFromSession();
			decision.ApprovedConditionsDetails = ApprovedConditionsDetails;

			SetDecisionInSession(decision);

			return RedirectToPage(Links.Decision.ApprovalDate.Page, new { id });
		}
	}
}
