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

public class PruPupilsAttendingGroup : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public PruPupilsAttendingGroup(IAcademyConversionProjectRepository repository, ErrorService errorService) : base(repository)
   {
      _errorService = errorService;
   }
   public bool ShowError => _errorService.HasErrors();

   [BindProperty(Name = "permanently-excluded")]
   public bool? PupilsAttendingGroupPermanentlyExcluded { get; set; }

   [BindProperty(Name = "medical-and-health-needs")]
   public bool? PupilsAttendingGroupMedicalAndHealthNeeds { get; set; }

   [BindProperty(Name = "teenage-mums")]
   public bool? PupilsAttendingGroupTeenageMums { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);
      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await base.OnGetAsync(id);

      if (ModelState.IsValid)
      {
         SetSchoolOverviewModel updatedSchoolOverview = SchoolOverviewHelper.CreateUpdateSchoolOverview(Project);
         updatedSchoolOverview.Id = id;
         updatedSchoolOverview.PupilsAttendingGroupPermanentlyExcluded = PupilsAttendingGroupPermanentlyExcluded;
         updatedSchoolOverview.PupilsAttendingGroupMedicalAndHealthNeeds = PupilsAttendingGroupMedicalAndHealthNeeds;
         updatedSchoolOverview.PupilsAttendingGroupTeenageMums = PupilsAttendingGroupTeenageMums;
         await _repository.SetSchoolOverview(id, updatedSchoolOverview);

         return RedirectToPage(Links.SchoolOverviewSection.ConfirmSchoolOverview.Page, new { id });
      }

      return await base.OnGetAsync(id);
   }
}
