using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ProjectList;
using Microsoft.AspNetCore.Mvc;
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
      ApiResponse<Application> applicationResponse = await _applicationRepository.GetApplicationByReference(applicationReferenceId);

      if (!applicationResponse.Success)
      {
         return NotFound();
      }
      if (applicationResponse.Body.ApplicationType is not ("formMat" or "formAMat"))
      {
         return StatusCode(501);
      }

      Application application = applicationResponse.Body;

      Sections = new BaseFormSection[]
      {
         new FamApplicationFormSection(application),
         new TrustInformationSection(application),
         new KeyPeopleSection(application),
         new AboutConversionSection(application.ApplyingSchools.First()),
         new FurtherInformationSection(application.ApplyingSchools.First()),
         new FinanceSection(application.ApplyingSchools.First()),
         new FuturePupilNumberSection(application.ApplyingSchools.First()),
         new LandAndBuildingsSection(application.ApplyingSchools.First()),
         new PreOpeningSupportGrantSection(application.ApplyingSchools.First()),
         new ConsultationSection(application.ApplyingSchools.First()),
         new DeclarationSection(application.ApplyingSchools.First())
      };

      return result;
   }
   public string GenerateId(string heading)
   {
      return heading.Replace(" ", "_");
   }
   public IEnumerable<BaseFormSection> ExcludeKeyPeople(IEnumerable<BaseFormSection> sections)
   {
      return sections.Where(x => x.Heading != "Key people");
   }
}
