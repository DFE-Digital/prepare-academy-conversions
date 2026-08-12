using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;

public abstract class SignificantChangeDecisionBaseModel(ISignificantChangeProjectRepository repository, ISession session) : PageModel
{
   public const string DECISION_SESSION_KEY = "SignificantChangeDecision";

   protected readonly ISignificantChangeProjectRepository _repository = repository;
   protected readonly ISession _session = session;

   /// <summary>
   ///    Whether the <c>obl</c> query parameter should be passed on to subsequent pages
   /// </summary>
   protected bool PropagateBackLinkOverride = true;

   public BackLinkModel BackLinkModel { get; set; }
   public SignificantChangeProjectViewBaseModel Project { get; set; }
   public string SchoolName { get; set; }
   public int Id { get; set; }

   protected object LinkParameters =>
      PropagateBackLinkOverride && Request.Query.ContainsKey("obl")
         ? new { Id, obl = Request.Query["obl"] }
         : new { Id };

   public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
   {
      if (context.RouteData.Values.ContainsKey("id") &&
          int.TryParse(context.RouteData.Values["id"] as string, out int id))
      {
         ApiResponse<SignificantChangeProjectResponse> result = await _repository.GetProjectById(id);

         if (result.StatusCode == HttpStatusCode.NotFound || result.Body == null)
         {
            context.Result = new NotFoundResult();
            return;
         }

         Id = id;
         Project = SignificantChangeProjectListHelper.Build(result.Body);
         SchoolName = Project.SchoolName;
      }

      await next();
   }

   protected void SetBackLinkModel(LinkItem linkItem, int linkRouteId)
   {
      BackLinkModel = new BackLinkModel { LinkPage = linkItem.Page, LinkText = linkItem.BackText, LinkRouteId = linkRouteId };
   }

   protected SignificantChangeDecision GetDecisionFromSession(int id)
   {
      return _session.Get<SignificantChangeDecision>($"{DECISION_SESSION_KEY}_{id}") ?? new SignificantChangeDecision();
   }

   protected void SetDecisionInSession(int id, SignificantChangeDecision decision)
   {
      _session.Set($"{DECISION_SESSION_KEY}_{id}", decision);
   }

   /// <summary>
   ///    Guards against deep-linking into the middle of the wizard with an empty session.
   ///    Returns null when there is a decision in session and the page may render.
   /// </summary>
   protected IActionResult RedirectToStartIfNoDecision(SignificantChangeDecision decision, int id)
   {
      return decision?.Decision == null
         ? RedirectToPage(Links.SignificantChange.Decision.RecordDecision.Page, new { id })
         : null;
   }
}