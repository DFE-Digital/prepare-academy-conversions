using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services.DocumentGenerator;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using static Dfe.PrepareConversions.Utils.KeyStageDataStatusHelper;

namespace Dfe.PrepareConversions.Pages.TaskList;

public class DownloadProjectTemplate : BaseAcademyConversionProjectPageModel
{
   private readonly SchoolOverviewService _schoolOverviewService;
   private readonly KeyStagePerformanceService _keyStagePerformanceService;
   private readonly SchoolPerformanceService _schoolPerformanceService;

   public DownloadProjectTemplate(SchoolPerformanceService schoolPerformanceService,
                                  SchoolOverviewService schoolOverviewService,
                                  IAcademyConversionProjectRepository repository,
                                  KeyStagePerformanceService keyStagePerformanceService) : base(repository)
   {
      _schoolPerformanceService = schoolPerformanceService;
      _schoolOverviewService = schoolOverviewService;
      _keyStagePerformanceService = keyStagePerformanceService;
   }

   public string ErrorPage => TempData[nameof(ErrorPage)].ToString();

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);
      if (Project.HeadTeacherBoardDate != null) return Page();

      TempData["ShowGenerateHtbTemplateError"] = true;
      return RedirectToPage(ErrorPage, new { id });
   }

   public async Task<IActionResult> OnGetHtbTemplateAsync(int id)
   {
      ApiResponse<AcademyConversionProject> response = await _repository.GetProjectById(id);
      if (response.Success is false)
      {
         return NotFound();
      }

      AcademyConversionProject project = response.Body;

      SchoolPerformance schoolPerformance = await _schoolPerformanceService.GetSchoolPerformanceByUrn(project.Urn.ToString());
      Data.Models.SchoolOverview schoolOverview = await _schoolOverviewService.GetSchoolOverviewByUrn(project.Urn.ToString());
      KeyStagePerformance keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(project.Urn.ToString());

      var DocumentGenerator = new DocumentGenerator();
      HtbTemplate document = DocumentGenerator.GenerateDocument(response, schoolPerformance, schoolOverview, keyStagePerformance, project, out byte[] documentByteArray);

      return File(documentByteArray, "application/vnd.ms-word.document", $"{document.SchoolName}-project-template-{DateTime.Today:dd-MM-yyyy}.docx");
   }

  
}
