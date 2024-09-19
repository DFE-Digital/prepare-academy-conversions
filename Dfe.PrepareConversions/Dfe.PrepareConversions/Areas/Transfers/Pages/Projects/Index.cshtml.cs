using Dfe.PrepareTransfers.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.BackgroundServices;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Dfe.PrepareConversions.Data.Models.UserRole;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Data.Models;

namespace Dfe.PrepareTransfers.Web.Pages.Projects
{
    public class Index(ITaskListService taskListService, PerformanceDataChannel performanceDataChannel, ISession session) : CommonPageModel
    {
      public ProjectStatuses FeatureTransferStatus { get; set; }
        public ProjectStatuses TransferDatesStatus { get; set; }
        public ProjectStatuses BenefitsAndOtherFactorsStatus { get; set; }
        public ProjectStatuses LegalRequirementsStatus { get; set; }
        public ProjectStatuses RationaleStatus { get; set; }
        public ProjectStatuses AcademyAndTrustInformationStatus { get; set; }
        public string ProjectStatus { get; set; }
        public User AssignedUser { get; set; }
        public bool HasPermission { get; set; }

        /// <summary>
        /// Item1 Academy Ukprn, Item2 Academy Name
        /// </summary>
        public List<Tuple<string, string>> Academies { get; set; }
      public const string SESSION_KEY = "RoleCapabilities";

      public Task<IActionResult> OnGet(CancellationToken cancellationToken)
      {
         taskListService.BuildTaskListStatuses(this);
         HasPermission = session.HasPermission($"{SESSION_KEY}_{HttpContext.User.Identity.Name}", RoleCapability.DeleteTransferProject);
         //await RetrievePerformanceData(cancellationToken);
         return Task.FromResult<IActionResult>(Page());
      }

      private async Task RetrievePerformanceData(CancellationToken cancellationToken)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(90)); // wait max 30 seconds

            foreach (var academyUkprnAndUrn in Academies)
            {
                await performanceDataChannel.AddAcademyAsync(academyUkprnAndUrn.Item1, cts.Token);
            }
         
        }
    }
}