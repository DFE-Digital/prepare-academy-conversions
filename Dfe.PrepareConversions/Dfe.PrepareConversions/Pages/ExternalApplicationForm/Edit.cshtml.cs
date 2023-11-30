using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ExternalApplicationForm;

public class EditModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public EditModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
   {
      _errorService = errorService;
   }

   [BindProperty]
   public bool? YesChecked { get; set; }

   [BindProperty]
   public string ExternalApplicationFormUrl { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      YesChecked = Project.ExternalApplicationFormSaved;
      ExternalApplicationFormUrl = Project.ExternalApplicationFormUrl;

      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      if (YesChecked is true && string.IsNullOrWhiteSpace(ExternalApplicationFormUrl))
      {
         ModelState.AddModelError(nameof(ExternalApplicationFormUrl), "You must enter valid link for the achools application form");
      }

      if (ModelState.IsValid)
      {
         UpdateAcademyConversionProject updatedProject = new() { ExternalApplicationFormSaved = YesChecked, ExternalApplicationFormUrl = YesChecked is true ? ExternalApplicationFormUrl : default };

         ApiResponse<AcademyConversionProject> apiResponse = await _repository.UpdateProject(id, updatedProject);

         if (apiResponse.Success)
            return RedirectToPage(Links.ExternalApplicationForm.Index.Page, new { id });
      }

      _errorService.AddErrors(ModelState.Keys, ModelState);
      return await base.OnGetAsync(id);
   }
}
