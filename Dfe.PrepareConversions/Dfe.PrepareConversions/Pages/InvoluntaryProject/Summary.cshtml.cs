using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Models.InvoluntaryProject;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.InvoluntaryProject;

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

      await _academyConversionProjectRepository.CreateInvoluntaryProject(MapToDto(establishment, trust));

      return RedirectToPage(Links.ProjectList.Index.Page);
   }

   private static CreateInvoluntaryProject MapToDto(EstablishmentResponse establishment, TrustDetail trust)
   {
      InvoluntaryProjectSchool createSchool = new(
         establishment.EstablishmentName,
         establishment.Urn,
         establishment.ViewAcademyConversion?.Pfi != null && establishment.ViewAcademyConversion?.Pfi != "No");

      InvoluntaryProjectTrust createTrust = new(
         trust.GiasData.GroupName,
         trust.GiasData.GroupId);

      return new CreateInvoluntaryProject(createSchool, createTrust);
   }
}
