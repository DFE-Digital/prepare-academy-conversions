using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data.Services;
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

   public string Ukprn { get; set; }
   
   public bool HasConversions { get; set; }

   public TrustDto Trust { get; set; } = null;
   
   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;

   public CheckIncomingTrustsDetailsModel(ITrustsRepository trustRepository,IAcademyConversionProjectRepository academyConversionProjectRepository)
   {
      _trustRepository = trustRepository;
      _academyConversionProjectRepository = academyConversionProjectRepository;
   }
   
   public async Task OnGet(string ukprn)
   {
      if (!string.IsNullOrEmpty(ukprn))
      {
         Trust = await _trustRepository.GetTrustByUkprn(ukprn);
      }
      
      var projects = await _academyConversionProjectRepository.GetProjectsForGroup(Trust.ReferenceNumber);

      HasConversions = projects.Body.Count() == 0 ? false : true;
   }
   
   public async Task<IActionResult> OnPost(string ukprn)
   {
      return RedirectToPage(Links.ProjectGroups.DoYouWantToAddConversions.Page, new { ukprn });
   }
}