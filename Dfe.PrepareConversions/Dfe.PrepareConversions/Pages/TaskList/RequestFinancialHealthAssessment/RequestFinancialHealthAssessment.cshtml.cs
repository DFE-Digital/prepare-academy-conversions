
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.RequestFinancialHealthAssessment;

public class RequestFinancialHealthAssessmentModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public RequestFinancialHealthAssessmentModel(IAcademyConversionProjectRepository repository, ErrorService errorService)
      : base(repository)
   {
      _errorService = errorService;
   }

   [BindProperty(Name = "sfso-commissioning-overview")]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   [StringLength(250, ErrorMessage = "Overview must be 250 characters or less")]
   public string SfsoCommissioningOverview { get; set; }

   public DateTime? RequestedDate { get; set; }

   public bool ShowError => _errorService.HasErrors();

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      RequestedDate = Project.SfsoCommissioningRequestedDate;
      SfsoCommissioningOverview = Project.SfsoCommissioningOverview;

      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await base.OnGetAsync(id);
      RequestedDate = Project.SfsoCommissioningRequestedDate;

      _errorService.AddErrors(Request.Form.Keys, ModelState);
      if (_errorService.HasErrors())
      {
         return Page();
      }

      var model = new SetSfsoCommissioningModel
      {
         SfsoCommissioningOverview = SfsoCommissioningOverview,
         SfsoCommissioningSectionComplete = true
      };

      await _repository.SetSfsoCommissioning(id, model);

      return RedirectToPage(Links.TaskList.Index.Page, new { id });
   }
}