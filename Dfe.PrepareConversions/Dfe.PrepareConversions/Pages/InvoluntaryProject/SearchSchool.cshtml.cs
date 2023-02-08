using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.InvoluntaryProject
{
	public class SearchSchoolModel : PageModel
	{
		private readonly IGetEstablishment _getEstablishment;
		private readonly ErrorService _errorService;

		public SearchSchoolModel(IGetEstablishment getEstablishment, ErrorService errorService)
		{
			_getEstablishment = getEstablishment;
			_errorService = errorService;
		}

		[BindProperty] public string SearchQuery { get; set; } = "";

		public async Task<IActionResult> OnGet(string urn, string ukprn)
		{
			var establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
			if (!string.IsNullOrWhiteSpace(establishment.Urn))
			{
				SearchQuery = $"{establishment.EstablishmentName} ({establishment.Urn})";
			}

			return Page();
		}

		public async Task<IActionResult> OnGetSearch(string searchQuery)
		{
			var searchSplit = searchQuery.Split('(', ')');

			var schools = await _getEstablishment.SearchEstablishments(searchSplit[0].Trim());

			return new JsonResult(schools.Select(s => new
			{
				suggestion = HighlightSearchMatch($"{s.Name} ({s.Urn})", searchSplit[0].Trim(), s),
				value = $"{s.Name} ({s.Urn})"
			}));
		}

		public IActionResult OnPost(string ukprn)
		{
			if (string.IsNullOrWhiteSpace(SearchQuery))
			{
				ModelState.AddModelError(nameof(SearchQuery), "Enter the school name or URN");
				_errorService.AddErrors(ModelState.Keys, ModelState);
				return Page();
			}

			var splitSearch = SearchQuery.Split('(', ')');
			if (splitSearch.Count() < 1) return Page();

			return RedirectToPage(Links.InvoluntaryProject.SearchTrusts.Page, new { urn = splitSearch[1], ukprn });
		}

		private static string HighlightSearchMatch(string input, string toReplace, EstablishmentSearchResponse school)
		{
			if (school == null || string.IsNullOrWhiteSpace(school.Urn) || string.IsNullOrWhiteSpace(school.Name)) return string.Empty;

			var index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
			var correctCaseSearchString = input.Substring(index, toReplace.Length);

			return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
		}
	}
}