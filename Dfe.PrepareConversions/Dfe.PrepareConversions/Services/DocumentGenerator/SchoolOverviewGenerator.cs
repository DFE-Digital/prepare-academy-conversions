using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Utils;
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
            var schoolOverviewTable = new List<TextElement[]>();
            schoolOverviewTable.AddRange(new List<TextElement[]> {
               new[] { new TextElement { Value = "School type", Bold = true }, new TextElement { Value = document.SchoolType } },
               new[] { new TextElement { Value = "School phase", Bold = true }, new TextElement { Value = document.SchoolPhase } },
               new[] { new TextElement { Value = "Age range", Bold = true }, new TextElement { Value = document.AgeRange } },
               new[] { new TextElement { Value = "Capacity", Bold = true }, new TextElement { Value = document.SchoolCapacity } },
               new[] { new TextElement { Value = "Published admission number (PAN)", Bold = true }, new TextElement { Value = document.PublishedAdmissionNumber } },
               new[] { new TextElement { Value = "Number on roll (NOR)", Bold = true }, new TextElement { Value = document.NumberOnRoll } },
               new[] { new TextElement { Value = "Percentage of the school is full", Bold = true }, new TextElement { Value = document.PercentageSchoolFull } },
               new[]
               {
                  new TextElement { Value = "Percentage of free school meals at the school (%FSM)", Bold = true },
                  new TextElement { Value = document.PercentageFreeSchoolMeals }
               },
               new[] { new TextElement { Value = "Viability issues", Bold = true }, new TextElement { Value = document.ViabilityIssues } }});

            if (document.SchoolType.ToLower().Contains("pupil referral unit"))
            {
               schoolOverviewTable.AddRange(new List<TextElement[]> {
                  new[] { new TextElement { Value = "Which groups of pupils attend the school", Bold = true }, new TextElement { Value = document.PupilsAttendingGroup } },
                  new[] { new TextElement { Value = "Number of places funded for", Bold = true }, new TextElement { Value = document.NumberOfPlacesFundedFor } },
                  new[] { new TextElement { Value = "Alternative provision", Bold = true }, new TextElement { Value = document.NumberOfAlternativeProvisionPlaces } },
                  new[] { new TextElement { Value = "SEN Unit", Bold = true }, new TextElement { Value = document.NumberOfSENUnitPlaces } },
                  new[] { new TextElement { Value = "Medical", Bold = true }, new TextElement { Value = document.NumberOfMedicalPlaces } },
                  new[] { new TextElement { Value = "Post-16", Bold = true }, new TextElement { Value = document.NumberOfPost16Places } },
               });
            }

            // Specific SEN fields
            if (document.SchoolType.ToLower().Contains("special"))
            {
               schoolOverviewTable.AddRange(new List<TextElement[]> {
               new[] { new TextElement { Value = "Number of places funded for", Bold = true }, new TextElement { Value = document.NumberOfPlacesFundedFor } },
               new[] { new TextElement { Value = "Number of residential places", Bold = true }, new TextElement { Value = document.NumberOfResidentialPlaces } },
               new[] { new TextElement { Value = "Number of funded residential places", Bold = true }, new TextElement { Value = document.NumberOfFundedResidentialPlaces } },
            });
            }

            schoolOverviewTable.AddRange(new List<TextElement[]> {
              new[] { new TextElement { Value = "Financial deficit", Bold = true }, new TextElement { Value = document.FinancialDeficit } },
               new[] { new TextElement { Value = "Private finance initiative (PFI) scheme", Bold = true }, new TextElement { Value = document.PartOfPfiScheme } },
               new[] { new TextElement { Value = "Is the school linked to a diocese?", Bold = true }, new TextElement { Value = document.IsSchoolLinkedToADiocese } },
               new[]
               {
                  new TextElement { Value = "Distance from the converting school to the trust or other schools in the trust", Bold = true },
                  new TextElement { Value = $"{document.DistanceFromSchoolToTrustHeadquarters} {document.DistanceFromSchoolToTrustHeadquartersAdditionalInformation}" }
               },
               new[] { new TextElement { Value = "Parliamentary constituency", Bold = true }, new TextElement { Value = document.ParliamentaryConstituency } },
               new[] { new TextElement { Value = "MP name and political party", Bold = true }, new TextElement { Value = document.MemberOfParliamentNameAndParty } }
            });

            build.AddTable(schoolOverviewTable);
         });
      }
   }
}
