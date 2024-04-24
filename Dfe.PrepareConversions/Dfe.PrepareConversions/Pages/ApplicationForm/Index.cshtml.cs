using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ApplicationForm;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   protected readonly ApplicationRepository _applicationRepository;

   public IndexModel(IAcademyConversionProjectRepository repository, ApplicationRepository applicationRepository) : base(repository)
   {
      _applicationRepository = applicationRepository;
   }

   public IEnumerable<BaseFormSection> Sections { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await base.OnGetAsync(id);
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

      // This page should only show for join a mat application forms
      if (applicationResponse.Body.ApplicationType is not ("JoinMat" or "joinAMat"))
      {
         return StatusCode(501);
      }

      if (applicationResponse.Body.ApplyingSchools.Count != 1)
      {
         return StatusCode(500);
      }

      Application application = applicationResponse.Body;

      Sections = new BaseFormSection[]
      {
         new ApplicationFormSection(application),
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
}
