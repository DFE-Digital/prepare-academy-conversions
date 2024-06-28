using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ImprovementPlans;

public abstract class SchoolImprovementPlanBaseModel : PageModel
{
   public const string SESSION_KEY = "SchoolImprovementPlan";
   protected readonly IAcademyConversionProjectRepository _repository;
   protected readonly ISession _session;
   protected AcademyConversionProject _project;

   /// <summary>
   ///    Whether the <c>obl</c> query parameter should be passed on to subsequent pages
   /// </summary>
   protected bool PropagateBackLinkOverride = true;

   protected SchoolImprovementPlanBaseModel(IAcademyConversionProjectRepository repository, ISession session)
   {
      _repository = repository;
      _session = session;
   }

   public BackLinkModel BackLinkModel { get; set; }
   public string SchoolName { get; set; }
   public string AcademyTypeAndRoute { get; set; }
   public DateTime? AdvisoryBoradDate { get; set; }
   public DateTime? ConversionDate { get; set; }
   public int Id { get; set; }
   public int? SipId { get; set; }


   public SchoolImprovementPlan SchoolImprovementPlan { get; set; }

   protected object LinkParameters
   {
      get
      {
         if (PropagateBackLinkOverride && Request.Query.ContainsKey("obl"))
         {
            return SipId.HasValue ? new { Id, SipId, obl = Request.Query["obl"] } : new { Id, obl = Request.Query["obl"] };
         }
          

          return new { Id, SipId };
      }
   }

   private async Task SetDefaults(int id, int? sipId = null)
   {
      Id = id;
      SipId = sipId;
      ApiResponse<AcademyConversionProject> project = await _repository.GetProjectById(id);
      SchoolName = project.Body.SchoolName;
      AcademyTypeAndRoute = project.Body.AcademyTypeAndRoute;
      AdvisoryBoradDate = project.Body.HeadTeacherBoardDate;
      ConversionDate = project.Body.ProposedAcademyOpeningDate;
   }

   public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
   {
      int? sipId = null;

      if (context.RouteData.Values.ContainsKey("sipid") &&
         int.TryParse(context.RouteData.Values["sipid"] as string, out int routeSipId))
         sipId = routeSipId;

      if (context.RouteData.Values.ContainsKey("id") &&
          int.TryParse(context.RouteData.Values["id"] as string, out int id))
         await SetDefaults(id, sipId);


      await next();
   }

   protected void SetBackLinkModel(LinkItem linkItem, int linkRouteId)
   {
      BackLinkModel = new BackLinkModel { LinkPage = linkItem.Page, LinkText = linkItem.BackText, LinkRouteId = linkRouteId };
   }

   /// <summary>
   ///    Returns the active <see cref="SchoolImprovementPlan" /> from the current session or a new instance if one is not available
   /// </summary>
   /// <param name="id">The ID of the <see cref="SchoolImprovementPlan" /> to retrieve.</param>
   /// <returns>Either the <see cref="SchoolImprovementPlan" /> instance currently stored in the session, or a new instance.</returns>
   /// <remarks>
   ///    <p>If the session does not contain an instance of <see cref="SchoolImprovementPlan" /> this call will create a new instance but will not store it in the session.</p>
   ///    <p>The ID parameter becomes part of the Session key with a prefix of <code>DECISION_SESSION_KEY_</code></p>
   /// </remarks>
   protected SchoolImprovementPlan GetSchoolImprovementPlanFromSession(int id)
   {
      return _session.Get<SchoolImprovementPlan>($"{SESSION_KEY}_{id}") ?? new SchoolImprovementPlan();
   }

   /// <summary>
   ///    Stores the provided <see cref="SchoolImprovementPlan" /> in the current session.
   /// </summary>
   /// <param name="id">The ID of the <see cref="SchoolImprovementPlan" /> to store in the session</param>
   /// <param name="schoolImprovementPlan">An instance of <see cref="SchoolImprovementPlan" /> to be persisted</param>
   protected void SetSchoolImprovementPlanInSession(int id, SchoolImprovementPlan schoolImprovementPlan)
   {
      _session.Set($"{SESSION_KEY}_{id}", schoolImprovementPlan);
   }
   public bool IsVoluntaryAcademyType(string AcademyTypeAndRoute)
   {
      // Convert the input to lowercase for case-insensitive comparison
      string lowerCaseType = AcademyTypeAndRoute.ToLower();

      // Define an array of valid academy types and routes
      string[] validTypes = { "converter", "form a mat" };

      // Check if the lowerCaseType is in the array of validTypes
      foreach (string validType in validTypes)
         if (lowerCaseType == validType)
            return true; // Match found = Voluntary

      return false; // No match found = Sponsored
   }

   public virtual async Task<IActionResult> OnGetAsync(int id, int? sipId = null)
   {
      SchoolImprovementPlan improvementPlan = GetSchoolImprovementPlanFromSession(id);

      if (improvementPlan.Id == 0 && sipId.HasValue)
      {
         ApiResponse<IEnumerable<SchoolImprovementPlan>> schoolImprovementPlansResponse = await _repository.GetSchoolImprovementPlansForProject(id).ConfigureAwait(false);
         if (schoolImprovementPlansResponse.Success)
         {
            SchoolImprovementPlan = schoolImprovementPlansResponse.Body.SingleOrDefault(x => x.Id == sipId);
         }
         SetSchoolImprovementPlanInSession(id, SchoolImprovementPlan);
      }
      else SchoolImprovementPlan = improvementPlan;


      return Page();
   }
}
