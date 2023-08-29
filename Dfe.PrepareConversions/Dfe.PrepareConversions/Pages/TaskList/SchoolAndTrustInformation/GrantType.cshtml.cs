using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolAndTrustInformation;

public class GrantType : CommonPageModel
{
   public GrantType(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository, errorService)
   {
   }

   public string SuccessPage
   {
      get => TempData[nameof(SuccessPage)].ToString();
      set => TempData[nameof(SuccessPage)] = value;
   }

   public virtual async Task<IActionResult> OnGetAsync()
   {
      ApiResponse<AcademyConversionProject> project = await _repository.GetProjectById(Id);
      if (!project.Success)
      {
         // 404 logic
         return NotFound();
      }

      return Page();
   }
}
