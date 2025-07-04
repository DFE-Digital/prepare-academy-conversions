using Dfe.PrepareConversions.Data;
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
using Dfe.PrepareConversions.Models;
using System.Globalization;

namespace Dfe.PrepareConversions.Pages.FormAMat;

public class FormAMatIndexModel : BaseAcademyConversionProjectPageModel
{
   protected readonly ApplicationRepository _applicationRepository;
   public IEnumerable<BaseFormSection> Sections { get; set; }

   public string ReturnPage { get; set; }
   public string ReturnId { get; set; }
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
         new LandAndBuildingsSection(currentSchool)
      };

      var applicationReceivedDate = DateTime.Parse(Project.ApplicationReceivedDate, CultureInfo.GetCultureInfo("en-GB"));
      if (DateTime.Compare(applicationReceivedDate, new DateTime(2024, 12, 20, 23, 59, 59, DateTimeKind.Utc)) <= 0)
      {
         Sections = [.. Sections, new ConversionSupportGrantSection(application.ApplyingSchools.First())];
      }

      Sections = [
         .. Sections,
         new ConsultationSection(application.ApplyingSchools.First()),
         new DeclarationSection(application.ApplyingSchools.First())
      ];

      ReturnPage = @Links.ProjectList.Index.Page;
      var returnToFormAMatMenu = TempData["returnToFormAMatMenu"] as bool?;

      if (Project.IsFormAMat && returnToFormAMatMenu.HasValue && returnToFormAMatMenu.Value)
      {
         ReturnId = Project.FormAMatProjectId.ToString();
         ReturnPage = @Links.FormAMat.OtherSchoolsInMat.Page;
         TempData["returnToFormAMatMenu"] = true;
      }

      return result;
   }
   public string GenerateId(string heading)
   {
      return heading.Replace(" ", "_");
   }
}
