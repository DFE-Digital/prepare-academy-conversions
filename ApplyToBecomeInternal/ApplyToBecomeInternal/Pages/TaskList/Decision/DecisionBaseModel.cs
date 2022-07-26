using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using static ApplyToBecomeInternal.Pages.TaskList.Decision.DecisionConstants;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public abstract class DecisionBaseModel : PageModel
	{
		protected readonly IAcademyConversionProjectRepository _repository;
		protected readonly ISession _session;

		public DecisionBaseModel(IAcademyConversionProjectRepository repository, ISession session)
		{
			_repository = repository;
			_session = session;
		}

		public string SchoolName { get; set; }
		public int Id { get; set; }

		public async Task SetDefaults(int id)
		{
			Id = id;			
			var project = await _repository.GetProjectById(id);
			SchoolName = project.Body.SchoolName;			
		}

		public AdvisoryBoardDecision GetDecisionFromSession()
		{
			return _session.Get<AdvisoryBoardDecision>(DECISION_SESSION_KEY) ?? new AdvisoryBoardDecision();
		}

		public void SetDecisionInSession(AdvisoryBoardDecision decision)
		{
			_session.Set(DECISION_SESSION_KEY, decision);
		}
	}
}
