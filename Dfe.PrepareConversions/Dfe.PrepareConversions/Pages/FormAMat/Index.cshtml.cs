using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ProjectList;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Pages.FormAMat;

public class FormAMatIndexModel : BaseAcademyConversionProjectPageModel
{
   protected readonly ApplicationRepository _applicationRepository;
   public IEnumerable<BaseFormSection> Sections { get; set; }
   public FormAMatIndexModel(IAcademyConversionProjectRepository repository, ApplicationRepository applicationRepository) : base(repository)
   {
      _applicationRepository = applicationRepository;
   }

   public void SetErrorPage(string errorPage)
   {
      TempData["ErrorPage"] = errorPage;
   }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);

      IActionResult result = await SetProject(id);

      if ((result as StatusCodeResult)?.StatusCode == (int)HttpStatusCode.NotFound)
      {
         return NotFound();
      }
      string applicationReferenceId = Project.ApplicationReferenceNumber;
      ApiResponse<Application> applicationResponse = await _applicationRepository.GetApplicationByReference(applicationReferenceId, Project.SchoolName);

      if (applicationResponse.Success is false)
      {
         return NotFound();
      }
      if (applicationResponse.Body.ApplicationType is not (GlobalStrings.FormMat or GlobalStrings.FormAMat))
      {
         throw new NotImplementedException("Only Join a MAT and Form a MAT are available at this time");
      }

      Application application = applicationResponse.Body;
      ApplyingSchool currentSchool = application.ApplyingSchools.First(x => x.SchoolName == Project.SchoolName);

      Sections = new BaseFormSection[]
      {
         new FamApplicationFormSection(application),
         new TrustInformationSection(application),
         new KeyPeopleSection(application),
         new AboutConversionSection(currentSchool),
         new FurtherInformationSection(currentSchool),
         new FinanceSection(currentSchool),
         new FuturePupilNumberSection(currentSchool),
         new LandAndBuildingsSection(currentSchool),
         new PreOpeningSupportGrantSection(currentSchool),
         new ConsultationSection(currentSchool),
         new DeclarationSection(currentSchool)
      };

      return result;
   }
   public string GenerateId(string heading)
   {
      return heading.Replace(" ", "_");
   }
}
