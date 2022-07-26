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
			ApprovedConditionsSet = GetDecisionFromSession()?.ApprovedConditionsSet ?? true;

			return Page();
		}

		public IActionResult OnPostAsync(int id)
		{
			var decision = GetDecisionFromSession();
			decision.ApprovedConditionsSet = ApprovedConditionsSet.Value;
			SetDecisionInSession(decision);

			return RedirectToPage(GetRedirectPageName(), new { id });
		}

		private string GetRedirectPageName()
		{
			if (ApprovedConditionsSet.Value)
			{
				return Links.Decision.WhatConditions.Page;
			}
			else return Links.Decision.ApprovalDate.Page;
		}
	}
}
