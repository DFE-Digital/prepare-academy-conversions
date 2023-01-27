using Dfe.PrepareConversions.Configuration;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Pages.ProjectType
{
	public class IndexModel : PageModel
	{
		private const string ErrorMessage = "Select a project type";
		
		private readonly string _transfersUrl;
		private readonly ErrorService _errorService;

		public IndexModel(IOptions<ServiceLinkOptions> options, ErrorService errorService)
		{
			_transfersUrl = options.Value.TransfersUrl;
			_errorService = errorService;
		}
		
		[BindProperty, Required(ErrorMessage = ErrorMessage)]
		public ProjectTypes? ProjectType { get; set; }
		
		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(new[] { nameof(ProjectType) }, ModelState);
				return Page();
			}
			
			if (ProjectType is ProjectTypes.Transfer) return Redirect($"{_transfersUrl}/home");
			return RedirectToPage(Links.ProjectList.Index.Page);
		}
	}
}
