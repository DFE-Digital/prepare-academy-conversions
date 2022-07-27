using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class DecisionDate : DecisionBaseModel
	{
		public DecisionDate(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{
		}

		[BindProperty, ModelBinder(BinderType = typeof(DateInputModelBinder))]
		[DateValidation(Services.DateRangeValidationService.DateRange.PastOrToday)]		
		[Required, Display(Name = "Decision date")]
		public DateTime? DateOfDecision { get; set; }

		public LinkItem GetPageForBackLink()
		{
			var decision = GetDecisionFromSession();

			return decision.ApprovedConditionsSet switch
			{
				true => Links.Decision.WhatConditions, 
				_ => Links.Decision.AnyConditions,
			};			
		}						

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			DateOfDecision = GetDecisionFromSession()?.AdvisoryBoardDecisionDate;
			SetBackLinkModel(GetPageForBackLink(), id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid) return await OnGetAsync(id);

			var decision = GetDecisionFromSession();
			decision.AdvisoryBoardDecisionDate = DateOfDecision.Value;

			SetDecisionInSession(decision);

			return RedirectToPage(Links.Decision.Summary.Page, new { id });
		}
	}
}
