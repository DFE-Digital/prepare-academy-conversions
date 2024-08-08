using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Dfe.PrepareConversions.Pages.Groups;

public class DoYouWantToAddConversionsModel : PageModel
{
   
   [BindProperty]
   public string Ukprn { get; set; }

   [BindProperty]
   public string AddConversion { get; set; }
   
   private readonly ErrorService _errorService;

   public DoYouWantToAddConversionsModel(ErrorService errorService)
   {
      _errorService = errorService;
   }

   public void OnGet(string ukprn)
   {
      Ukprn = ukprn;
      AddConversion = AddConversion ?? "yes";
   }


   public IActionResult OnPost(string ukprn)
   {

      if (AddConversion.IsNullOrEmpty())
      {
         ModelState.AddModelError("AddConversion", "Choose if any conversions need to be added to the group");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      if (AddConversion == "Yes")
      {
         return RedirectToPage(Links.ProjectGroups.SelectConversions.Page, new { ukprn });
      }


      return RedirectToPage(Links.ProjectGroups.CreateANewGroup.Page, new { ukprn });
   }
}