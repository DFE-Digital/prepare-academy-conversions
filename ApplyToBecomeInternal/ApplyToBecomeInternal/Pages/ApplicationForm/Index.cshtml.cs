using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Pages.ApplicationForm
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		protected readonly ApplicationRepository _applicationRepository;
		public IEnumerable<BaseFormSection> Sections { get; set; }

		public IndexModel(IAcademyConversionProjectRepository repository, ApplicationRepository applicationRepository) : base(repository)
		{
			_applicationRepository = applicationRepository;
		}
		
		public override async Task<IActionResult> OnGetAsync(int id)
        {
			var result = await base.OnGetAsync(id);
			if ((result as StatusCodeResult)?.StatusCode == (int)HttpStatusCode.NotFound)
			{
				return NotFound();
			}
			var applicationReference = base.Project.ApplicationReferenceNumber;

			var applicationResponse = await _applicationRepository.GetApplicationByReference(applicationReference);

			if (!applicationResponse.Success)
			{
				return NotFound();
			}
			if (applicationResponse.Body.ApplicationType != "JoinMat")
			{
				return StatusCode(501);
			}
			if (applicationResponse.Body.ApplyingSchools.Count != 1)
			{
				return StatusCode(500);
			}
			var application = applicationResponse.Body;

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
}
