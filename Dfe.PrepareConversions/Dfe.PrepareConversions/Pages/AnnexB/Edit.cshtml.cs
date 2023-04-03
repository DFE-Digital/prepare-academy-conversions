using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.AnnexB;

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
   public string AnnexFormUrl { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      YesChecked = Project.AnnexBFormReceived;
      AnnexFormUrl = Project.AnnexBFormUrl;

      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      if (YesChecked is true && string.IsNullOrWhiteSpace(AnnexFormUrl))
      {
         ModelState.AddModelError(nameof(AnnexFormUrl), "You must enter valid link for the Annex B form");
      }

      if (ModelState.IsValid)
      {
         UpdateAcademyConversionProject updatedProject = new() { AnnexBFormReceived = YesChecked, AnnexBFormUrl = YesChecked is true ? AnnexFormUrl : default };

         ApiResponse<AcademyConversionProject> apiResponse = await _repository.UpdateProject(id, updatedProject);

         if (apiResponse.Success)
            return RedirectToPage(Links.AnnexB.Index.Page, new { id });
      }

      _errorService.AddErrors(ModelState.Keys, ModelState);
      return await base.OnGetAsync(id);
   }
}
