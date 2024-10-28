using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision
{
   public class IndexModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : BaseAcademyConversionProjectPageModel(repository)
   {
      public string ReturnPage { get; set; }
      public string ReturnId { get; set; }

      public async override Task<IActionResult> OnGetAsync(int id)
      {
         await base.OnGetAsync(id);         
         
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
}


