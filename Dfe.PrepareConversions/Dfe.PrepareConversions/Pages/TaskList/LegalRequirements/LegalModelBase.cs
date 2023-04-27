using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AcademyConversion;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.LegalRequirements;

public class LegalModelBase : PageModel
{
   protected readonly IAcademyConversionProjectRepository AcademyConversionProjectRepository;

   public LegalModelBase(IAcademyConversionProjectRepository _academyConversionProjectRepository)
   {
      AcademyConversionProjectRepository = _academyConversionProjectRepository;
   }

   public int Id { get; private set; }
   public string SchoolName { get; private set; }
   public Data.Models.AcademyConversion.LegalRequirements Requirements { get; private set; }

   public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
   {
      if (context.HandlerArguments.ContainsKey(nameof(Id)))
      {
         Id = (int)context.HandlerArguments[nameof(Id)];

         ApiResponse<AcademyConversionProject> projectResponse = await AcademyConversionProjectRepository.GetProjectById(Id);
         if (projectResponse.Success)
         {
            SchoolName = projectResponse.Body.SchoolName;
         }
         else
         {
            context.Result = NotFound();
         }

         ApiResponse<AcademyConversionProject> project =
            await AcademyConversionProjectRepository.GetProjectById(Id);
         if (project.Success)
         {
            Requirements = Data.Models.AcademyConversion.LegalRequirements.From(project.Body);
         }
         else
         {
            context.Result = NotFound();
         }

         if (context.Result == default)
            await next();
      }

      context.Result ??= NotFound();
   }

   protected (string, string, string) GetReturnPageAndFragment()
   {
      Request.Query.TryGetValue("return", out StringValues returnQuery);
      Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
      Request.Query.TryGetValue("back", out StringValues backQuery);
      return (returnQuery, fragmentQuery, backQuery);
   }

   protected IActionResult ActionResult(int id, string fragment, string back)
   {
      (string returnPage, fragment, back) = GetReturnPageAndFragment();
      if (string.IsNullOrWhiteSpace(returnPage) is false)
      {
         return !string.IsNullOrEmpty(back)
            ? RedirectToPage(returnPage, null,
               new { id, @return = back, back }, fragment)
            : RedirectToPage(returnPage, null, new { id }, fragment);
      }

      return RedirectToPage(Links.LegalRequirements.Summary.Page, new { id });
   }

   protected ThreeOptions? ToLegalRequirementsEnum(ThreeOptions? requirements, string approved)
   {
      ThreeOptions? result = approved switch
      {
         nameof(ThreeOptions.Yes) => ThreeOptions.Yes,
         nameof(ThreeOptions.No) => ThreeOptions.No,
         nameof(ThreeOptions.NotApplicable) => ThreeOptions.NotApplicable,
         _ => requirements
      };
      return result;
   }
}
