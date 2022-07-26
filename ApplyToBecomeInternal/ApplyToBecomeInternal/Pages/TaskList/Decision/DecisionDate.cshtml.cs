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

		[ModelBinder(BinderType = typeof(DateInputModelBinder))]
		[Required, DateValidation(Services.DateRangeValidationService.DateRange.PastOrToday)] 
		public DateTime? DateOfDecision { get; set; }

		public string GetPageNameForBackLink()
		{
			var decision = GetDecisionFromSession();
			if (decision.ApprovedConditionsSet.HasValue && decision.ApprovedConditionsSet.Value)
			{
				return Links.Decision.WhatConditions.Page;
			}
			else return Links.Decision.AnyConditions.Page;			
		}						

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			DateOfDecision = GetDecisionFromSession()?.AdvisoryBoardDecisionDate;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid) return await OnGetAsync(id);

			var decision = GetDecisionFromSession();
			decision.AdvisoryBoardDecisionDate = DateOfDecision.Value;

			SetDecisionInSession(decision);

			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}
