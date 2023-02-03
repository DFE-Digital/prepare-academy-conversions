using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

		[BindProperty(Name = "query", SupportsGet = true)]
		public string SearchQuery { get; set; } = "";

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPost()
		{
			if (string.IsNullOrWhiteSpace(SearchQuery))
			{
				ModelState.AddModelError(nameof(SearchQuery), "Enter the school name or URN");
				_errorService.AddErrors(ModelState.Keys, ModelState);
				return Page();
			}

			var schools = await _getEstablishment.SearchEstablishments(SearchQuery);

			if (schools.Any())
			{
				return RedirectToPage(Links.InvoluntaryProject.SchoolResults.Page, new { query = SearchQuery });
			}
			else
			{
				ModelState.AddModelError(nameof(SearchQuery), "We could not find any schools matching your search criteria");
				_errorService.AddErrors(ModelState.Keys, ModelState);
				return Page();
			}
		}
	}
}