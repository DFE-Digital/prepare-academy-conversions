using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace Dfe.PrepareConversions.Models;

public class CommonPageModel : PageModel
{
   protected readonly ErrorService _errorService;

   protected readonly IAcademyConversionProjectRepository _repository;

   public CommonPageModel(IAcademyConversionProjectRepository repository, ErrorService errorService)
   {
      _repository = repository;
      _errorService = errorService;
   }

   [BindProperty(SupportsGet = true)]
   public int Id { get; set; }

   public string SchoolName { get; set; }
   public bool ShowError => _errorService.HasErrors();

   protected (string, string) GetReturnPageAndFragment()
   {
      Request.Query.TryGetValue("return", out StringValues returnQuery);
      Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
      return (returnQuery, fragmentQuery);
   }
}
