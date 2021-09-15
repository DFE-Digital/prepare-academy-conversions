using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services.WordDocument;
using DocumentGeneration;
using DocumentGeneration.Elements;
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
			AddKeyStage4Information(documentBuilder, document, project);
			AddKeyStage5Information(documentBuilder, document, project);
			var documentByteArray = documentBuilder.Build();

			return File(documentByteArray, "application/vnd.ms-word.document", $"{document.SchoolName}-htb-template-{DateTime.Today.ToString("dd-MM-yyyy")}.docx");
		}

		private void AddKeyStage4Information(DocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
		{
			if (document.KeyStage4 == null)
			{
				documentBuilder.ReplacePlaceholderWithContent("KS4PerformanceData", builder => builder.AddParagraph(""));
				return;
			}

			var ks4Data = document.KeyStage4;

			documentBuilder.ReplacePlaceholderWithContent("KS4PerformanceData", builder =>
			{
				builder.AddHeading("Key stage 4 performance tables", HeadingLevel.One);
				builder.AddHeading("Attainment 8", HeadingLevel.Two);
				builder.AddHeading("Attainment 8 scores", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.Attainment8Score),
						HtmlStringToTextElement(ks4Data.Attainment8ScorePreviousYear), HtmlStringToTextElement(ks4Data.Attainment8ScoreTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAverageAttainment8Score),
						HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScorePreviousYear), HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAverageAttainment8Score),
						HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScorePreviousYear),
						HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreTwoYearsAgo)
					},
				});
				builder.AddParagraph("");
				
				builder.AddHeading("Attainment 8 English", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglish),
						HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglishPreviousYear), HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglishTwoYearsAgo)
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglish),
						HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglishPreviousYear),
						HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglishTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglish),
						HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglishPreviousYear),
						HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglishTwoYearsAgo),
					},
				});
				builder.AddParagraph("");

				builder.AddHeading("Attainment 8 English", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.Attainment8ScoreMaths),
						HtmlStringToTextElement(ks4Data.Attainment8ScoreMathsPreviousYear), HtmlStringToTextElement(ks4Data.Attainment8ScoreMathsTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMaths),
						HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMathsPreviousYear), HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMathsTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMaths),
						HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMathsPreviousYear),
						HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMathsTwoYearsAgo),
					},
				});
				builder.AddParagraph("");

				builder.AddHeading("Attainment 8 Ebacc", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true },
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.Attainment8ScoreEbacc),
						HtmlStringToTextElement(ks4Data.Attainment8ScoreEbaccPreviousYear), HtmlStringToTextElement(ks4Data.Attainment8ScoreEbaccTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbacc),
						HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbaccPreviousYear),
						HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbaccTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbacc),
						HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbaccPreviousYear),
						HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbaccTwoYearsAgo),
					}
				});
				builder.AddParagraph("");

				builder.AddHeading("Progress 8", HeadingLevel.Two);
				builder.AddHeading("Pupils included in P8", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true },
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8),
						HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8PreviousYear), HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8TwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8),
						HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8PreviousYear),
						HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8TwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8),
						HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8PreviousYear),
						HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8TwoYearsAgo),
					},
				});
				builder.AddParagraph("");

				builder.AddHeading("School progress scores", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true },
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.Progress8Score),
						HtmlStringToTextElement(ks4Data.Progress8ScorePreviousYear), HtmlStringToTextElement(ks4Data.Progress8ScoreTwoYearsAgo),
					},
					new[]
					{
						new TextElement("School confidence interval") { Bold = true }, new TextElement(ks4Data.Progress8ConfidenceInterval),
						new TextElement(ks4Data.Progress8ConfidenceIntervalPreviousYear), new TextElement(ks4Data.Progress8ConfidenceIntervalTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAverageProgress8Score),
						HtmlStringToTextElement(ks4Data.LaAverageProgress8ScorePreviousYear), HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA confidence interval") { Bold = true }, new TextElement(ks4Data.LaAverageProgress8ConfidenceInterval),
						new TextElement(ks4Data.LaAverageProgress8ConfidenceIntervalPreviousYear), new TextElement(ks4Data.LaAverageProgress8ConfidenceIntervalTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAverageProgress8Score),
						HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScorePreviousYear), HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National LA confidence interval") { Bold = true }, new TextElement(ks4Data.NationalAverageProgress8ConfidenceInterval),
						new TextElement(ks4Data.NationalAverageProgress8ConfidenceIntervalPreviousYear),
						new TextElement(ks4Data.NationalAverageProgress8ConfidenceIntervalTwoYearsAgo),
					},
				});
				builder.AddParagraph("");

				builder.AddHeading("Progress 8 English", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true },
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.Progress8ScoreEnglish),
						HtmlStringToTextElement(ks4Data.Progress8ScoreEnglishPreviousYear), HtmlStringToTextElement(ks4Data.Progress8ScoreEnglishTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglish),
						HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglishPreviousYear),
						HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglishTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglish),
						HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglishPreviousYear),
						HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglishTwoYearsAgo),
					}
				});
				builder.AddParagraph("");

				builder.AddHeading("Progress 8 Maths", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true },
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.Progress8ScoreMaths),
						HtmlStringToTextElement(ks4Data.Progress8ScoreMathsPreviousYear), HtmlStringToTextElement(ks4Data.Progress8ScoreMathsTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMaths),
						HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMathsPreviousYear), HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMathsTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMaths),
						HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMathsPreviousYear),
						HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMathsTwoYearsAgo),
					}
				});
				builder.AddParagraph("");

				builder.AddHeading("Progress 8 Ebacc", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true },
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, HtmlStringToTextElement(ks4Data.Progress8ScoreEbacc),
						HtmlStringToTextElement(ks4Data.Progress8ScoreEbaccPreviousYear), HtmlStringToTextElement(ks4Data.Progress8ScoreEbaccTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbacc),
						HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbaccPreviousYear), HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbaccTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbacc),
						HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbaccPreviousYear),
						HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbaccTwoYearsAgo),
					}
				});
				builder.AddParagraph("");

				builder.AddHeading("Percentage of students entering EBacc", HeadingLevel.Three);
				builder.AddTable(new List<TextElement[]>
				{
					new[]
					{
						new TextElement { Bold = true }, new TextElement(ks4Data.Year) { Bold = true }, new TextElement(ks4Data.PreviousYear) { Bold = true },
						new TextElement(ks4Data.TwoYearsAgo) { Bold = true },
					},
					new[]
					{
						new TextElement(project.SchoolName) { Bold = true }, new TextElement(ks4Data.PercentageEnteringEbacc),
						new TextElement(ks4Data.PercentageEnteringEbaccPreviousYear), new TextElement(ks4Data.PercentageEnteringEbaccTwoYearsAgo),
					},
					new[]
					{
						new TextElement($"{project.LocalAuthority} LA average") { Bold = true }, new TextElement(ks4Data.LaPercentageEnteringEbacc),
						new TextElement(ks4Data.LaPercentageEnteringEbaccPreviousYear), new TextElement(ks4Data.LaPercentageEnteringEbaccTwoYearsAgo),
					},
					new[]
					{
						new TextElement("National average") { Bold = true }, new TextElement(ks4Data.NaPercentageEnteringEbacc),
						new TextElement(ks4Data.NaPercentageEnteringEbaccPreviousYear), new TextElement(ks4Data.NaPercentageEnteringEbaccTwoYearsAgo),
					}
				});

				builder.AddParagraph("");

				builder.AddTable(new List<TextElement[]>
				{
					new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.KeyStage4PerformanceAdditionalInformation) }
				});
				builder.AddParagraph("");
			});
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
				builder.AddHeading("Key stage 5 performance tables", HeadingLevel.One);
				
				foreach (var ks5Data in document.KeyStage5)
				{
					builder.AddHeading($"{ks5Data.Year} scores for academic and applied general qualifications", HeadingLevel.Two);
					builder.AddHeading($"Local authority: {project.LocalAuthority}", HeadingLevel.Three);

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
					
					builder.AddParagraph("");
				}

				builder.AddTable(new List<TextElement[]>
				{
					new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.KeyStage5PerformanceAdditionalInformation) }
				});
				builder.AddParagraph("");
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
				builder.AddHeading("Key stage 2 performance tables", HeadingLevel.One);
				
				foreach (var ks2Data in document.KeyStage2)
				{
					builder.AddHeading($"{ks2Data.Year} Key stage 2", HeadingLevel.Two);
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
					builder.AddParagraph("");
				}

				builder.AddTable(new List<TextElement[]>
				{
					new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.KeyStage2PerformanceAdditionalInformation) }
				});
				builder.AddParagraph("");
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