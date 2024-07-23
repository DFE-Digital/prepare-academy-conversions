using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.Groups;

public class CheckIncomingTrustsDetailsModel : PageModel
{
   
   private readonly ITrustsRepository _trustRepository;

   public string Urn { get; set; }

   public TrustDto Trust { get; set; } = null;

   public CheckIncomingTrustsDetailsModel(ITrustsRepository trustRepository)
   {
      _trustRepository = trustRepository;
   }
   
   public async Task OnGet(string urn, string ukprn)
   {
      if (!string.IsNullOrEmpty(ukprn))
      {
         Trust = (await _trustRepository.SearchTrusts(ukprn)).Data.FirstOrDefault();
      }
   }
   
   public async Task<IActionResult> OnPost(string urn)
   {
      return RedirectToPage(Links.Groups.DoYouWantToAddConversions.Page, new { urn});
   }
}