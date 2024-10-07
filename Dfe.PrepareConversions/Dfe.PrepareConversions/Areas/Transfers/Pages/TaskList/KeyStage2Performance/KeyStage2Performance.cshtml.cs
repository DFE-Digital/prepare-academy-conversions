using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList.KeyStage2Performance
{
   public class KeyStage2Performance(IGetInformationForProject getInformationForProject, IProjects projectRepository) : CommonPageModel
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
         await BuildPageModel();
         return Page();
      }

      public async Task<IActionResult> OnPostAsync()
      {
         var project = await projectRepository.GetByUrn(Urn);
         var academy = project.Result.TransferringAcademies.First(a => a.OutgoingAcademyUkprn == AcademyUkprn);
         academy.KeyStage2PerformanceAdditionalInformation = AdditionalInformationViewModel.AdditionalInformation;
         await projectRepository.UpdateAcademy(project.Result.Urn, academy);

         if (ReturnToPreview)
         {
            return new RedirectToPageResult(
                Links.HeadteacherBoard.Preview.PageName,
                new { Urn }
            );
         }

         return new RedirectToPageResult(nameof(KeyStage2Performance),
             null,
             new { Urn },
             "additional-information-hint");
      }

      private async Task BuildPageModel()
      {
         var projectInformation = await getInformationForProject.Execute(Urn);
         var academy = projectInformation.OutgoingAcademies.First(a => a.Ukprn == AcademyUkprn);
         AcademyName = academy.Name;
         OutgoingAcademyUrn = academy.Urn;
         EducationPerformance = academy.EducationPerformance;
      }

   }
}