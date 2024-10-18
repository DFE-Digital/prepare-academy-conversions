using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision.Models;

public abstract class DecisionBaseModel(IAcademyConversionProjectRepository repository, ISession session) : PageModel
{
   public const string DECISION_SESSION_KEY = "Decision";
   public const string PROJECT_READONLY_SESSION_KEY = "Project_iro";
   protected readonly IAcademyConversionProjectRepository _repository = repository;
   protected readonly ISession _session = session;
   protected AcademyConversionProject _project;

   /// <summary>
   ///    Whether the <c>obl</c> query parameter should be passed on to subsequent pages
   /// </summary>
   protected bool PropagateBackLinkOverride = true;

   public BackLinkModel BackLinkModel { get; set; }
   public string SchoolName { get; set; }
   public string AcademyTypeAndRoute { get; set; }
   public int Id { get; set; }

   protected object LinkParameters =>
      PropagateBackLinkOverride && Request.Query.ContainsKey("obl")
         ? new { Id, obl = Request.Query["obl"] }
         : new { Id };

   private async Task SetDefaults(int id)
   {
      Id = id;
      var project = await _repository.GetProjectById(id);
      SchoolName = project.Body.SchoolName;
      AcademyTypeAndRoute = project.Body.AcademyTypeAndRoute;
   }

   public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
   {
      if (context.RouteData.Values.ContainsKey("id") &&
          int.TryParse(context.RouteData.Values["id"] as string, out int id))
      {
         await SetDefaults(id);
      }

      await next();
   }

   protected void SetBackLinkModel(LinkItem linkItem, int linkRouteId)
   {
      BackLinkModel = new BackLinkModel { LinkPage = linkItem.Page, LinkText = linkItem.BackText, LinkRouteId = linkRouteId };
   }

   /// <summary>
   ///    Returns the active <see cref="AdvisoryBoardDecision" /> from the current session or a new instance if one is not available
   /// </summary>
   /// <param name="id">The ID of the <see cref="AdvisoryBoardDecision" /> to retrieve.</param>
   /// <returns>Either the <see cref="AdvisoryBoardDecision" /> instance currently stored in the session, or a new instance.</returns>
   /// <remarks>
   ///    <p>If the session does not contain an instance of <see cref="AdvisoryBoardDecision" /> this call will create a new instance but will not store it in the session.</p>
   ///    <p>The ID parameter becomes part of the Session key with a prefix of <code>DECISION_SESSION_KEY_</code></p>
   /// </remarks>
   protected AdvisoryBoardDecision GetDecisionFromSession(int id)
   {
      return _session.Get<AdvisoryBoardDecision>($"{DECISION_SESSION_KEY}_{id}") ?? new AdvisoryBoardDecision();
   }

   protected bool GetIsProjectReadOnly(int id)
   {
      _ = bool.TryParse(_session.GetString($"{PROJECT_READONLY_SESSION_KEY}_{id}"), out bool isReadOnly);
      return isReadOnly;
   }

   /// <summary>
   ///    Stores the provided <see cref="AdvisoryBoardDecision" /> in the current session.
   /// </summary>
   /// <param name="id">The ID of the <see cref="AdvisoryBoardDecision" /> to store in the session</param>
   /// <param name="decision">An instance of <see cref="AdvisoryBoardDecision" /> to be persisted</param>
   protected void SetDecisionInSession(int id, AdvisoryBoardDecision decision)
   {
      _session.Set($"{DECISION_SESSION_KEY}_{id}", decision);
   }
   public bool IsVoluntaryAcademyType(string AcademyTypeAndRoute)
   {
      // Convert the input to lowercase for case-insensitive comparison
      string lowerCaseType = AcademyTypeAndRoute.ToLower();

      // Define an array of valid academy types and routes
      string[] validTypes = { "converter", "form a mat" };

      // Check if the lowerCaseType is in the array of validTypes
      foreach (string validType in validTypes)
      {
         if (lowerCaseType == validType)
         {
            return true; // Match found = Voluntary
         }
      }

      return false; // No match found = Sponsored
   }
}
