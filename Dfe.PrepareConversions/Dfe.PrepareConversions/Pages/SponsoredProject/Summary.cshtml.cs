using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Models.SponsoredProject;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Mappings;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class SummaryModel : PageModel
{
   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;
   private readonly IGetEstablishment _getEstablishment;
   private readonly ITrustsRepository _trustRepository;

   public SummaryModel(IGetEstablishment getEstablishment,
                       ITrustsRepository trustRepository,
                       IAcademyConversionProjectRepository academyConversionProjectRepository)
   {
      _getEstablishment = getEstablishment;
      _trustRepository = trustRepository;
      _academyConversionProjectRepository = academyConversionProjectRepository;
   }

   public EstablishmentResponse Establishment { get; set; }
   public TrustSummary Trust { get; set; }


   public async Task<IActionResult> OnGetAsync(string urn, string ukprn)
   {
      Establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      Trust = (await _trustRepository.SearchTrusts(ukprn)).Data.FirstOrDefault();

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(string urn, string ukprn)
   {
      EstablishmentResponse establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      TrustDetail trust = await _trustRepository.GetTrustByUkprn(ukprn);

      await _academyConversionProjectRepository.CreateSponsoredProject(CreateSponsoredProjectMapper.MapToDto(establishment, trust));

      return RedirectToPage(Links.ProjectList.Index.Page);
   }
}