using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services.DocumentGenerator;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList;

public class DownloadProjectTemplate(SchoolOverviewService schoolOverviewService,
                               IAcademyConversionProjectRepository repository,
                               KeyStagePerformanceService keyStagePerformanceService) : BaseAcademyConversionProjectPageModel(repository)
{
   public string ErrorPage => TempData[nameof(ErrorPage)].ToString();

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      return Page();
   }

   public async Task<IActionResult> OnGetHtbTemplateAsync(int id)
   {
      var response = await _repository.GetProjectById(id);
      if (response.Success is false)
      {
         return NotFound();
      }

      var project = response.Body;

      var schoolOverview = await schoolOverviewService.GetSchoolOverviewByUrn(project.Urn.ToString());
      var keyStagePerformance = await keyStagePerformanceService.GetKeyStagePerformance(project.Urn.ToString());
      var document = DocumentGenerator.GenerateDocument(response, schoolOverview, keyStagePerformance, project, out byte[] documentByteArray);

      return File(documentByteArray, "application/vnd.ms-word.document", $"{document.SchoolName}-project-template-{DateTime.Today:dd-MM-yyyy}.docx");
   }
}
