using Dfe.PrepareTransfers.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.BackgroundServices;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Dfe.PrepareConversions.Data.Models.UserRole;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Services;
using System.Net;

namespace Dfe.PrepareTransfers.Web.Pages.Projects
{
    public class Index(ErrorService errorService, ITaskListService taskListService, PerformanceDataChannel performanceDataChannel, ISession session) : CommonPageModel
    {
      public ProjectStatuses FeatureTransferStatus { get; set; }
      public ProjectStatuses TransferDatesStatus { get; set; }
      public ProjectStatuses BenefitsAndOtherFactorsStatus { get; set; }
      public ProjectStatuses LegalRequirementsStatus { get; set; }
      public ProjectStatuses RationaleStatus { get; set; }
      public ProjectStatuses AcademyAndTrustInformationStatus { get; set; }
      public ProjectStatuses PublicSectorEqualityDutyStatus { get; set; }
      public string ProjectStatus { get; set; }
      public User AssignedUser { get; set; }
      public bool HasPermission { get; set; }
        

      /// <summary>
      /// Item1 Academy Ukprn, Item2 Academy Name
      /// </summary>
      public List<Tuple<string, string>> Academies { get; set; }
      public const string SESSION_KEY = "RoleCapabilities";

      private void SetPermission()
      {
         HasPermission = session.HasPermission($"{SESSION_KEY}_{HttpContext.User.Identity.Name}", RoleCapability.DeleteTransferProject);
      }

      public IActionResult OnGet(CancellationToken cancellationToken)
      {
         taskListService.BuildTaskListStatuses(this);
         
         SetPermission();

         return Page();
      }

      private void Validate()
      {
         string returnPage = WebUtility.UrlEncode("/TaskList/Index");

         if (string.IsNullOrWhiteSpace(HeadTeacherBoardDate))
         {
            errorService.AddError($"/transfers/project/{Urn}/transfer-dates/proposed-decision-date?return={returnPage}",
               "Set a Proposed decision date before you generate your project template before you generate your project template");
         }

         var isPsedValid = PrepareConversions.Models.PreviewPublicSectorEqualityDutyModel.IsValid(PublicEqualityDutyImpact, PublicEqualityDutyReduceImpactReason, PublicEqualityDutySectionComplete ?? false);
         if (!isPsedValid)
         {
            errorService.AddError($"/transfers/project/{Urn}/public-sector-equality-duty?return={returnPage}",
               "Consider the Public Sector Equality Duty");
         }
      }

      public IActionResult OnPostPreviewAsync(string urn)
      {
         taskListService.BuildTaskListStatuses(this);

         Validate();

         if (errorService.HasErrors())
         {
            SetPermission();

            return Page();
         }

         return RedirectToPage("/TaskList/HtbDocument/Preview", new { Urn });
      }

      public IActionResult OnPostGenerateAsync(string urn)
      {
         taskListService.BuildTaskListStatuses(this);

         Validate();

         if (errorService.HasErrors())
         {
            SetPermission();

            return Page();
         }

         return RedirectToPage("/TaskList/HtbDocument/Download", new { Urn });
      }
    }
}