using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.FinancialHealthAssessment
{
   public class Index : CommonPageModel
   {
      private readonly IProjects _projects;

      public Index(IProjects projects) => _projects = projects;

      [BindProperty]
      [DisplayFormat(ConvertEmptyStringToNull = false)]
      [StringLength(250, ErrorMessage = "Overview must be 250 characters or less")]
      public string SfsoCommissioningOverview { get; set; }

      public DateTime? RequestedDate { get; set; }
      public DateTime? ProposedDecisionDate { get; set; }
      public bool HasProposedDecisionDate => ProposedDecisionDate.HasValue;
      
      // Stored request date is in the future -> scheduled but not yet sent.
      public bool RequestWillBeSent => RequestedDate.HasValue && RequestedDate.Value.Date > DateTime.Today;
      
      // Stored request date is today or past -> already sent.
      public bool RequestSent => RequestedDate.HasValue && RequestedDate.Value.Date <= DateTime.Today;

      public async Task<IActionResult> OnGetAsync()
      {
         var projectResult = (await _projects.GetByUrn(Urn)).Result;
         Populate(projectResult);
         SfsoCommissioningOverview = projectResult.SfsoCommissioningOverview;
         return Page();
      }

      public async Task<IActionResult> OnPostAsync()
      {
         var projectResult = (await _projects.GetByUrn(Urn)).Result;
         Populate(projectResult);   // does NOT overwrite the bound overview -> keeps user input on validation error

         if (!ModelState.IsValid)
         {
            return Page();
         }

         await _projects.UpdateSfsoCommissioning(Urn, SfsoCommissioningOverview);

         return RedirectToPage("/Projects/Index", new { Urn });
      }

      private void Populate(Project projectResult)
      {
        ProjectReference = projectResult.Reference;
        IncomingTrustName = projectResult.IncomingTrustName;
        IsReadOnly = projectResult.IsReadOnly;
        RequestedDate = projectResult.Dates?.SfsoCommissioningRequestedDate;
        ProposedDecisionDate =
            DateTime.TryParseExact(projectResult.Dates?.Htb, new[] { "dd/MM/yyyy", "dd-MM-yyyy" },
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var htbDate)
                ? htbDate
                : (DateTime?)null;
      }
   }
}