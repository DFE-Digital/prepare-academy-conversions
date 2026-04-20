using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using HTMLQuestPDF.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
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

      public static TextBlockDescriptor GenerateSchoolNameAndUrn(this IContainer container, string schoolName, int schoolUrn)
      {
         return container
            .Text($"{schoolName} - URN {schoolUrn}")
            .FontColor(Colors.Blue.Darken4).FontSize(20).Bold();
      }

      public static TextBlockDescriptor GenerateTrustNameAndReferenceNumber(this IContainer container, string nameOfTrust, string trustReferenceNumber)
      {
         return container
            .Text($"{nameOfTrust} - {trustReferenceNumber}")
            .FontColor(Colors.Blue.Darken4).FontSize(16).Bold();
      }

      public static TextBlockDescriptor GenerateSectionTitle(this IContainer container, string title)
      {
         return container.Text(title).FontColor(Colors.Blue.Darken4).FontSize(16).Bold();
      }

      public static TextBlockDescriptor GenerateSectionSubTitle(this IContainer container, string subTitle)
      {
         return container
            .PaddingTop(4).PaddingBottom(4)
            .Text(subTitle).FontColor(Colors.Blue.Darken4).FontSize(14).Bold();
      }

      public static void GenerateSchoolOverviewSectionContent(this IContainer container, AcademyConversionProject project, SchoolOverview schoolOverview)
      {
         string distanceFromSchoolToTrustHeadquarters = project.DistanceFromSchoolToTrustHeadquarters != null ? $"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()} miles" : null;

         container.Table(table => 
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("School type");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.SchoolType);

            table.Cell().Element(TableLabelCellStyle).Text("School phase");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.SchoolPhase);

            table.Cell().Element(TableLabelCellStyle).Text("Age range");
            table.Cell().Element(TableDataCellStyle).Text(!string.IsNullOrEmpty(schoolOverview.AgeRangeLower) && !string.IsNullOrEmpty(schoolOverview.AgeRangeUpper) ? $"{schoolOverview.AgeRangeLower} to {schoolOverview.AgeRangeUpper}" : "");

            table.Cell().Element(TableLabelCellStyle).Text("Capacity");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.SchoolCapacity?.ToString());

            table.Cell().Element(TableLabelCellStyle).Text("Published admission number (PAN)");
            table.Cell().Element(TableDataCellStyle).Text(project.PublishedAdmissionNumber);

            table.Cell().Element(TableLabelCellStyle).Text("Number on roll (NOR)");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.NumberOnRoll?.ToString());

            table.Cell().Element(TableLabelCellStyle).Text("Percentage of the school is full");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.NumberOnRoll.AsPercentageOf(schoolOverview.SchoolCapacity));

            table.Cell().Element(TableLabelCellStyle).Text("Percentage of free school meals at the school (%FSM)");
            table.Cell().Element(TableDataCellStyle).Text(!string.IsNullOrEmpty(schoolOverview.PercentageFreeSchoolMeals) ? $"{schoolOverview.PercentageFreeSchoolMeals}%" : "");

            table.Cell().Element(TableLabelCellStyle).Text("Viability issues");
            table.Cell().Element(TableDataCellStyle).Text(project.ViabilityIssues);

            if (schoolOverview.SchoolType.Contains("pupil referral unit", System.StringComparison.CurrentCultureIgnoreCase))
            {
               table.Cell().Element(TableLabelCellStyle).Text("Which groups of pupils attend the school");
               table.Cell().Element(TableDataCellStyle).Text(SchoolOverviewHelper.MapPupilsAttendingGroup(project.PupilsAttendingGroupPermanentlyExcluded, project.PupilsAttendingGroupMedicalAndHealthNeeds, project.PupilsAttendingGroupTeenageMums));

               table.Cell().Element(TableLabelCellStyle).Text("Number of places funded for");
               table.Cell().Element(TableDataCellStyle).Text(project.NumberOfPlacesFundedFor.ToStringOrDefault());

               table.Cell().Element(TableLabelCellStyle).Text("Alternative provision");
               table.Cell().Element(TableDataCellStyle).Text(project.NumberOfAlternativeProvisionPlaces?.ToString());

               table.Cell().Element(TableLabelCellStyle).Text("SEN Unit");
               table.Cell().Element(TableDataCellStyle).Text(project.NumberOfSENUnitPlaces?.ToString());

               table.Cell().Element(TableLabelCellStyle).Text("Medical");
               table.Cell().Element(TableDataCellStyle).Text(project.NumberOfMedicalPlaces?.ToString());

               table.Cell().Element(TableLabelCellStyle).Text("Post-16");
               table.Cell().Element(TableDataCellStyle).Text(project.NumberOfPost16Places?.ToString());
            }

            if (schoolOverview.SchoolType.Contains("special", System.StringComparison.CurrentCultureIgnoreCase))
            {
               table.Cell().Element(TableLabelCellStyle).Text("Number of places funded for");
               table.Cell().Element(TableDataCellStyle).Text(project.NumberOfPlacesFundedFor.ToStringOrDefault());

               table.Cell().Element(TableLabelCellStyle).Text("Number of residential places");
               table.Cell().Element(TableDataCellStyle).Text(project.NumberOfResidentialPlaces.ToStringOrDefault());

               table.Cell().Element(TableLabelCellStyle).Text("Number of funded residential places");
               table.Cell().Element(TableDataCellStyle).Text(project.NumberOfFundedResidentialPlaces.ToStringOrDefault());
            }

            table.Cell().Element(TableLabelCellStyle).Text("Financial deficit");
            table.Cell().Element(TableDataCellStyle).Text(project.FinancialDeficit);

            table.Cell().Element(TableLabelCellStyle).Text("Private finance initiative (PFI) scheme");
            table.Cell().Element(TableDataCellStyle).Text(project.PartOfPfiScheme);

            table.Cell().Element(TableLabelCellStyle).Text("Is the school linked to a diocese?");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.IsSchoolLinkedToADiocese);

            table.Cell().Element(TableLabelCellStyle).Text("Distance from the converting school to the trust or other schools in the trust");
            table.Cell().Element(TableDataCellStyle).Text($"{distanceFromSchoolToTrustHeadquarters} {schoolOverview.DistanceFromSchoolToTrustHeadquartersAdditionalInformation}");

            table.Cell().Element(TableLabelCellStyle).Text("Parliamentary constituency");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.ParliamentaryConstituency);

            table.Cell().Element(TableLabelCellStyle).Text("MP name and political party");
            table.Cell().Element(TableDataCellStyle).Text(project.MemberOfParliamentNameAndParty);
         });
      }

      public static void GenerateBudgetInformationContent(this IContainer container, AcademyConversionProject project)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("End of current financial year");
            table.Cell().Element(TableDataCellStyle).Text(project.EndOfCurrentFinancialYear?.ToDateString());

            table.Cell().Element(TableLabelCellStyle).Text("Forecast revenue carry forward at the end of the current financial year");
            table.Cell().Element(TableDataCellStyle).Text(project.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString(true));

            table.Cell().Element(TableLabelCellStyle).Text("Forecast capital carry forward at the end of the current financial year");
            table.Cell().Element(TableDataCellStyle).Text(project.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString(true));

            table.Cell().Element(TableLabelCellStyle).Text("End of next financial year");
            table.Cell().Element(TableDataCellStyle).Text(project.EndOfNextFinancialYear?.ToDateString());

            table.Cell().Element(TableLabelCellStyle).Text("Forecast revenue carry forward at the end of the next financial year");
            table.Cell().Element(TableDataCellStyle).Text(project.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString(true));

            table.Cell().Element(TableLabelCellStyle).Text("Forecast capital carry forward at the end of the next financial year");
            table.Cell().Element(TableDataCellStyle).Text(project.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString(true));

            table.Cell().Element(TableLabelCellStyle).Text("Additional information");
            table.Cell().Element(TableDataCellStyle).Text(project.SchoolBudgetInformationAdditionalInformation);
         });
      }

      public static void GeneratePupilForecastsContent(this IContainer container, AcademyConversionProject project, SchoolOverview schoolOverview)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Year");
            table.Cell().Element(TableLabelCellStyle).Text("Capacity");
            table.Cell().Element(TableLabelCellStyle).Text("Total pupil numbers");
            table.Cell().Element(TableLabelCellStyle).Text("Percentage full");

            table.Cell().Element(TableLabelCellStyle).Text("Current year");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.SchoolCapacity?.ToString());
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.NumberOnRoll?.ToString());
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.NumberOnRoll.AsPercentageOf(schoolOverview.SchoolCapacity));

            table.Cell().Element(TableLabelCellStyle).Text("Projected pupil numbers on roll (year 1)");
            table.Cell().Element(TableDataCellStyle).Text(project.YearOneProjectedCapacity.ToString());
            table.Cell().Element(TableDataCellStyle).Text(project.YearOneProjectedPupilNumbers.ToStringOrDefault());
            table.Cell().Element(TableDataCellStyle).Text(project.YearOneProjectedPupilNumbers.AsPercentageOf(project.YearOneProjectedCapacity));

            table.Cell().Element(TableLabelCellStyle).Text("Projected pupil numbers on roll (year 2)");
            table.Cell().Element(TableDataCellStyle).Text(project.YearTwoProjectedCapacity.ToString());
            table.Cell().Element(TableDataCellStyle).Text(project.YearTwoProjectedPupilNumbers.ToString());
            table.Cell().Element(TableDataCellStyle).Text(project.YearTwoProjectedPupilNumbers.AsPercentageOf(project.YearTwoProjectedCapacity));

            table.Cell().Element(TableLabelCellStyle).Text("Projected pupil numbers on roll (year 3)");
            table.Cell().Element(TableDataCellStyle).Text(project.YearThreeProjectedCapacity.ToString());
            table.Cell().Element(TableDataCellStyle).Text(project.YearThreeProjectedPupilNumbers.ToString());
            table.Cell().Element(TableDataCellStyle).Text(project.YearThreeProjectedPupilNumbers.AsPercentageOf(project.YearThreeProjectedCapacity));

         });
      }
      public static void GeneratePupilForecastsAddionalInfoContent(this IContainer container, AcademyConversionProject project)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Additional information");
            table.Cell().Element(TableDataCellStyle).Text(project.SchoolPupilForecastsAdditionalInformation);
         });
      }

      public static void GenerateConversionDetailsContent(this IContainer container, AcademyConversionProject project)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Academy type and route");
            table.Cell().Element(TableDataCellStyle).Text(project.AcademyTypeAndRoute);

            switch (project.AcademyTypeAndRoute)
            {
               case AcademyTypeAndRoutes.Voluntary:
                  bool isPreDeadline = project.ApplicationReceivedDate.HasValue && DateTime.Compare(project.ApplicationReceivedDate.Value, new DateTime(2024, 12, 20, 23, 59, 59, DateTimeKind.Utc)) <= 0;
                  bool isVoluntaryConverionPreDeadline = isPreDeadline && project.AcademyTypeAndRoute == "Converter";

                  if (isVoluntaryConverionPreDeadline)
                  {
                     table.Cell().Element(TableLabelCellStyle).Text("Grant funding amount");
                     table.Cell().Element(TableDataCellStyle).Text(project.ConversionSupportGrantAmount?.ToMoneyString(true));

                     table.Cell().Element(TableLabelCellStyle).Text("Grant funding reason");
                     table.Cell().Element(TableDataCellStyle).Text(project.ConversionSupportGrantChangeReason);
                  }

                  table.Cell().Element(TableLabelCellStyle).Text("Recommendation");
                  table.Cell().Element(TableDataCellStyle).Text(project.RecommendationForProject);

                  if (!string.IsNullOrWhiteSpace(project.RecommendationNotesForProject))
                  {
                     table.Cell().Element(TableLabelCellStyle).Text("Recommendation notes");
                     table.Cell().Element(TableDataCellStyle).Text(project.RecommendationNotesForProject);
                  }

                  break;
               case AcademyTypeAndRoutes.Sponsored:
                  table.Cell().Element(TableLabelCellStyle).Text("Grant funding type");
                  table.Cell().Element(TableDataCellStyle).Text(project.ConversionSupportGrantType);

                  table.Cell().Element(TableLabelCellStyle).Text("Grant funding amount");
                  table.Cell().Element(TableDataCellStyle).Text(project.ConversionSupportGrantAmount?.ToMoneyString(true));

                  table.Cell().Element(TableLabelCellStyle).Text("Grant funding reason");
                  table.Cell().Element(TableDataCellStyle).Text(project.ConversionSupportGrantChangeReason);

                  table.Cell().Element(TableLabelCellStyle).Text("Is the school applying for an EIG (Environmental Improvement Grant)?");
                  table.Cell().Element(TableDataCellStyle).Text(project.ConversionSupportGrantEnvironmentalImprovementGrant);

                  table.Cell().Element(TableLabelCellStyle).Text("Has the Schools Notification Mailbox (SNM) received a Form 7?");
                  table.Cell().Element(TableDataCellStyle).Text(project.Form7Received);

                  table.Cell().Element(TableLabelCellStyle).Text("Date SNM received Form 7");
                  table.Cell().Element(TableDataCellStyle).Text(project.Form7ReceivedDate?.ToDateString());

                  table.Cell().Element(TableLabelCellStyle).Text("Date directive academy order (DAO) pack sent");
                  table.Cell().Element(TableDataCellStyle).Text(project.DaoPackSentDate?.ToDateString());

                  break;
            }

            if (project.SchoolType.Contains("pupil referral unit", StringComparison.CurrentCultureIgnoreCase))
            {
               table.Cell().Element(TableLabelCellStyle).Text("Number of sites");
               table.Cell().Element(TableDataCellStyle).Text(project.ConversionSupportGrantNumberOfSites?.ToString());
            }

            // Advisory board
            table.Cell().Element(TableLabelCellStyle).Text("Date of advisory board");
            table.Cell().Element(TableDataCellStyle).Text(project.HeadTeacherBoardDate?.ToDateString());

            table.Cell().Element(TableLabelCellStyle).Text("Proposed academy opening date");
            table.Cell().Element(TableDataCellStyle).Text(project.ProposedConversionDate.ToDateString());

            table.Cell().Element(TableLabelCellStyle).Text("Previous advisory board");
            table.Cell().Element(TableDataCellStyle).Text(project.PreviousHeadTeacherBoardDate.ToDateString());

            // Local Authority
            table.Cell().Element(TableLabelCellStyle).Text("Local authority");
            table.Cell().Element(TableDataCellStyle).Text(project.LocalAuthority);

            table.Cell().Element(TableLabelCellStyle).Text("Sponsor name");
            table.Cell().Element(TableDataCellStyle).Text(project.SponsorName);

            table.Cell().Element(TableLabelCellStyle).Text("Sponsor reference number");
            table.Cell().Element(TableDataCellStyle).Text(project.SponsorReferenceNumber);
         });
      }

      public static void GenerateLegalRequirementsContent(this IContainer container, AcademyConversionProject project)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Management committee resolution");
            table.Cell().Element(TableDataCellStyle).Text(project.GoverningBodyResolution.SplitPascalCase() ?? "N/A");

            table.Cell().Element(TableLabelCellStyle).Text("Consultation");
            table.Cell().Element(TableDataCellStyle).Text(project.Consultation.SplitPascalCase() ?? "N/A");

            table.Cell().Element(TableLabelCellStyle).Text("Diocesan consent");
            table.Cell().Element(TableDataCellStyle).Text(project.DiocesanConsent.SplitPascalCase() ?? "N/A");

            table.Cell().Element(TableLabelCellStyle).Text("Foundation consent");
            table.Cell().Element(TableDataCellStyle).Text(project.FoundationConsent.SplitPascalCase() ?? "N/A");
         });
      }

      public static void GeneratePublicSectorEqualityDutyContent(this IContainer container, AcademyConversionProject project)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableLabelCellStyle).Text("Public Sector Equality Duty consideration");
            table.Cell().Element(TableDataCellStyle).Text(PreviewPublicSectorEqualityDutyModel.GenerateReduceImpactReasonLabel(project.PublicEqualityDutyImpact));

            if (!string.IsNullOrWhiteSpace(project.PublicEqualityDutyReduceImpactReason))
            {
               table.Cell().Element(TableLabelCellStyle).Text("What will be done to reduce this impact?");
               table.Cell().Element(TableDataCellStyle).Text(project.PublicEqualityDutyReduceImpactReason);
            }
         });
      }

      public static void GenerateRationaleForProjectContent(this IContainer container, AcademyConversionProject project)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableDataCellStyle).HTML(handler =>
            {
               handler.SetHtml(project.RationaleForProject ?? "N/A");
            });
         });
      }

      public static void GenerateRationaleForTrustOrSponsorContent(this IContainer container, AcademyConversionProject project)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableDataCellStyle).HTML(handler =>
            {
               handler.SetHtml(project.RationaleForTrust ?? "N/A");
            });
         });
      }

      public static void GenerateRiskAndIssuesContent(this IContainer container, AcademyConversionProject project)
      {
         container.Table(table =>
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
            });

            table.Cell().Element(TableDataCellStyle).Text(project.RisksAndIssues ?? "N/A");
         });
      }

      public static void GenerateOverallAbsenceContent(this IContainer container, AcademyConversionProject project, IEnumerable<EducationalAttendanceViewModel> educationalAttendance)
      {
         var educationalAttendanceViewModels = educationalAttendance.OrderByDescending(x => x.Year).ToList();

         container.Table(table => 
         {
            table.ColumnsDefinition(columns =>
            {
               for (int i = 0; i <= educationalAttendanceViewModels.Count; i++)
               {
                  columns.RelativeColumn(1);
               }
            });

            table.Cell().Element(TableLabelCellStyle).Text("");

            for (int i = 0; i < educationalAttendanceViewModels.Count; i++)
            {
               var vm = educationalAttendanceViewModels[i];

               table.Cell().Element(TableLabelCellStyle).Text(vm.Year);
            }

            table.Cell().Element(TableLabelCellStyle).Text(project.SchoolName);

            for (int i = 0; i < educationalAttendanceViewModels.Count; i++)
            {
               var vm = educationalAttendanceViewModels[i];

               table.Cell().Element(TableLabelCellStyle).Text(vm.OverallAbsence);
            }
         });
      }

      public static void GeneratePersistentAbsenceContent(this IContainer container, AcademyConversionProject project, IEnumerable<EducationalAttendanceViewModel> educationalAttendance)
      {
         var educationalAttendanceViewModels = educationalAttendance.OrderByDescending(x => x.Year).ToList();

         container.Table(table => 
         {
            table.ColumnsDefinition(columns =>
            {
               for (int i = 0; i <= educationalAttendanceViewModels.Count; i++)
               {
                  columns.RelativeColumn(1);
               }
            });

            table.Cell().Element(TableLabelCellStyle).Text("");

            for (int i = 0; i < educationalAttendanceViewModels.Count; i++)
            {
               var vm = educationalAttendanceViewModels[i];

               table.Cell().Element(TableLabelCellStyle).Text(vm.Year);
            }

            table.Cell().Element(TableLabelCellStyle).Text(project.SchoolName);

            for (int i = 0; i < educationalAttendanceViewModels.Count; i++)
            {
               var vm = educationalAttendanceViewModels[i];

               table.Cell().Element(TableLabelCellStyle).Text(vm.PersistentAbsence);
            }
         });
      }
   }
}
