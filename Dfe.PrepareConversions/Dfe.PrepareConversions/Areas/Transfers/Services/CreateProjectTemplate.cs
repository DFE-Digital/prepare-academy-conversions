using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Helpers;
using Dfe.PrepareTransfers.Web.Models.ProjectTemplate;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareTransfers.Web.Services.Responses;
using static Dfe.PrepareTransfers.Web.Services.DocumentGenerators.TrustInformationGenerator;
using static Dfe.PrepareTransfers.Web.Services.DocumentGenerators.RisksGenerator;
using static Dfe.PrepareTransfers.Web.Services.DocumentGenerators.BenefitsGenerator;
using static Dfe.PrepareTransfers.Web.Services.DocumentGenerators.RationaleGenerator;
using static Dfe.PrepareTransfers.Web.Services.DocumentGenerators.LegalRequirementsGenerator;
using static Dfe.PrepareTransfers.Web.Services.DocumentGenerators.TransferFeaturesGenerator;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;

namespace Dfe.PrepareTransfers.Web.Services
{
    public class CreateProjectTemplate : ICreateProjectTemplate
    {
        private readonly IGetProjectTemplateModel _getProjectTemplateModel;
        private const string LocalAuthority = "local authority";

        public CreateProjectTemplate(IGetProjectTemplateModel getProjectTemplateModel)
        {
            _getProjectTemplateModel = getProjectTemplateModel;
        }

        private static MemoryStream CreateMemoryStream(string template)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.Contains(template, StringComparison.OrdinalIgnoreCase));
            using var templateStream = assembly.GetManifestResourceStream(resourceName);
            var ms = new MemoryStream();
            templateStream.CopyTo(ms);
            return ms;
        }

        public async Task<CreateProjectTemplateResponse> Execute(string projectUrn)
        {
            var getHtbDocumentForProject = await _getProjectTemplateModel.Execute(projectUrn);
            if (!getHtbDocumentForProject.IsValid)
            {
                return CreateErrorResponse(getHtbDocumentForProject.ResponseError);
            }

            var projectTemplateModel = getHtbDocumentForProject.ProjectTemplateModel;

            var ms = CreateMemoryStream("transfers-htb-template");
            var builder = DocumentBuilder.CreateFromTemplate(ms, projectTemplateModel);

            BuildTitle(builder, projectTemplateModel);
            AddTrustInformationDetail(builder, projectTemplateModel);
            AddFeaturesDetail(builder, projectTemplateModel);
            AddBenefits(builder, projectTemplateModel);
            AddRisks(builder, projectTemplateModel);
            AddRationale(builder, projectTemplateModel);
            AddLegalRequirementsDetail(builder, projectTemplateModel);
            BuildAcademyData(builder, projectTemplateModel.Academies);

            return new CreateProjectTemplateResponse
            {
                Document = builder.Build()
            };
        }
        
        private static void BuildAcademyData(DocumentBuilder documentBuilder, List<ProjectTemplateAcademyModel> academies)
        {
            documentBuilder.ReplacePlaceholderWithContent("AcademySection", builder =>
            {
                foreach (var academy in academies)
                {
                    builder.AddHeading(hBuilder =>
                    {
                        hBuilder.SetHeadingLevel(HeadingLevel.Two);
                        hBuilder.AddText(new TextElement {Value = academy.SchoolName, Bold = true});
                    });

                    BuildGeneralInformation(builder, academy);
                    BuildPupilNumbers(builder, academy);
                    builder.AddParagraph(pBuilder => pBuilder.AddPageBreak());
                }
            });
        }

        private static void BuildTitle(IDocumentBuilder documentBuilder, ProjectTemplateModel projectTemplateModel)
        {
            documentBuilder.ReplacePlaceholderWithContent("HtbTemplateTitle", builder =>
            {
                builder.AddHeading(hBuilder =>
                {
                    hBuilder.SetHeadingLevel(HeadingLevel.One);
                    hBuilder.AddText(new TextElement
                    {
                        Value = "Project template for:\n" +
                                $"Incoming trust: {projectTemplateModel.IncomingTrustName} (UKPRN: {projectTemplateModel.IncomingTrustUkprn})\n \n" +
                                $"Project reference: {projectTemplateModel.ProjectReference}",
                        Bold = true
                    });
                });
            });
        }

        private static void BuildGeneralInformation(IDocumentBodyBuilder builder, ProjectTemplateAcademyModel academy)
        {
            builder.AddHeading(hBuilder =>
            {
                hBuilder.SetHeadingLevel(HeadingLevel.Three);
                hBuilder.AddText(new TextElement {Value = "General information", Bold = true});
            });

            builder.AddTable(new[]
            {
                new[]
                {
                    new TextElement {Value = "School type", Bold = true},
                    new TextElement {Value = academy.SchoolType},
                },
                new[]
                {
                    new TextElement {Value = "School phase", Bold = true},
                    new TextElement {Value = academy.SchoolPhase},
                },
                new[]
                {
                    new TextElement {Value = "Age range", Bold = true},
                    new TextElement {Value = academy.AgeRange},
                },
                new[]
                {
                    new TextElement {Value = "Capacity", Bold = true},
                    new TextElement {Value = academy.SchoolCapacity},
                },
                new[]
                {
                    new TextElement {Value = "Published admission number (PAN)", Bold = true},
                    new TextElement {Value = academy.PublishedAdmissionNumber},
                },
                new[]
                {
                    new TextElement {Value = "Number on roll (percentage the school is full)", Bold = true},
                    new TextElement {Value = $"{academy.NumberOnRoll}"},
                },
                new[]
                {
                    new TextElement {Value = "Percentage of free school meals (%FSM)", Bold = true},
                    new TextElement {Value = $"{academy.PercentageFreeSchoolMeals}"},
                },
                new[]
                {
                    new TextElement {Value = "Viability issues", Bold = true},
                    new TextElement {Value = academy.ViabilityIssues},
                },
                new[]
                {
                    new TextElement {Value = "Financial deficit", Bold = true},
                    new TextElement {Value = academy.FinancialDeficit},
                },
                new[]
                {
                    new TextElement {Value = "Private finance initiative (PFI) scheme", Bold = true},
                    new TextElement {Value = $"{academy.Pfi}"},
                },
                new[]
                {
                    new TextElement
                        {Value = "Percentage of good or outstanding schools in the diocesan trust", Bold = true},
                    new TextElement {Value = academy.PercentageGoodOrOutstandingInDiocesanTrust},
                },
                new[]
                {
                    new TextElement {Value = "Distance from the academy to the trust headquarters", Bold = true},
                    new TextElement {Value = academy.DistanceFromTheAcademyToTheTrustHeadquarters},
                },
                new[]
                {
                    new TextElement {Value = "MP (party)", Bold = true},
                    new TextElement {Value = $"{academy.MpAndParty}"},
                }
            });
        }

        private static void BuildPupilNumbers(IDocumentBodyBuilder builder, ProjectTemplateAcademyModel academy)
        {
            builder.AddParagraph(pBuilder => pBuilder.AddPageBreak());

            builder.AddHeading(hBuilder =>
            {
                hBuilder.SetHeadingLevel(HeadingLevel.Three);
                hBuilder.AddText(new TextElement {Value = "Pupil numbers", Bold = true});
            });

            builder.AddTable(new[]
            {
                new[]
                {
                    new TextElement {Value = "Girls on roll", Bold = true},
                    new TextElement {Value = academy.GirlsOnRoll},
                },
                new[]
                {
                    new TextElement {Value = "Boys on roll", Bold = true},
                    new TextElement {Value = academy.BoysOnRoll},
                },
                new[]
                {
                    new TextElement
                        {Value = "Pupils with a statement of special educational needs (SEN)", Bold = true},
                    new TextElement {Value = academy.PupilsWithSen},
                },
                new[]
                {
                    new TextElement {Value = "Pupils whose first language is not English", Bold = true},
                    new TextElement {Value = academy.PupilsWithFirstLanguageNotEnglish},
                },
                new[]
                {
                    new TextElement
                        {Value = "Pupils eligible for free school meals during the past 6 years", Bold = true},
                    new TextElement {Value = academy.PupilsFsm6Years},
                },
                new[]
                {
                    new TextElement {Value = "Additional information", Bold = true},
                    new TextElement {Value = academy.PupilNumbersAdditionalInformation},
                }
            });
        }

        private static CreateProjectTemplateResponse CreateErrorResponse(
            ServiceResponseError serviceResponseError)
        {
            if (serviceResponseError.ErrorCode == ErrorCode.NotFound)
            {
                return new CreateProjectTemplateResponse
                {
                    ResponseError = new ServiceResponseError
                    {
                        ErrorCode = ErrorCode.NotFound,
                        ErrorMessage = "Not found"
                    }
                };
            }

            return new CreateProjectTemplateResponse
            {
                ResponseError = new ServiceResponseError
                {
                    ErrorCode = ErrorCode.ApiError,
                    ErrorMessage = "API has encountered an error"
                }
            };
        }
        private static TextElement[] KeyStageStatus()
        {
            return new[]
            {
                new TextElement("Status") { Bold = true }, new TextElement(KeyStage4DataStatusHelper.DetermineKeyStageDataStatus(DateTime.Now)) { Bold = true },
                new TextElement(KeyStage4DataStatusHelper.DetermineKeyStageDataStatus(DateTime.Now.AddYears(-1))) { Bold = true },
                new TextElement(KeyStage4DataStatusHelper.DetermineKeyStageDataStatus(DateTime.Now.AddYears(-2))) { Bold = true }
            };
        }
    }
}