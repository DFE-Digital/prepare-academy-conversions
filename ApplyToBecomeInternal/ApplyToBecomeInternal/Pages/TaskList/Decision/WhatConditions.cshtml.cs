using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhatConditionsModel : DecisionBaseModel
	{
		public WhatConditionsModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{
		}

		[BindProperty]
		public string ApprovedConditionsDetails { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			ApprovedConditionsDetails = GetDecisionFromSession()?.ApprovedConditionsDetails ?? string.Empty;

			return Page();
		}

		public IActionResult OnPostAsync(int id)
		{
			var decision = GetDecisionFromSession();
			decision.ApprovedConditionsDetails = ApprovedConditionsDetails;

			SetDecisionInSession(decision);

			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}
