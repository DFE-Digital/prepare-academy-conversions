using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using ApplyToBecomeInternal.Services.WordDocument;
using DocumentGeneration;
using DocumentGeneration.Elements;
using DocumentGeneration.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class GenerateHTBTemplateModel : BaseAcademyConversionProjectPageModel
	{
		private readonly WordDocumentService _wordDocumentService;
		private readonly SchoolPerformanceService _schoolPerformanceService;
		private readonly GeneralInformationService _generalInformationService;
		private readonly KeyStagePerformanceService _keyStagePerformanceService;

		public GenerateHTBTemplateModel(
			WordDocumentService wordDocumentService,
			SchoolPerformanceService schoolPerformanceService,
			GeneralInformationService generalInformationService,
			IAcademyConversionProjectRepository repository,
			KeyStagePerformanceService keyStagePerformanceService) : base(repository)
		{
			_wordDocumentService = wordDocumentService;
			_schoolPerformanceService = schoolPerformanceService;
			_generalInformationService = generalInformationService;
			_keyStagePerformanceService = keyStagePerformanceService;
		}

		public string ErrorPage
		{
			get => TempData[nameof(ErrorPage)].ToString();
		}

		public override async Task<IActionResult> OnGetAsync(int id)
		{
			await base.OnGetAsync(id);
			if (Project.HeadTeacherBoardDate != null) return Page();

			TempData["ShowGenerateHtbTemplateError"] = true;
			return RedirectToPage(ErrorPage, new { id });
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
			var keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(project.Urn.ToString());

			var document = HtbTemplate.Build(response.Body, schoolPerformance, generalInformation, keyStagePerformance);
			var ms = CreateMemoryStream("htb-template");

			var documentBuilder = DocumentBuilder.CreateFromTemplate(ms, document);
			AddKeyStage2Information(documentBuilder, document, project);
			AddKeyStage5Information(documentBuilder, document, project);
			var documentByteArray = documentBuilder.Build();

			return File(documentByteArray, "application/vnd.ms-word.document", $"{document.SchoolName}-htb-template-{DateTime.Today.ToString("dd-MM-yyyy")}.docx");
		}

		private void AddKeyStage5Information(DocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
		{
			if (document.KeyStage5 == null)
			{
				documentBuilder.ReplacePlaceholderWithContent("KS5PerformanceData", builder => builder.AddParagraph(""));
				return;
			}

			documentBuilder.ReplacePlaceholderWithContent("KS5PerformanceData", builder =>
			{
				foreach (var ks5Data in document.KeyStage5)
				{
					builder.AddHeading(hBuilder =>
					{
						hBuilder.AddText($"{ks5Data.Year} scores for academic and applied general qualifications");
						hBuilder.SetHeadingLevel(HeadingLevel.Two);
					});
					
					builder.AddHeading(hBuilder =>
					{
						hBuilder.AddText($"Local authority: {project.LocalAuthority}");
						hBuilder.SetHeadingLevel(HeadingLevel.Three);
					});
					
					builder.AddTable(new List<TextElement[]>
					{
						new[]
						{
							new TextElement(), new TextElement("Academic progress") { Bold = true }, new TextElement("Academic average") { Bold = true },
							new TextElement("Applied general progress") { Bold = true }, new TextElement("Applied general average") { Bold = true },
						},
						new[]
						{
							new TextElement(project.SchoolName) { Bold = true }, new TextElement(ks5Data.AcademicProgress), new TextElement(ks5Data.AcademicAverage),
							new TextElement(ks5Data.AppliedGeneralProgress), new TextElement(ks5Data.AppliedGeneralAverage),
						},
						new[]
						{
							new TextElement("National average") { Bold = true }, new TextElement(ks5Data.NationalAverageAcademicProgress),
							new TextElement(ks5Data.NationalAverageAcademicAverage), new TextElement(ks5Data.NationalAverageAppliedGeneralProgress),
							new TextElement(ks5Data.NationalAverageAppliedGeneralAverage),
						}
					});
				}
			});
		}

		private void AddKeyStage2Information(DocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
		{
			if (document.KeyStage2 == null)
			{
				documentBuilder.ReplacePlaceholderWithContent("KS2PerformanceData", builder => builder.AddParagraph(""));
				return;
			}

			documentBuilder.ReplacePlaceholderWithContent("KS2PerformanceData", builder =>
			{
				foreach (var ks2Data in document.KeyStage2)
				{
					builder.AddHeading(hBuilder =>
					{
						hBuilder.AddText($"{ks2Data.Year} Key stage 2");
						hBuilder.SetHeadingLevel(HeadingLevel.Two);
					});
					
					builder.AddTable(new List<TextElement[]>
					{
						new[]
						{
							new TextElement(), new TextElement("Percentage meeting expected standard in reading, writing and maths") { Bold = true },
							new TextElement("Percentage achieving a higher standard in reading, writing and maths") { Bold = true },
							new TextElement("Reading progress scores") { Bold = true }, new TextElement("Writing progress scores") { Bold = true },
							new TextElement("Maths progress scores") { Bold = true },
						},
						new[]
						{
							new TextElement($"{project.SchoolName}") { Bold = true }, new TextElement(ks2Data.PercentageMeetingExpectedStdInRWM),
							new TextElement(ks2Data.PercentageAchievingHigherStdInRWM), new TextElement(ks2Data.ReadingProgressScore),
							new TextElement(ks2Data.WritingProgressScore), new TextElement(ks2Data.MathsProgressScore),
						},
						new[]
						{
							new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, new TextElement(ks2Data.LAAveragePercentageMeetingExpectedStdInRWM),
							new TextElement(ks2Data.LAAveragePercentageAchievingHigherStdInRWM), new TextElement(ks2Data.LAAverageReadingProgressScore),
							new TextElement(ks2Data.LAAverageWritingProgressScore), new TextElement(ks2Data.LAAverageMathsProgressScore),
						},
						new[]
						{
							new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks2Data.NationalAveragePercentageMeetingExpectedStdInRWM),
							HtmlStringToTextElement(ks2Data.NationalAveragePercentageAchievingHigherStdInRWM), new TextElement(ks2Data.NationalAverageReadingProgressScore),
							new TextElement(ks2Data.NationalAverageWritingProgressScore), new TextElement(ks2Data.NationalAverageMathsProgressScore),
						}
					});
				}
			});
		}

		private MemoryStream CreateMemoryStream(string template)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = assembly.GetManifestResourceNames()
				.FirstOrDefault(n => n.Contains(template, StringComparison.OrdinalIgnoreCase));
			using var templateStream = assembly.GetManifestResourceStream(resourceName);
			var ms = new MemoryStream();
			templateStream.CopyTo(ms);
			return ms;
		}

		private TextElement HtmlStringToTextElement(HtmlString str)
		{
			return new TextElement(str.Value.Replace("<br>", "\n"));
		}
	}
}