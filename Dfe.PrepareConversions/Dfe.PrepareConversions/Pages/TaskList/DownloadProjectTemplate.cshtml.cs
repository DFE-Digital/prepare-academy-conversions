using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
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
   private readonly GeneralInformationService _generalInformationService;
   private readonly KeyStagePerformanceService _keyStagePerformanceService;
   private readonly SchoolPerformanceService _schoolPerformanceService;

   public DownloadProjectTemplate(SchoolPerformanceService schoolPerformanceService,
                                  GeneralInformationService generalInformationService,
                                  IAcademyConversionProjectRepository repository,
                                  KeyStagePerformanceService keyStagePerformanceService) : base(repository)
   {
      _schoolPerformanceService = schoolPerformanceService;
      _generalInformationService = generalInformationService;
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
      GeneralInformation generalInformation = await _generalInformationService.GetGeneralInformationByUrn(project.Urn.ToString());
      KeyStagePerformance keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(project.Urn.ToString());

      HtbTemplate document = HtbTemplate.Build(response.Body, schoolPerformance, generalInformation, keyStagePerformance);
      MemoryStream ms = CreateMemoryStream("htb-template");

      DocumentBuilder documentBuilder = DocumentBuilder.CreateFromTemplate(ms, document);
      AddOfstedInformation(documentBuilder, document, project);
      AddKeyStage2Information(documentBuilder, document, project);
      AddKeyStage4Information(documentBuilder, document, project);
      AddKeyStage5Information(documentBuilder, document, project);
      byte[] documentByteArray = documentBuilder.Build();

      return File(documentByteArray, "application/vnd.ms-word.document", $"{document.SchoolName}-project-template-{DateTime.Today:dd-MM-yyyy}.docx");
   }

   private static void AddOfstedInformation(DocumentBuilder builder, HtbTemplate document, AcademyConversionProject project)
   {
      SchoolPerformance schoolPerformance = document.SchoolPerformance;

      List<TextElement[]> ofstedInformation = new()
      {
         new[] { new TextElement { Value = "School name", Bold = true }, new TextElement { Value = project.SchoolName } },
         new[]
         {
            new TextElement { Value = "Latest full inspection date", Bold = true },
            new TextElement { Value = schoolPerformance.InspectionEndDate?.ToString("d MMMM yyyy") ?? "No data" }
         },
         new[] { new TextElement { Value = "Overall effectiveness", Bold = true }, new TextElement { Value = schoolPerformance.OverallEffectiveness.DisplayOfstedRating() } },
         new[] { new TextElement { Value = "Quality of education", Bold = true }, new TextElement { Value = schoolPerformance.QualityOfEducation.DisplayOfstedRating() } },
         new[] { new TextElement { Value = "Behaviour and attitudes", Bold = true }, new TextElement { Value = schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating() } },
         new[] { new TextElement { Value = "Personal development", Bold = true }, new TextElement { Value = schoolPerformance.PersonalDevelopment.DisplayOfstedRating() } },
         new[]
         {
            new TextElement { Value = "Effectiveness of leadership and management", Bold = true },
            new TextElement { Value = schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating() }
         }
      };

      if (schoolPerformance.LatestInspectionIsSection8)
      {
         ofstedInformation.Insert(1,
            new[]
            {
               new TextElement { Value = "Latest short inspection date", Bold = true },
               new TextElement { Value = schoolPerformance.DateOfLatestSection8Inspection?.ToString("d MMMM yyyy") }
            });
      }

      if (schoolPerformance.EarlyYearsProvision.DisplayOfstedRating().HasData())
      {
         ofstedInformation.Add(new[]
         {
            new TextElement { Value = "Early years provision", Bold = true }, new TextElement { Value = schoolPerformance.EarlyYearsProvision.DisplayOfstedRating() }
         });
      }

      if (schoolPerformance.SixthFormProvision.DisplayOfstedRating().HasData())
      {
         ofstedInformation.Add(new[]
         {
            new TextElement { Value = "Sixth form provision", Bold = true }, new TextElement { Value = schoolPerformance.SixthFormProvision.DisplayOfstedRating() }
         });
      }

      ofstedInformation.Add(new[]
      {
         new TextElement { Value = "Additional information", Bold = true }, new TextElement { Value = project.SchoolPerformanceAdditionalInformation }
      });

      builder.ReplacePlaceholderWithContent("SchoolPerformanceData", body => body.AddTable(ofstedInformation));
   }

   private static void AddKeyStage4Information(DocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
   {
      if (document.KeyStage4 == null)
      {
         documentBuilder.ReplacePlaceholderWithContent("KS4PerformanceData", builder => builder.AddParagraph(""));
         return;
      }

      KeyStage4PerformanceTableViewModel ks4Data = document.KeyStage4;

      documentBuilder.ReplacePlaceholderWithContent("KS4PerformanceData", builder =>
      {
         builder.AddHeading("Key stage 4 performance tables", HeadingLevel.One);
         builder.AddHeading("Attainment 8", HeadingLevel.Two);
         builder.AddHeading("Attainment 8 scores", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Attainment8Score),
               HtmlStringToTextElement(ks4Data.Attainment8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8Score),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8Score),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreTwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("Attainment 8 English", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglishTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglishTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglishTwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("Attainment 8 Maths", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Attainment8ScoreMaths),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreMathsTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMaths),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMathsTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMaths),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMathsTwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("Attainment 8 Ebacc", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbaccTwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("Progress 8", HeadingLevel.Two);
         builder.AddHeading("Pupils included in P8", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8),
               HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8PreviousYear),
               HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8TwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8),
               HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8PreviousYear),
               HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8TwoYearsAgo)
            },
            new[]
            {
               new TextElement("National") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8),
               HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8PreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8TwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("School progress scores", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Progress8Score),
               HtmlStringToTextElement(ks4Data.Progress8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.Progress8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement("School confidence interval") { Bold = true },
               new TextElement(ks4Data.Progress8ConfidenceInterval),
               new TextElement(ks4Data.Progress8ConfidenceIntervalPreviousYear),
               new TextElement(ks4Data.Progress8ConfidenceIntervalTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageProgress8Score),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA confidence interval") { Bold = true },
               new TextElement(ks4Data.LaAverageProgress8ConfidenceInterval),
               new TextElement(ks4Data.LaAverageProgress8ConfidenceIntervalPreviousYear),
               new TextElement(ks4Data.LaAverageProgress8ConfidenceIntervalTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8Score),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National LA confidence interval") { Bold = true },
               new TextElement(ks4Data.NationalAverageProgress8ConfidenceInterval),
               new TextElement(ks4Data.NationalAverageProgress8ConfidenceIntervalPreviousYear),
               new TextElement(ks4Data.NationalAverageProgress8ConfidenceIntervalTwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("Progress 8 English", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Progress8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.Progress8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.Progress8ScoreEnglishTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglishTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglishTwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("Progress 8 Maths", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Progress8ScoreMaths),
               HtmlStringToTextElement(ks4Data.Progress8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.Progress8ScoreMathsTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMaths),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMathsTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMaths),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMathsTwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("Progress 8 Ebacc", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Progress8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.Progress8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.Progress8ScoreEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbaccTwoYearsAgo)
            }
         });
         builder.AddParagraph("");

         builder.AddHeading("Percentage of students entering EBacc", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               new TextElement(ks4Data.PercentageEnteringEbacc),
               new TextElement(ks4Data.PercentageEnteringEbaccPreviousYear),
               new TextElement(ks4Data.PercentageEnteringEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               new TextElement(ks4Data.LaPercentageEnteringEbacc),
               new TextElement(ks4Data.LaPercentageEnteringEbaccPreviousYear),
               new TextElement(ks4Data.LaPercentageEnteringEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               new TextElement(ks4Data.NaPercentageEnteringEbacc),
               new TextElement(ks4Data.NaPercentageEnteringEbaccPreviousYear),
               new TextElement(ks4Data.NaPercentageEnteringEbaccTwoYearsAgo)
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

   private static void AddKeyStage5Information(DocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
   {
      if (document.KeyStage5 == null)
      {
         documentBuilder.ReplacePlaceholderWithContent("KS5PerformanceData", builder => builder.AddParagraph(""));
         return;
      }

      documentBuilder.ReplacePlaceholderWithContent("KS5PerformanceData", builder =>
      {
         builder.AddHeading("Key stage 5 performance tables", HeadingLevel.One);
         int yearIndex = 0;
         foreach (KeyStage5PerformanceTableViewModel ks5Data in document.KeyStage5)
         {
            builder.AddHeading($"{ks5Data.Year} scores for academic and applied general qualifications", HeadingLevel.Two);
            builder.AddHeading($"Local authority: {project.LocalAuthority}", HeadingLevel.Three);
            builder.AddTable(new List<TextElement[]>
            {
               new[]
               {
                  new TextElement($"{KeyStageHeaderStatus(KeyStages.KS5, yearIndex)}"){ Bold = true },
                  new TextElement("Academic progress") { Bold = true },
                  new TextElement("Academic average") { Bold = true },
                  new TextElement("Applied general progress") { Bold = true },
                  new TextElement("Applied general average") { Bold = true }
               },
               new[]
               {
                  new TextElement(project.SchoolName) { Bold = true },
                  new TextElement(ks5Data.AcademicProgress),
                  new TextElement(ks5Data.AcademicAverage),
                  new TextElement(ks5Data.AppliedGeneralProgress),
                  new TextElement(ks5Data.AppliedGeneralAverage)
               },
               new[]
               {
                  new TextElement("National average") { Bold = true },
                  new TextElement(ks5Data.NationalAverageAcademicProgress),
                  new TextElement(ks5Data.NationalAverageAcademicAverage),
                  new TextElement(ks5Data.NationalAverageAppliedGeneralProgress),
                  new TextElement(ks5Data.NationalAverageAppliedGeneralAverage)
               }
            });
            yearIndex++;
            builder.AddParagraph("");
         }

         builder.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.KeyStage5PerformanceAdditionalInformation) }
         });
         builder.AddParagraph("");
      });
   }

   private static void AddKeyStage2Information(DocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
   {
      if (document.KeyStage2 == null)
      {
         documentBuilder.ReplacePlaceholderWithContent("KS2PerformanceData", builder => builder.AddParagraph(""));
         return;
      }

      documentBuilder.ReplacePlaceholderWithContent("KS2PerformanceData", builder =>
      {
         builder.AddHeading("Key stage 2 performance tables", HeadingLevel.One);
         int yearIndex = 0;
         foreach (KeyStage2PerformanceTableViewModel ks2Data in document.KeyStage2)
         {
            builder.AddHeading($"{ks2Data.Year} key stage 2", HeadingLevel.Two);
            builder.AddTable(new List<TextElement[]>
            {
               new[]
               {
                  new TextElement($"{KeyStageHeaderStatus(KeyStages.KS2, yearIndex)}"){ Bold = true },
                  new TextElement("Percentage meeting expected standard in reading, writing and maths") { Bold = true },
                  new TextElement("Percentage achieving a higher standard in reading, writing and maths") { Bold = true },
                  new TextElement("Reading progress scores") { Bold = true },
                  new TextElement("Writing progress scores") { Bold = true },
                  new TextElement("Maths progress scores") { Bold = true }
               },
               new[]
               {
                  new TextElement($"{project.SchoolName}") { Bold = true },
                  new TextElement(ks2Data.PercentageMeetingExpectedStdInRWM),
                  new TextElement(ks2Data.PercentageAchievingHigherStdInRWM),
                  new TextElement(ks2Data.ReadingProgressScore),
                  new TextElement(ks2Data.WritingProgressScore),
                  new TextElement(ks2Data.MathsProgressScore)
               },
               new[]
               {
                  new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
                  new TextElement(ks2Data.LAAveragePercentageMeetingExpectedStdInRWM),
                  new TextElement(ks2Data.LAAveragePercentageAchievingHigherStdInRWM),
                  new TextElement(ks2Data.LAAverageReadingProgressScore),
                  new TextElement(ks2Data.LAAverageWritingProgressScore),
                  new TextElement(ks2Data.LAAverageMathsProgressScore)
               },
               new[]
               {
                  new TextElement("National average") { Bold = true },
                  HtmlStringToTextElement(ks2Data.NationalAveragePercentageMeetingExpectedStdInRWM),
                  HtmlStringToTextElement(ks2Data.NationalAveragePercentageAchievingHigherStdInRWM),
                  new TextElement(ks2Data.NationalAverageReadingProgressScore),
                  new TextElement(ks2Data.NationalAverageWritingProgressScore),
                  new TextElement(ks2Data.NationalAverageMathsProgressScore)
               }
            });
            builder.AddParagraph("");
            yearIndex++;
         }

         builder.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.KeyStage2PerformanceAdditionalInformation) }
         });
         builder.AddParagraph("");
      });
   }

   private static MemoryStream CreateMemoryStream(string template)
   {
      Assembly assembly = Assembly.GetExecutingAssembly();
      string resourceName = assembly.GetManifestResourceNames()
         .FirstOrDefault(n => n.Contains(template, StringComparison.OrdinalIgnoreCase));
      using Stream templateStream = assembly.GetManifestResourceStream(resourceName!);
      MemoryStream ms = new();
      templateStream!.CopyTo(ms);
      return ms;
   }

   private static TextElement HtmlStringToTextElement(HtmlString str)
   {
      return new TextElement(str.Value!.Replace("<br>", "\n"));
   }

   private static TextElement[] KeyStage4Status()
   {
      return new[]
      {
         new TextElement("Status") { Bold = true },
         new TextElement(DetermineKeyStageDataStatus(DateTime.Now, KeyStages.KS4)) { Bold = true },
         new TextElement(DetermineKeyStageDataStatus(DateTime.Now.AddYears(-1), KeyStages.KS4)) { Bold = true },
         new TextElement(DetermineKeyStageDataStatus(DateTime.Now.AddYears(-2), KeyStages.KS4)) { Bold = true }
      };
   }
   private static string KeyStageHeaderStatus(KeyStages keyStage, int yearIndex)
   {
      string statusType = DetermineStatusType(yearIndex, DateTime.Now, keyStage);
      return ("Status: " + statusType);
   }
}
