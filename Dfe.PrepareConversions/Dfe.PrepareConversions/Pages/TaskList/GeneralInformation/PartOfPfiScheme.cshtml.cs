using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.GeneralInformation;

public class PartOfPfiModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public PartOfPfiModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
   {
      _errorService = errorService;
   }
   public bool ShowError => _errorService.HasErrors();

   [BindProperty]
   public bool? YesChecked { get; set; }

   [BindProperty]
   public string PfiSchemeDetails { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      YesChecked = Project.PartOfPfiScheme.ToBool();
      PfiSchemeDetails = Project.PfiSchemeDetails;

      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      if (YesChecked is true && string.IsNullOrWhiteSpace(PfiSchemeDetails))
      {
         ModelState.AddModelError(nameof(PfiSchemeDetails), "You must enter valid input.");
      }

      if (ModelState.IsValid)
      {
         UpdateAcademyConversionProject updatedProject = new() { PartOfPfiScheme = YesChecked.ToYesNoString(), PfiSchemeDetails = YesChecked is true ? PfiSchemeDetails : default };
         if (updatedProject.PartOfPfiScheme.Equals("No")) updatedProject.PfiSchemeDetails = String.Empty;
         ApiResponse<AcademyConversionProject> apiResponse = await _repository.UpdateProject(id, updatedProject);

         if (apiResponse.Success)
            return RedirectToPage(Links.GeneralInformationSection.ConfirmGeneralInformation.Page, new { id });
      }

      _errorService.AddErrors(ModelState.Keys, ModelState);
      return await base.OnGetAsync(id);
   }
}
