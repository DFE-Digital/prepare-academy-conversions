using AngleSharp.Text;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
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
         ModelState.AddModelError(nameof(NumberOfFundedResidentialPlaces), "You must enter the number of places funded for");
      }


      if (ModelState.IsValid)
      {
         SetSchoolOverviewModel updatedSchoolOverview = CreateUpdateSchoolOverview(Project);
         updatedSchoolOverview.Id = id;
         updatedSchoolOverview.NumberOfFundedResidentialPlaces = NumberOfFundedResidentialPlaces;
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
          projectViewModel.NumberOfResidentialPlaces,
          projectViewModel.NumberOfFundedResidentialPlaces,
          projectViewModel.PartOfPfiScheme,
          projectViewModel.PfiSchemeDetails,
          projectViewModel.DistanceFromSchoolToTrustHeadquarters,
          projectViewModel.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
          projectViewModel.MemberOfParliamentNameAndParty
      );
   }
}
