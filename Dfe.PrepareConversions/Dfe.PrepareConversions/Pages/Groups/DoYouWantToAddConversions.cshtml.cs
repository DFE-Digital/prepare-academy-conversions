using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.Groups;

public class DoYouWantToAddConversionsModel : PageModel
{
   public string Urn { get; set; }
   
   public void OnGet(string urn)
   {
      Urn = urn.ToString();
   }


   public async Task<IActionResult> OnPost()
   {
      return RedirectToPage(Links.Groups.CheckIncomingTrustsDetails.Page, new { Urn});
   }
}