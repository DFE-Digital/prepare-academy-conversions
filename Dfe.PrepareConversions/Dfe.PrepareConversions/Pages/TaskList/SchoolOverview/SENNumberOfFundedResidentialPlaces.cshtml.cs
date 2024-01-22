using AngleSharp.Text;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolOverview;

public class SENNumberOfFundedResidentialPlaces : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public SENNumberOfFundedResidentialPlaces(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
   {
      _errorService = errorService;
   }
   public bool ShowError => _errorService.HasErrors();

   [BindProperty(Name = "number-of-funded-residential-places")]
   public decimal? NumberOfFundedResidentialPlaces { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);
      NumberOfFundedResidentialPlaces = Project.NumberOfFundedResidentialPlaces ?? null;
      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await base.OnGetAsync(id);
      if (NumberOfFundedResidentialPlaces is null)
      {
         _errorService.AddError(nameof(NumberOfFundedResidentialPlaces), "Enter a number. No letters or special characters.");
      }

      if (ModelState.IsValid)
      {
         SetSchoolOverviewModel updatedSchoolOverview = SchoolOverviewHelper.CreateUpdateSchoolOverview(Project);
         updatedSchoolOverview.Id = id;
         updatedSchoolOverview.NumberOfFundedResidentialPlaces = NumberOfFundedResidentialPlaces;
         await _repository.SetSchoolOverview(id, updatedSchoolOverview);

         return RedirectToPage(Links.SchoolOverviewSection.ConfirmSchoolOverview.Page, new { id });
      }

      return await base.OnGetAsync(id);
   }
}
