
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Generic;

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

   // The stored request date (null when the proposed decision date is > 15 days away).
   public DateTime? RequestedDate { get; set; }

   // Mandatory information still outstanding. Empty == ready to request.
   public IReadOnlyList<FinancialHealthAssessmentPrerequisite> MissingInformation =>
      FinancialHealthAssessmentPrerequisites.GetMissing(Project);

   public bool HasAllMandatoryInformation => MissingInformation.Count == 0;

   // Stored request date is in the future -> scheduled but not yet sent.
   public bool RequestWillBeSent => RequestedDate.HasValue && RequestedDate.Value.Date > DateTime.Today;
   
   // Stored request date is today or past -> already sent.
   public bool RequestSent => RequestedDate.HasValue && RequestedDate.Value.Date <= DateTime.Today;

   // Scenario 8: once a decision is recorded the API marks the project read-only -> lock this task.
   public bool IsReadOnly => Project?.IsReadOnly == true;

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
         SfsoCommissioningOverview = SfsoCommissioningOverview
      };

      await _repository.SetSfsoCommissioning(id, model);

      return RedirectToPage(Links.TaskList.Index.Page, new { id });
   }
}