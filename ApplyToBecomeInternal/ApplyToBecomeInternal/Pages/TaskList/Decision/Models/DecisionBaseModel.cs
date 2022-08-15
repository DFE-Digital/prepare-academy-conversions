using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList.Decision;
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
		protected AcademyConversionProject _project;

		public DecisionBaseModel(IAcademyConversionProjectRepository repository, ISession session)
		{
			_repository = repository;
			_session = session;
		}

		public BackLinkModel BackLinkModel { get; set; }
		public string SchoolName { get; set; }
		public int Id { get; set; }

		protected async Task SetDefaults(int id)
		{
			Id = id;
			var project = await _repository.GetProjectById(id);
			SchoolName = project.Body.SchoolName;
		}

		protected void SetBackLinkModel(LinkItem linkItem, int linkRouteId)
		{
			BackLinkModel = new BackLinkModel { LinkPage = linkItem.Page, LinkText = linkItem.BackText, LinkRouteId = linkRouteId };
		}


		/// <summary>
		/// Returns the active <see cref="AdvisoryBoardDecision"/> from the current session or a new instance if one is not available
		/// </summary>
		/// <param name="id">The ID of the <see cref="AdvisoryBoardDecision"/> to retrieve.</param>
		/// <returns>Either the <see cref="AdvisoryBoardDecision"/> instance currently stored in the session, or a new instance.</returns>
		/// <remarks>
		/// <p>If the session does not contain an instance of <see cref="AdvisoryBoardDecision"/> this call will create a new instance but will not store it in the session.</p>
		/// <p>The ID parameter becomes part of the Session key with a prefix of <code>DECISION_SESSION_KEY_</code></p>
		/// </remarks>
		protected AdvisoryBoardDecision GetDecisionFromSession(int id)
		{
			return _session.Get<AdvisoryBoardDecision>($"{DECISION_SESSION_KEY}_{id}") ?? new AdvisoryBoardDecision();
		}

		/// <summary>
		/// Stores the provided <see cref="AdvisoryBoardDecision"/> in the current session.
		/// </summary>
		/// <param name="id">The ID of the <see cref="AdvisoryBoardDecision"/> to store in the session</param>
		/// <param name="decision">An instance of <see cref="AdvisoryBoardDecision"/> to be persisted</param>
		protected void SetDecisionInSession(int id, AdvisoryBoardDecision decision)
		{
			_session.Set($"{DECISION_SESSION_KEY}_{id}", decision);
		}
	}
}
