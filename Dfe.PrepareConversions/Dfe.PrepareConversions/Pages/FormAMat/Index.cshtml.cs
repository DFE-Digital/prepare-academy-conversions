using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.FormAMat
{
	public class FormAMatIndexModel : BaseAcademyConversionProjectPageModel
	{
      public FormAMatIndexModel(IAcademyConversionProjectRepository repository) : base(repository)
		{
      }
      public void SetErrorPage(string errorPage)
		{
			TempData["ErrorPage"] = errorPage;
		}

		public override async Task<IActionResult> OnGetAsync(int id)
		{
         ProjectListFilters.ClearFiltersFrom(TempData);

         IActionResult result = await SetProject(id);

			if ((result as StatusCodeResult)?.StatusCode == (int)HttpStatusCode.NotFound)
			{
				return NotFound();
			}

         return Page();
		}
	}
}
