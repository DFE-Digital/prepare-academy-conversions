using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services.WordDocument;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class GenerateHTBTemplateModel : BaseAcademyConversionProjectPageModel
	{
		private readonly WordDocumentService _wordDocumentService;
		private readonly SchoolPerformanceService _schoolPerformanceService;
		private readonly GeneralInformationService _generalInformationService;

		public GenerateHTBTemplateModel(
			WordDocumentService wordDocumentService, 
			SchoolPerformanceService schoolPerformanceService, 
			GeneralInformationService generalInformationService, 
			IAcademyConversionProjectRepository repository) : base(repository)
		{
			_wordDocumentService = wordDocumentService;
			_schoolPerformanceService = schoolPerformanceService;
			_generalInformationService = generalInformationService;
		}

		public async Task<IActionResult> OnGetHtbTemplateAsync(int id)
		{
			var response = await _repository.GetProjectById(id);
			if (!response.Success)
			{
				return NotFound();
			}

			var project = response.Body;
			var schoolPerformance = await _schoolPerformanceService.GetSchoolPerformanceByUrn(project.Urn.ToString());
			var generalInformation = await _generalInformationService.GetGeneralInformationByUrn(project.Urn.ToString());

			var document = HtbTemplate.Build(response.Body, schoolPerformance, generalInformation);

			var documentByteArray = _wordDocumentService.Create("htb-template", document);

			return File(documentByteArray, "application/vnd.ms-word.document", $"{document.SchoolName}-htb-template-{DateTime.Today.ToString("dd-MM-yyyy")}.docx");
		}
	}
}
