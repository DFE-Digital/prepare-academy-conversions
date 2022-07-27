using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class SummaryModel : DecisionBaseModel
	{
		public SummaryModel(IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{
		}

		public AdvisoryBoardDecision Decision { get; set; }

		public string GetDecisionAsFriendlyName()
		{
			if (Decision.Decision == AdvisoryBoardDecisions.Approved && Decision.ApprovedConditionsSet.Value)
			{
				return "APPROVED WITH CONDITIONS";
			}

			return Decision.Decision.ToString().ToUpper();
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			Decision = GetDecisionFromSession();

			return Page();
		}
	}
}
