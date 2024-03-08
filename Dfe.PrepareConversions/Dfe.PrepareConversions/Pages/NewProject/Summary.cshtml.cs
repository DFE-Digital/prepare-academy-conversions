using Dfe.Academies.Contracts.V4.Trusts;
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

   public Academies.Contracts.V4.Establishments.EstablishmentDto Establishment { get; set; }
   public TrustDto Trust { get; set; } = null;
   public string HasSchoolApplied { get; set; }
   public string HasPreferredTrust { get; set; }
   public string ProposedTrustName { get; set; }
   public string IsFormAMat { get; set; }


   public async Task<IActionResult> OnGetAsync(string urn, string ukprn, string hasSchoolApplied, string hasPreferredTrust, string proposedTrustName, string isFormAMat)
   {
      Establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      if (!string.IsNullOrEmpty(ukprn))
      {
         Trust = (await _trustRepository.SearchTrusts(ukprn)).Data.FirstOrDefault();
      }

      HasSchoolApplied = hasSchoolApplied;
      HasPreferredTrust = hasPreferredTrust;
      // Default to no as it's most common
      IsFormAMat = isFormAMat ?? "no";
      ProposedTrustName = proposedTrustName ?? null;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(string urn, string ukprn, string hasSchoolApplied, string hasPreferredTrust, string proposedTrustName)
   {
      Academies.Contracts.V4.Establishments.EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);

      TrustDto trust = new TrustDto();
      if (ukprn != null)
      {
         trust = await _trustRepository.GetTrustByUkprn(ukprn);
      }
      if (proposedTrustName != null)
      {
         trust.Name = proposedTrustName;
      }
      if (proposedTrustName != null)
      {
         await _academyConversionProjectRepository.CreateFormAMatProject(CreateProjectMapper.MapFormAMatToDto(establishment, trust, hasSchoolApplied, hasPreferredTrust));
      }
      else
      {
         await _academyConversionProjectRepository.CreateProject(CreateProjectMapper.MapToDto(establishment, trust, hasSchoolApplied, hasPreferredTrust));
      }


      return RedirectToPage(Links.ProjectList.Index.Page);
   }
   public static string DetermineBackLink(bool hasSchoolApplied, bool hasPreferredTrust, bool hasProposedTrustName)
   {
      return hasProposedTrustName switch
      {
         true => Links.NewProject.CreateNewFormAMat.Page,
         _ when hasSchoolApplied => Links.NewProject.SearchTrusts.Page,
         _ when hasPreferredTrust => Links.NewProject.PreferredTrust.Page,
         _ => Links.NewProject.SearchTrusts.Page,
      };
   }

}
