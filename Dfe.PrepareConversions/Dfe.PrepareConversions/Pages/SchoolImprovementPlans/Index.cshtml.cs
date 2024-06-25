using Dfe.PrepareConversions.Data.Models.SchoolImprovementPlans;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ImprovementPlans;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   public string ReturnPage { get; set; }
   public string ReturnId { get; set; }

   public IndexModel(IAcademyConversionProjectRepository repository) : base(repository)
   {
   }

   //[BindProperty]
   public IEnumerable<SchoolImprovementPlan> SchoolImprovementPlans { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      ////NewNote = (bool)(TempData["newNote"] ?? false);
      // initialize plans
      SchoolImprovementPlans = new List<SchoolImprovementPlan>();
      var schoolImprovementPlansResponse = await _repository.GetSchoolImprovementPlansForProject(id).ConfigureAwait(false);

      if (schoolImprovementPlansResponse.Success)
      {
         SchoolImprovementPlans = schoolImprovementPlansResponse.Body.OrderByDescending(x => x.CreatedDate);
      }

      ReturnPage = @Links.ProjectList.Index.Page;
      var returnToFormAMatMenu = TempData["returnToFormAMatMenu"] as bool?;

      if (Project.IsFormAMat && returnToFormAMatMenu.HasValue && returnToFormAMatMenu.Value)
      {
         ReturnId = Project.FormAMatProjectId.ToString();
         ReturnPage = @Links.FormAMat.OtherSchoolsInMat.Page;
         TempData["returnToFormAMatMenu"] = true;
      }

      return Page();
   }
}
