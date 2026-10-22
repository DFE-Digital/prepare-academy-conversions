using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Preview;

public class IndexModel(ISignificantChangeProjectRepository repository) : PageModel
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   public SignificantChangeProjectViewBaseModel Project { get; private set; } = null!;

   public async Task<IActionResult> OnGetAsync(int id)
   {
      var result = await _repository.GetProjectById(id);

      if (result.StatusCode == HttpStatusCode.NotFound)
      {
         return NotFound();
      }

      Project = SignificantChangeProjectHelper.Build(result.Body);

      return Page();
   }
}