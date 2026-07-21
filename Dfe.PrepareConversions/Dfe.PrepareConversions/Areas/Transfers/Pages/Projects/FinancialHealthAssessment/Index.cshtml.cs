
using Dfe.PrepareConversions.Utils;
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
      public bool RequestSent => RequestedDate.HasValue;
      public bool RequestDateInPast => RequestedDate.HasValue && RequestedDate.Value.Date < DateTime.Today;

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
            projectResult.Dates != null && projectResult.Dates.HasHtbDate == true && !string.IsNullOrEmpty(projectResult.Dates.Htb)
               ? DateTime.ParseExact(projectResult.Dates.Htb, "dd/MM/yyyy", CultureInfo.InvariantCulture)
               : (DateTime?)null;
      }
   }
}