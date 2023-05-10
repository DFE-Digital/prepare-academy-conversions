using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Validators;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolAndTrustInformation;

public class RouteAndGrant : CommonPageModel
{
   public RouteAndGrant(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository, errorService)
   {
   }

   [BindProperty]
   public InputModel RouteAndGrantViewModel { get; set; }

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

      AcademyConversionProject conversionProject = project.Body;
      RouteAndGrantViewModel = new InputModel
      {
         ConversionSupportGrantAmount = conversionProject.ConversionSupportGrantAmount, 
         ConversionSupportGrantChangeReason = conversionProject.ConversionSupportGrantChangeReason,
         AcademyTypeAndRoute = conversionProject.AcademyTypeAndRoute
      };

      return Page();
   }

   public virtual async Task<IActionResult> OnPostAsync()
   {
      _errorService.AddErrors(Request.Form.Keys, ModelState);
      if (!ModelState.IsValid)
      {
         return Page();
      }

      ApiResponse<AcademyConversionProject> response = await _repository.UpdateProject(Id, Build());

      if (!response.Success)
      {
         _errorService.AddApiError();
         return Page();
      }

      (string returnPage, string fragment) = GetReturnPageAndFragment();
      if (!string.IsNullOrWhiteSpace(returnPage))
      {
         return RedirectToPage(returnPage, null, new { Id }, fragment);
      }

      return RedirectToPage(SuccessPage, new { Id });
   }

   protected UpdateAcademyConversionProject Build()
   {
      return new UpdateAcademyConversionProject
      {
         ConversionSupportGrantAmount = RouteAndGrantViewModel.ConversionSupportGrantAmount,
         ConversionSupportGrantChangeReason = RouteAndGrantViewModel.ConversionSupportGrantChangeReason,
      };
   }

   public class InputModel
   {
      [ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
      [Display(Name = "Conversion support grant")]
      [Range(typeof(decimal), "0", "25000", ErrorMessage = "Enter an amount that is £25,000 or less, for example £20,000")]
      [BindProperty(Name = "conversion-support-grant-amount")]
      public decimal? ConversionSupportGrantAmount { get; set; }

      [BindProperty(Name = "conversion-support-grant-change-reason")]
      [DisplayFormat(ConvertEmptyStringToNull = false)]
      [SupportGrantValidator]
      public string ConversionSupportGrantChangeReason { get; set; }
      public string AcademyTypeAndRoute { get; set; }
   }
}
