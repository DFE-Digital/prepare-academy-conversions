using AngleSharp.Text;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolOverview;

public class NumberOfPlacesFundedFor : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public NumberOfPlacesFundedFor(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
   {
      _errorService = errorService;
   }
   public bool ShowError => _errorService.HasErrors();

   [BindProperty]
   public int NumberOfPlaces { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await base.OnGetAsync(id);
      if (NumberOfPlaces.IsInvalid())
      {
         ModelState.AddModelError(nameof(NumberOfPlaces), "You must enter the number of places funded for");
      }


      if (ModelState.IsValid)
      {
         SetSchoolOverviewModel updatedSchoolOverview = CreateUpdateSchoolOverview(Project);
         updatedSchoolOverview.Id = id;
         updatedSchoolOverview.NumberOfPlacesFundedFor = NumberOfPlaces;
         await _repository.SetSchoolOverview(id, updatedSchoolOverview);

         return RedirectToPage(Links.SchoolOverviewSection.ConfirmSchoolOverview.Page, new { id });
      }

      _errorService.AddErrors(ModelState.Keys, ModelState);
      return await base.OnGetAsync(id);
   }
   public static SetSchoolOverviewModel CreateUpdateSchoolOverview(ProjectViewModel projectViewModel)
   {
      return new SetSchoolOverviewModel(
          projectViewModel.Id.ToInteger(0),
          projectViewModel.PublishedAdmissionNumber,
          projectViewModel.ViabilityIssues,
          projectViewModel.FinancialDeficit,
          projectViewModel.NumberOfPlacesFundedFor,
          projectViewModel.PartOfPfiScheme,
          projectViewModel.PfiSchemeDetails,
          projectViewModel.DistanceFromSchoolToTrustHeadquarters,
          projectViewModel.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
          projectViewModel.MemberOfParliamentNameAndParty
      );
   }
}
