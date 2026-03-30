using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services.DocumentGenerator;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Models.ProjectTemplate;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Dfe.PrepareTransfers.Web.Services
{
   public static class PdfGeneratorExtensions
   {
      private static IContainer TableLabelCellStyle(IContainer container)
      {
         return container
            .Border(1).Padding(4).DefaultTextStyle(x => x.FontSize(12).Bold());
      }

      private static IContainer TableDataCellStyle(IContainer container)
      {
         return container
            .Border(1).Padding(4).DefaultTextStyle(x => x.FontSize(12));
      }

      public static TextBlockDescriptor GenerateTitle(this IContainer container, string title)
      {
         return container
            .Text(title)
            .FontColor(Colors.Blue.Darken4).FontSize(20).Bold();
      }

      public static TextBlockDescriptor GenerateSectionTitle(this IContainer container, string title)
      {
         return container.PaddingTop(6).PaddingBottom(4).Text(title).FontColor(Colors.Blue.Darken4).FontSize(16).Bold();
      }

      public static void GenerateTrustInformationDetailContent(this IContainer container, ProjectTemplateModel projectTemplateModel)
      {
         container.Table(table => 
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Recommendation");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.Recommendation);

            table.Cell().Element(TableLabelCellStyle).Text("Author");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.Author);

            table.Cell().Element(TableLabelCellStyle).Text("Project name");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.ProjectName);

            table.Cell().Element(TableLabelCellStyle).Text("Date of advisory board");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.DateOfHtb);

            table.Cell().Element(TableLabelCellStyle).Text("Proposed academy transfer date");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.DateOfProposedTransfer);
         });
      }

      public static void GenerateFeaturesDetailContent(this IContainer container, ProjectTemplateModel projectTemplateModel)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Reason for this transfer");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.ReasonForTheTransfer);

            table.Cell().Element(TableLabelCellStyle).Text("What are the specific reasons for this transfer?");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.SpecificReasonsForTheTransfer);

            table.Cell().Element(TableLabelCellStyle).Text("What type of transfer is it?");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.TypeOfTransfer);
         });
      }

      public static void GenerateBenefitsContent(this IContainer container, ProjectTemplateModel projectTemplateModel)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
            });

            if (string.IsNullOrEmpty(projectTemplateModel.TransferBenefits))
            {
               table.Cell().Element(TableDataCellStyle).Text("N/A");
            }
            else
            {
               foreach (var item in projectTemplateModel.ListOfTransferBenefits)
               {
                  var displayedOption = item == TransferBenefits.IntendedBenefit.Other ? $"Other: {projectTemplateModel.OtherIntendedBenefit}" : EnumHelpers<TransferBenefits.IntendedBenefit>.GetDisplayValue(item);

                  table.Cell().Element(TableDataCellStyle).Text(displayedOption);
               }
            }
         });
      }

      public static void GenerateRisksContent(this IContainer container, ProjectTemplateModel projectTemplateModel)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            if (projectTemplateModel.AnyIdentifiedRisks.Equals(true))
            {
               foreach (var item in projectTemplateModel.ListOfOtherFactors)
               {
                  table.Cell().Element(TableLabelCellStyle).Text(EnumHelpers<TransferBenefits.OtherFactor>.GetDisplayValue(item.Key));
                  table.Cell().Element(TableDataCellStyle).Text(item.Value);
               }
            }
            else
            {
               table.Cell().Element(TableLabelCellStyle).Text("Risks");
               table.Cell().Element(TableDataCellStyle).Text("No risks Identified");
            }

            table.Cell().Element(TableLabelCellStyle).Text("Equalities impact assessment considered");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.EqualitiesImpactAssessmentConsidered);
         });
      }

      public static void GenerateLegalRequirementsDetailContent(this IContainer container, ProjectTemplateModel projectTemplateModel)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Outgoing trust resolution");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.OutgoingTrustConsent);

            table.Cell().Element(TableLabelCellStyle).Text("Incoming trust agreement");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.IncomingTrustAgreement);

            table.Cell().Element(TableLabelCellStyle).Text("Diocesan consent");
            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.DiocesanConsent);
         });
      }

      public static void GeneratePublicSectorEqualityDutyContent(this IContainer container, ProjectTemplateModel projectTemplateModel)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Public Sector Equality Duty consideration");
            table.Cell().Element(TableDataCellStyle).Text(PreviewPublicSectorEqualityDutyModel.GenerateReduceImpactReasonLabel(projectTemplateModel.PublicEqualityDutyImpact));

            if (!string.IsNullOrWhiteSpace(projectTemplateModel.PublicEqualityDutyReduceImpactReason))
            {
               table.Cell().Element(TableLabelCellStyle).Text("What will be done to reduce this impact?");
               table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.PublicEqualityDutyReduceImpactReason);
            }
         });
      }

      public static void GenerateRationaleForProjectContent(this IContainer container, ProjectTemplateModel projectTemplateModel)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.RationaleForProject ?? "N/A");
         });
      }

      public static void GenerateRationaleForTrustOrSponsorContent(this IContainer container, ProjectTemplateModel projectTemplateModel)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableDataCellStyle).Text(projectTemplateModel.RationaleForTrust ?? "N/A");
         });
      }

      public static void GenerateGeneraleInformationContent(this IContainer container, ProjectTemplateAcademyModel academy)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("School type");
            table.Cell().Element(TableDataCellStyle).Text(academy.SchoolType);

            table.Cell().Element(TableLabelCellStyle).Text("School phase");
            table.Cell().Element(TableDataCellStyle).Text(academy.SchoolPhase);

            table.Cell().Element(TableLabelCellStyle).Text("Age range");
            table.Cell().Element(TableDataCellStyle).Text(academy.AgeRange);

            table.Cell().Element(TableLabelCellStyle).Text("Capacity");
            table.Cell().Element(TableDataCellStyle).Text(academy.SchoolCapacity);

            table.Cell().Element(TableLabelCellStyle).Text("Published admission number (PAN)");
            table.Cell().Element(TableDataCellStyle).Text(academy.PublishedAdmissionNumber);

            table.Cell().Element(TableLabelCellStyle).Text("Number on roll (percentage the school is full)");
            table.Cell().Element(TableDataCellStyle).Text(academy.NumberOnRoll);

            table.Cell().Element(TableLabelCellStyle).Text("Percentage of free school meals (%FSM)");
            table.Cell().Element(TableDataCellStyle).Text(academy.PercentageFreeSchoolMeals);

            table.Cell().Element(TableLabelCellStyle).Text("Viability issues");
            table.Cell().Element(TableDataCellStyle).Text(academy.ViabilityIssues);

            table.Cell().Element(TableLabelCellStyle).Text("Financial deficit");
            table.Cell().Element(TableDataCellStyle).Text(academy.FinancialDeficit);

            table.Cell().Element(TableLabelCellStyle).Text("Private finance initiative (PFI) scheme");
            table.Cell().Element(TableDataCellStyle).Text(academy.Pfi);

            table.Cell().Element(TableLabelCellStyle).Text("Percentage of good or outstanding schools in the diocesan trust");
            table.Cell().Element(TableDataCellStyle).Text(academy.PercentageGoodOrOutstandingInDiocesanTrust);

            table.Cell().Element(TableLabelCellStyle).Text("Distance from the academy to the trust headquarters");
            table.Cell().Element(TableDataCellStyle).Text(academy.DistanceFromTheAcademyToTheTrustHeadquarters);

            table.Cell().Element(TableLabelCellStyle).Text("MP (party)");
            table.Cell().Element(TableDataCellStyle).Text(academy.MpAndParty);
         });
      }

      public static void GeneratePupilNumbersContent(this IContainer container, ProjectTemplateAcademyModel academy)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Girls on roll");
            table.Cell().Element(TableDataCellStyle).Text(academy.GirlsOnRoll);

            table.Cell().Element(TableLabelCellStyle).Text("Boys on roll");
            table.Cell().Element(TableDataCellStyle).Text(academy.BoysOnRoll);

            table.Cell().Element(TableLabelCellStyle).Text("Pupils with a statement of special educational needs (SEN)");
            table.Cell().Element(TableDataCellStyle).Text(academy.PupilsWithSen);

            table.Cell().Element(TableLabelCellStyle).Text("Pupils with a statement of special educational needs (SEN)");
            table.Cell().Element(TableDataCellStyle).Text(academy.PupilsWithSen);

            table.Cell().Element(TableLabelCellStyle).Text("Pupils whose first language is not English");
            table.Cell().Element(TableDataCellStyle).Text(academy.PupilsWithFirstLanguageNotEnglish);

            table.Cell().Element(TableLabelCellStyle).Text("Pupils eligible for free school meals during the past 6 years");
            table.Cell().Element(TableDataCellStyle).Text(academy.PupilsFsm6Years);

            table.Cell().Element(TableLabelCellStyle).Text("Additional information");
            table.Cell().Element(TableDataCellStyle).Text(academy.PupilNumbersAdditionalInformation);
         });
      }
   }
}
