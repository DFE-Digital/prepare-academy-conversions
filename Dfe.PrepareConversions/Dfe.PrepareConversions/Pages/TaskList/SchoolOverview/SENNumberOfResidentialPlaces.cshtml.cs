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

public class SENNumberOfResidentialPlaces : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public SENNumberOfResidentialPlaces(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
   {
      _errorService = errorService;
   }
   public bool ShowError => _errorService.HasErrors();

   [BindProperty(Name = "number-of-residential-places")]
   public decimal? NumberOfResidentialPlaces { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);
      NumberOfResidentialPlaces = Project.NumberOfResidentialPlaces ?? null;
      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await base.OnGetAsync(id);
      if (NumberOfResidentialPlaces is null)
      {
         _errorService.AddError(nameof(NumberOfResidentialPlaces), "Enter a number. No letters or special characters.");
      }


      if (ModelState.IsValid)
      {
         SetSchoolOverviewModel updatedSchoolOverview = SchoolOverviewHelper.CreateUpdateSchoolOverview(Project);
         updatedSchoolOverview.Id = id;
         updatedSchoolOverview.NumberOfResidentialPlaces = NumberOfResidentialPlaces;
         await _repository.SetSchoolOverview(id, updatedSchoolOverview);

         return RedirectToPage(Links.SchoolOverviewSection.ConfirmSchoolOverview.Page, new { id });
      }

      return await base.OnGetAsync(id);
   }
}
