using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Models
{
	public class CommonPageModel : PageModel
	{
		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }
		public string SchoolName { get; set; }

		protected readonly IAcademyConversionProjectRepository _repository;
		protected readonly ErrorService _errorService;

		public CommonPageModel(IAcademyConversionProjectRepository repository, ErrorService errorService)
		{
			_repository = repository;
			_errorService = errorService;
		}
		public bool ShowError => _errorService.HasErrors();
		protected (string, string) GetReturnPageAndFragment()
		{
			Request.Query.TryGetValue("return", out var returnQuery);
			Request.Query.TryGetValue("fragment", out var fragmentQuery);
			return (returnQuery, fragmentQuery);
		}
	}
}
