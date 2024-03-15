using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class SchoolOverviewGenerator
   {

      public static void AddSchoolOverview(IDocumentBuilder builder, HtbTemplate document)
      {
         builder.ReplacePlaceholderWithContent("SchoolOverview", build =>
         {
            build.AddHeading("School overview", HeadingLevel.One);
            var schoolOverviewTable = new List<TextElement[]>
            {
               DocumentGeneratorStringSanitiser.CreateTextElements("School type", document.SchoolType),
               DocumentGeneratorStringSanitiser.CreateTextElements("School phase", document.SchoolPhase),
               DocumentGeneratorStringSanitiser.CreateTextElements("Age range", document.AgeRange),
               DocumentGeneratorStringSanitiser.CreateTextElements("Capacity", document.SchoolCapacity),
               DocumentGeneratorStringSanitiser.CreateTextElements("Published admission number (PAN)", document.PublishedAdmissionNumber),
               DocumentGeneratorStringSanitiser.CreateTextElements("Number on roll (NOR)", document.NumberOnRoll),
               DocumentGeneratorStringSanitiser.CreateTextElements("Percentage of the school is full", document.PercentageSchoolFull),
               DocumentGeneratorStringSanitiser.CreateTextElements("Percentage of free school meals at the school (%FSM)", document.PercentageFreeSchoolMeals),
               DocumentGeneratorStringSanitiser.CreateTextElements("Viability issues", document.ViabilityIssues)
            };

            if (document.SchoolType.ToLower().Contains("pupil referral unit"))
            {
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Which groups of pupils attend the school", document.PupilsAttendingGroup));
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Number of places funded for", document.NumberOfPlacesFundedFor));
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Alternative provision", document.NumberOfAlternativeProvisionPlaces));
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("SEN Unit", document.NumberOfSENUnitPlaces));
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Medical", document.NumberOfMedicalPlaces));
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Post-16", document.NumberOfPost16Places));
            }

            if (document.SchoolType.ToLower().Contains("special"))
            {
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Number of places funded for", document.NumberOfPlacesFundedFor));
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Number of residential places", document.NumberOfResidentialPlaces));
               schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Number of funded residential places", document.NumberOfFundedResidentialPlaces));
            }

            schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Financial deficit", document.FinancialDeficit));
            schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Private finance initiative (PFI) scheme", document.PartOfPfiScheme));
            schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Is the school linked to a diocese?", document.IsSchoolLinkedToADiocese));
            schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Distance from the converting school to the trust or other schools in the trust", $"{document.DistanceFromSchoolToTrustHeadquarters} {document.DistanceFromSchoolToTrustHeadquartersAdditionalInformation}"));
            schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Parliamentary constituency", document.ParliamentaryConstituency));
            schoolOverviewTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("MP name and political party", document.MemberOfParliamentNameAndParty));

            build.AddTable(schoolOverviewTable);
         });
      }
   }
}
