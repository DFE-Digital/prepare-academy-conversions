using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstablishmentDto = Dfe.Academies.Contracts.V4.Establishments.EstablishmentDto;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class LinkFormAMatProject : PageModel
{
   private readonly ErrorService _errorService;
   private readonly IGetEstablishment _getEstablishment;
   private readonly IAcademyConversionProjectRepository _repository;

   public LinkFormAMatProject(IAcademyConversionProjectRepository repository, IGetEstablishment getEstablishment, ErrorService errorService)
   {
      _repository = repository;
      _getEstablishment = getEstablishment;
      _errorService = errorService;
   }

   private const string SEARCH_LABEL = "Enter the trust name, application reference or FAM reference of the existing form a MAT/SAT project";
   private const string SEARCH_ENDPOINT = "/start-new-project/link-project?handler=Search&searchQuery=";

   [BindProperty]
   public string SearchQuery { get; set; } = "";

   [BindProperty]
   public string IsFormAMat { get; set; }
   [BindProperty]
   public string IsProjectInPrepare { get; set; }
   [BindProperty]
   public string HasSchoolApplied { get; set; }

   public string Urn { get; set; }

   public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

   public async Task<IActionResult> OnGet(string urn, string isFormAMat, string hasSchoolApplied, string isProjectInPrepare)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);
      HasSchoolApplied = hasSchoolApplied;
      IsFormAMat = isFormAMat;
      IsProjectInPrepare = isProjectInPrepare;

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      Urn = establishment.Urn;

      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);

      return Page();
   }

   public async Task<IActionResult> OnGetSearch(string searchQuery)
   {
      string[] searchSplit = SplitOnBrackets(searchQuery);

      IEnumerable<FormAMatProject> projects = (await _repository.SearchFormAMatProjects(searchQuery)).Body;

      return new JsonResult(projects.Select(s => new { suggestion = HighlightSearchMatch($"{s.ProposedTrustName} ({DeduceReferenceNumber(s)})", searchSplit[0].Trim(), s), value = $"{DeduceReferenceNumber(s)}" }));
   }

   public async Task<IActionResult> OnPost(string ukprn, string urn, string redirect)
   {
      AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);

      if (SearchQuery.IsNullOrEmpty() || SearchQuery.Length <= 2)
      {
         _errorService.AddError("Application Reference", "Please enter a application reference with more than three characters");
         return Page();
      }

      var applicationReference = SearchQuery;

      var nextPage = Links.NewProject.Summary.Page;

      redirect = string.IsNullOrEmpty(redirect) ? nextPage : redirect;

      return RedirectToPage(redirect, new { ukprn, urn, HasSchoolApplied, IsFormAMat, IsProjectInPrepare, applicationReference });
   }

   private static string HighlightSearchMatch(string input, string toReplace, FormAMatProject project)
   {
      if (project == null || string.IsNullOrWhiteSpace(project.ReferenceNumber) || string.IsNullOrWhiteSpace(project.ProposedTrustName)) return string.Empty;

      int index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
      string correctCaseSearchString = input.Substring(index, toReplace.Length);

      return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
   }

   private static string[] SplitOnBrackets(string input)
   {
      // return array containing one empty string if input string is null or empty
      if (string.IsNullOrWhiteSpace(input)) return new string[1] { string.Empty };

      return input.Split(new[] { '(', ')' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
   }

   private static string DeduceReferenceNumber(FormAMatProject project)
   {
      return !string.IsNullOrEmpty(project.ReferenceNumber) ? project.ReferenceNumber : project.ApplicationReference;
   }
}
