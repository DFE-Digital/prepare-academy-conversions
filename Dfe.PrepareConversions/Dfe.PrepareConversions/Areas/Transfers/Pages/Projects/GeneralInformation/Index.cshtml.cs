using Dfe.PrepareConversions.Data.Services.Person;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.GeneralInformation
{
   public class Index(
      IGetInformationForProject getInformationForProject,
      IPersonApiEstablishmentsService establishmentsService,
      IProjects projectsRepository,
      ErrorService errorService) : CommonPageModel
   {
      public IPersonApiEstablishmentsService _establishmentsService = establishmentsService;
      public IProjects _projectsRepository = projectsRepository;
      public string SchoolPhase { get; set; }
      public string AgeRange { get; set; }
      public string Capacity { get; set; }
      public string NumberOnRoll { get; set; }
      public string FreeSchoolMeals { get; set; }
      public string PublishedAdmissionNumber { get; set; }
      public string PrivateFinanceInitiative { get; set; }
      public string ViabilityIssues { get; set; }
      public string FinancialDeficit { get; set; }
      public string SchoolType { get; set; }
      public string DiocesePercent { get; set; }
      public string DistanceFromAcademyToTrustHq { get; set; }
      public string GIASLastChangedDate { get; set; }
      public string MP { get; set; }

      [BindProperty(SupportsGet = true)]
      public string AcademyUkprn { get; set; }

      public string AcademyName { get; set; }
      public string Urn { get; set; }

      public async Task<IActionResult> OnGetAsync(string urn)
      {
         var getInformationForProjectResponse = await getInformationForProject.Execute(urn);
         var project = getInformationForProjectResponse.Project;
         var academy = getInformationForProjectResponse.OutgoingAcademies.First(a => a.Ukprn == AcademyUkprn);
         var generalInformation = getInformationForProjectResponse.OutgoingAcademies.First(a => a.Ukprn == AcademyUkprn).GeneralInformation;

         AcademyName = academy.Name;
         SchoolPhase = generalInformation.SchoolPhase;
         AgeRange = generalInformation.AgeRange;
         Capacity = generalInformation.Capacity;
         NumberOnRoll = $"{generalInformation.NumberOnRoll} ({generalInformation.PercentageFull})";
         FreeSchoolMeals = generalInformation.PercentageFsm;
         PublishedAdmissionNumber = academy.PublishedAdmissionNumber;
         PrivateFinanceInitiative = $"{academy.PFIScheme} {academy.PFISchemeDetails}";
         ViabilityIssues = academy.ViabilityIssues;
         FinancialDeficit = academy.FinancialDeficit;
         SchoolType = generalInformation.SchoolType;
         DiocesePercent = generalInformation.DiocesesPercent;
         DistanceFromAcademyToTrustHq = $"{academy.DistanceFromAcademyToTrustHq?.ToString()} {academy.DistanceFromAcademyToTrustHqDetails}";
         Urn = urn;
         GIASLastChangedDate = "N/A";
         if (string.IsNullOrEmpty(academy.LastChangedDate) is false)
         {
            GIASLastChangedDate = DateTime.Parse(academy.LastChangedDate, CultureInfo.GetCultureInfo("en-GB")).ToString("MMMM yyyy");
         }

         await SetMPDetails(project, academy.Urn);

         OutgoingAcademyUrn = project.OutgoingAcademyUrn;
         return Page();
      }

      private async Task SetMPDetails(Project project, string urn)
      {
         var academy = project.TransferringAcademies.First(a => a.OutgoingAcademyUkprn == AcademyUkprn);

         if (string.IsNullOrEmpty(project.Status) || project.Status == "Pre decision" || project.Status == "Deferred")
         {
            var result = await _establishmentsService.GetMemberOfParliamentBySchoolUrnAsync(int.Parse(urn));

            if (result.IsSuccess)
            {
               var mpNameAndParty = result.Value.DisplayName;
               if (!string.IsNullOrWhiteSpace(result.Value.ConstituencyPartyName))
               {
                  mpNameAndParty = $"{mpNameAndParty} ({result.Value.ConstituencyPartyName})";
               }

               if (academy.MPNameAndParty != mpNameAndParty)
               {
                  academy.MPNameAndParty = mpNameAndParty;
                  await _projectsRepository.UpdateAcademyGeneralInformation(Urn, academy);
               }

               MP = mpNameAndParty;

               return;
            }

            SetError("member-of-parliament-name-and-party", "Member of Parliament name is not currently available. Try again later.");
            return;
         }

         MP = academy.MPNameAndParty;
      }

      public void SetError(string key, string message)
      {
         errorService.AddError(key, message);
      }
   }
}