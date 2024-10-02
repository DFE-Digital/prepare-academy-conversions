using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareTransfers.Web.Services.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList.KeyStage5Performance
{
   public class KeyStage5Performance(IGetInformationForProject getInformationForProject, IProjects projects) : CommonPageModel
   {
      [BindProperty]
      public AdditionalInformationViewModel AdditionalInformationViewModel { get; set; }
      [BindProperty(SupportsGet = true)]
      public string AcademyUkprn { get; set; }
      public string AcademyName { get; set; }


      #region remove
      public EducationPerformance EducationPerformance { get; set; }

      #endregion

      public async Task<IActionResult> OnGetAsync()
      {
         var projectInformation = await getInformationForProject.Execute(Urn);

         PopulateModel(projectInformation);
         return Page();
      }

      public async Task<IActionResult> OnPostAsync()
      {
         var project = await projects.GetByUrn(Urn);
         var academy = project.Result.TransferringAcademies.First(a => a.OutgoingAcademyUkprn == AcademyUkprn);
         academy.KeyStage5PerformanceAdditionalInformation = AdditionalInformationViewModel.AdditionalInformation;
         await projects.UpdateAcademy(project.Result.Urn, academy);

         if (ReturnToPreview)
         {
            return new RedirectToPageResult(
                Links.HeadteacherBoard.Preview.PageName,
                new { Urn }
            );
         }

         return new RedirectToPageResult(nameof(KeyStage5Performance),
             null,
             new { Urn },
             "additional-information-hint");
      }

      private void PopulateModel(GetInformationForProjectResponse projectInformation)
      {
         var academy = projectInformation.OutgoingAcademies.First(a => a.Ukprn == AcademyUkprn);
         EducationPerformance = academy.EducationPerformance;
         OutgoingAcademyUrn = academy.Urn;
      }
   }
}