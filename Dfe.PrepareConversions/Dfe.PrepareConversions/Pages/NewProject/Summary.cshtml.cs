using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data.Models;
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
   public string IsProjectInPrepare { get; set; }
   public string IsProjectAlreadyInPrepare { get; set; }
   public string ApplicationReference { get; set; }


   public async Task<IActionResult> OnGetAsync(string urn, string ukprn, string hasSchoolApplied, string hasPreferredTrust, string proposedTrustName, string isFormAMat, string isProjectInPrepare, string applicationReference)
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
      IsProjectInPrepare = isProjectInPrepare ?? "no";
      ProposedTrustName = proposedTrustName ?? null;
      ApplicationReference = applicationReference ?? null;

      if (ApplicationReference != null)
      {
         var results = await _academyConversionProjectRepository.SearchFormAMatProjects(ApplicationReference);
         ProposedTrustName = results.Body.First().ProposedTrustName;
      }

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(string urn, string ukprn, string hasSchoolApplied, string hasPreferredTrust, string proposedTrustName, string isFormAMat, string applicationReference)
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
         await _academyConversionProjectRepository.CreateFormAMatProject(CreateProjectMapper.MapFormAMatToDto(establishment, trust, hasSchoolApplied, hasPreferredTrust));
      }

      bool _isFormAMAT = !string.IsNullOrEmpty(isFormAMat) && isFormAMat.ToLower().Equals("yes");

      if (_isFormAMAT && proposedTrustName == null)
      {
         var createdProject = await _academyConversionProjectRepository.CreateProject(CreateProjectMapper.MapToDto(establishment, trust, hasSchoolApplied, hasPreferredTrust, true));
         var formAMatProject = await _academyConversionProjectRepository.SearchFormAMatProjects(applicationReference);

         int projectId = createdProject.Body.Id;
         var formAMatProjectID = formAMatProject.Body.First().Id;

         await _academyConversionProjectRepository.SetFormAMatProjectReference(projectId, new SetFormAMatProjectReference(projectId, formAMatProjectID));
      }
      else
      {
         await _academyConversionProjectRepository.CreateProject(CreateProjectMapper.MapToDto(establishment, trust, hasSchoolApplied, hasPreferredTrust));
      }


      return RedirectToPage(Links.ProjectList.Index.Page);
   }
   /// <summary>
   /// Determines the appropriate back link based on the application's current state in the New conversion journey.
   /// </summary>
   /// <param name="hasSchoolApplied">Indicates whether the school is voluntary or sponsored.</param>
   /// <param name="hasPreferredTrust">Indicates whether there is a preferred trust for the sponsored conversion.</param>
   /// <param name="hasProposedTrustName">Indicates whether a proposed trust name has been provided (Only happens in the creation of a FAM journey currently).</param>
   /// <returns>A string representing the URL to which the user should be directed.</returns>
   public static string DetermineBackLink(bool hasSchoolApplied, bool hasPreferredTrust, bool hasProposedTrustName)
   {
      // If a proposed trust name has been provided it means it's the new FAM route, return the link to create a new form for a MAT.
      if (hasProposedTrustName)
      {
         return Links.NewProject.CreateNewFormAMat.Page;
      }
      // If the school has applied or a preferred trust has been specified, return the search trusts page as both will need to search trust.
      else if (hasSchoolApplied || hasPreferredTrust)
      {
         return Links.NewProject.SearchTrusts.Page;
      }
      // If it's not of the above cases it suggests that it is a sponsored conversion without a preferred trust
      else
      {
         return Links.NewProject.PreferredTrust.Page;
      }

   }
}
