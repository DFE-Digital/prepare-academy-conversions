using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class DeclineReasonModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;
		public DeclineReasonModel(ErrorService errorService, IAcademyConversionProjectRepository repository, ISession session) 
			: base(repository, session)
		{
			_errorService = errorService;
			DeclineOptions = Enum.GetValues(typeof(AdvisoryBoardDeclinedReasons)).Cast<AdvisoryBoardDeclinedReasons>();
		}

		[BindProperty] public IEnumerable<string> DeclinedReasons { get; set; }
		[BindProperty] public string DeclineOtherReason { get; set; }
		[BindProperty] public string DeclineFinanceReason { get; set; }
		[BindProperty] public string DeclinePerformanceReason { get; set; }
		[BindProperty] public string DeclineGovernanceReason { get; set; }
		[BindProperty] public string DeclineChoiceOfTrustReason { get; set; }

		public IEnumerable<AdvisoryBoardDeclinedReasons> DeclineOptions { get; }
		public string DecisionText { get; set; }

		public UIHelpers UI => new UIHelpers(this);

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			PreloadStateFromSession(id);
			SetBackLinkModel(Links.Decision.WhoDecided, id);

			return Page();
		}		

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var decision = GetDecisionFromSession(id);

			if (DeclinedReasons.Any())
			{
				var reasons = DeclinedReasons.Select(Enum.Parse<AdvisoryBoardDeclinedReasons>);
				decision.DeclinedReasons.Clear();
				AddReason(decision.DeclinedReasons, MapReason(reasons, AdvisoryBoardDeclinedReasons.Finance));
				AddReason(decision.DeclinedReasons, MapReason(reasons, AdvisoryBoardDeclinedReasons.Performance));
				AddReason(decision.DeclinedReasons, MapReason(reasons, AdvisoryBoardDeclinedReasons.Governance));
				AddReason(decision.DeclinedReasons, MapReason(reasons, AdvisoryBoardDeclinedReasons.ChoiceOfTrust));
				AddReason(decision.DeclinedReasons, MapReason(reasons, AdvisoryBoardDeclinedReasons.Other));
			}
			else
			{
				_errorService.AddError("DeclinedReasonSet", "Select at least one reason");
			}

			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.Finance, DeclineFinanceReason);
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.Performance, DeclinePerformanceReason);
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.Governance, DeclineGovernanceReason);
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.ChoiceOfTrust, DeclineChoiceOfTrustReason);
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.Other, DeclineOtherReason);

			SetDecisionInSession(id, decision);

			if (ModelState.IsValid) return RedirectToPage(Links.Decision.DecisionDate.Page, new { id });

			_errorService.AddErrors(ModelState.Keys, ModelState);
			return await OnGetAsync(id);
		}

		private void PreloadStateFromSession(int id)
		{
			AdvisoryBoardDecision decision = GetDecisionFromSession(id);
			DecisionText = decision.Decision.ToDescription().ToLowerInvariant();

			var reasons = decision.DeclinedReasons?.ToDictionary(key => key.Reason, value => value.Details);

			DeclinedReasons = reasons.Select(r => r.Key.ToString());
			DeclineOtherReason = reasons.GetValueOrDefault(AdvisoryBoardDeclinedReasons.Other);
			DeclineFinanceReason = reasons.GetValueOrDefault(AdvisoryBoardDeclinedReasons.Finance);
			DeclinePerformanceReason = reasons.GetValueOrDefault(AdvisoryBoardDeclinedReasons.Performance);
			DeclineGovernanceReason = reasons.GetValueOrDefault(AdvisoryBoardDeclinedReasons.Governance);
			DeclineChoiceOfTrustReason = reasons.GetValueOrDefault(AdvisoryBoardDeclinedReasons.ChoiceOfTrust);
		}

		private void AddReason(List<AdvisoryBoardDeclinedReasonDetails> reasons, AdvisoryBoardDeclinedReasonDetails reason)
		{
			if (reason == null) return;

			reasons.Add(reason);
		}

		private AdvisoryBoardDeclinedReasonDetails MapReason(IEnumerable<AdvisoryBoardDeclinedReasons> reasons, AdvisoryBoardDeclinedReasons reason)
		{
			if (!DeclinedReasons.Contains(reason.ToString())) return null;	

			return reason switch
			{
				AdvisoryBoardDeclinedReasons.Finance => new AdvisoryBoardDeclinedReasonDetails(reason, DeclineFinanceReason),
				AdvisoryBoardDeclinedReasons.Performance => new AdvisoryBoardDeclinedReasonDetails(reason, DeclinePerformanceReason),
				AdvisoryBoardDeclinedReasons.Governance => new AdvisoryBoardDeclinedReasonDetails(reason, DeclineGovernanceReason),
				AdvisoryBoardDeclinedReasons.ChoiceOfTrust => new AdvisoryBoardDeclinedReasonDetails(reason, DeclineChoiceOfTrustReason),
				AdvisoryBoardDeclinedReasons.Other => new AdvisoryBoardDeclinedReasonDetails(reason, DeclineOtherReason),
				_ => throw new ArgumentOutOfRangeException(nameof(reason), reason, "Unexpected value for AdvisoryBoardDeclinedReasons")
			};				
		}

		private void EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons reason, string explanation)
		{
			string reasonName = reason.ToString();

			if (DeclinedReasons.Contains(reasonName) && string.IsNullOrWhiteSpace(explanation))
				ModelState.AddModelError($"Decline{reasonName}Reason", $"Enter a reason for selecting {reason.ToDescription()}");
		}

		public class UIHelpers
		{
			private readonly DeclineReasonModel _model;

			public UIHelpers(DeclineReasonModel model) => _model = model;

			public bool IsChecked(AdvisoryBoardDeclinedReasons reason)
			{
				return _model.DeclinedReasons.Contains(reason.ToString());
			}

			public string IdFor(string prefix, object item)
			{
				string connector = prefix.EndsWith('-') ? string.Empty : "-";
				string suffix = item?.ToString().ToLowerInvariant();

				return $"{prefix}{connector}{suffix}";
			}

			public string ValueFor(object item)
			{
				return item.ToString();
			}

			public string ReasonFieldFor(object item)
			{
				return $"Decline{item}Reason";
			}

			public string ReasonValueFor(AdvisoryBoardDeclinedReasons reason)
			{
				return reason switch
				{
					AdvisoryBoardDeclinedReasons.Finance => _model.DeclineFinanceReason,
					AdvisoryBoardDeclinedReasons.Performance => _model.DeclinePerformanceReason,
					AdvisoryBoardDeclinedReasons.Governance => _model.DeclineGovernanceReason,
					AdvisoryBoardDeclinedReasons.ChoiceOfTrust => _model.DeclineChoiceOfTrustReason,
					AdvisoryBoardDeclinedReasons.Other => _model.DeclineOtherReason,
					_ => string.Empty
				};
			}
		}
	}
}