using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.Groups;

public class DoYouWantToAddConversionsModel : PageModel
{
   
   
   
   [BindProperty]
   public string ReferenceNumber { get; set; }

   

   [BindProperty]
   public string AddConversion { get; set; }
   
   private readonly ErrorService _errorService;

   public DoYouWantToAddConversionsModel(ErrorService errorService)
   {
      _errorService = errorService;
   }

   public void OnGet(string referencenumber)
   {
      ReferenceNumber = referencenumber.ToString();
   }


   public async Task<IActionResult> OnPost(string referencenumber)
   {
      
      if (AddConversion.IsNullOrEmpty())
      {
         ModelState.AddModelError("AddConversion", "Choose if any conversions need to be added to the group");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      if (AddConversion == "Yes")
      {
         return RedirectToPage(Links.Groups.SelectConversions.Page, new { referencenumber});
      }
      

      return RedirectToPage(Links.Groups.CreateANewGroup.Page, new { referencenumber});
   }
}