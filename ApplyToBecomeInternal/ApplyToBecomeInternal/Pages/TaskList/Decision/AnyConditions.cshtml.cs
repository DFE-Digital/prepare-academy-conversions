using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class AnyConditionsModel : DecisionBaseModel
	{
		[BindProperty]
		public bool? ApprovedConditionsSet { get; set; }

		public AnyConditionsModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			SetBackLinkModel(Links.Decision.WhoDecided, id);
			ApprovedConditionsSet = GetDecisionFromSession(id)?.ApprovedConditionsSet ?? true;

			return Page();
		}

		public IActionResult OnPostAsync(int id, [FromQuery(Name = "obl")] bool overideBackLink)
		{			
			var decision = GetDecisionFromSession(id);
			decision.ApprovedConditionsSet = ApprovedConditionsSet.Value;

			if (!decision.ApprovedConditionsSet.Value) decision.ApprovedConditionsDetails = string.Empty;

			SetDecisionInSession(id, decision);
		
			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });

			return RedirectToPage(GetRedirectPageName(), new { id });
		}

		private string GetRedirectPageName()
		{
			return ApprovedConditionsSet switch
			{
				true => Links.Decision.WhatConditions.Page,
				_ => Links.Decision.ApprovalDate.Page
			};
		}
	}
}
