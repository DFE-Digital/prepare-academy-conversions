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

         ValidateProject(id);

         return Page();
      }

      private void ValidateProject(int id)
      {
         if (!Project.HeadTeacherBoardDate.HasValue || Project.AssignedUser == null || Project.AssignedUser.EmailAddress.Length < 1)
         {
            ReturnPage = @Links.TaskList.Index.Page;
            if (!Project.HeadTeacherBoardDate.HasValue)
            { 
               errorService.AddError($"/task-list/{id}/confirm-school-trust-information-project-dates/advisory-board-date?return={ReturnPage}",
               "You must enter an advisory board date before you can record a decision.");
            }
            if (Project.AssignedUser == null || Project.AssignedUser.EmailAddress.Length < 1)
            {
               errorService.AddError($"/project-assignment/{id}",
               "You must enter the name of the person who worked on this project before you can record a decision.");
            }
         }
      }
   }
}


