using AngleSharp.Text;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolOverview;

public class SENNumberOfPlacesFundedFor : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public SENNumberOfPlacesFundedFor(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
   {
      _errorService = errorService;
   }
   public bool ShowError => _errorService.HasErrors();

   [BindProperty(Name = "number-of-places-funded-for")]
   public decimal? NumberOfPlacesFundedFor { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);
      NumberOfPlacesFundedFor = Project.NumberOfPlacesFundedFor ?? null;
      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await base.OnGetAsync(id);
      if (NumberOfPlacesFundedFor is null)
      {
         _errorService.AddError(nameof(NumberOfPlacesFundedFor), "Enter a number. No letters or special characters.");
      }


      if (ModelState.IsValid)
      {
         SetSchoolOverviewModel updatedSchoolOverview = CreateUpdateSchoolOverview(Project);
         updatedSchoolOverview.Id = id;
         updatedSchoolOverview.NumberOfPlacesFundedFor = NumberOfPlacesFundedFor;
         await _repository.SetSchoolOverview(id, updatedSchoolOverview);

         return RedirectToPage(Links.SchoolOverviewSection.ConfirmSchoolOverview.Page, new { id });
      }


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
