using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public class GeneralInformationGenerator
   {
      public static void AddGeneralInformation(IDocumentBuilder builder, HtbTemplate document)
      {
         builder.ReplacePlaceholderWithContent("GeneralInformation", build =>
         {
            build.AddHeading("General Information", HeadingLevel.One);
            build.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement { Value = "School type", Bold = true }, new TextElement { Value = document.SchoolType } },
            new[] { new TextElement { Value = "School phase", Bold = true }, new TextElement { Value = document.SchoolPhase } },
            new[] { new TextElement { Value = "Age range", Bold = true }, new TextElement { Value = document.AgeRange } },
            new[] { new TextElement { Value = "Capacity", Bold = true }, new TextElement { Value = document.SchoolCapacity } },
            new[] { new TextElement { Value = "Published admission number (PAN)", Bold = true }, new TextElement { Value = document.PublishedAdmissionNumber } },
            new[] { new TextElement { Value = "Number on roll (NOR)", Bold = true }, new TextElement { Value = document.NumberOnRoll } },
            new[] { new TextElement { Value = "Percentage of the school is full", Bold = true }, new TextElement { Value = document.PercentageSchoolFull } },
            new[]
            {
               new TextElement { Value = "Percentage of free school meals at the school (%FSM)", Bold = true }, new TextElement { Value = document.PercentageFreeSchoolMeals }
            },
            new[] { new TextElement { Value = "Viability issues", Bold = true }, new TextElement { Value = document.ViabilityIssues } },
            new[] { new TextElement { Value = "Financial deficit", Bold = true }, new TextElement { Value = document.FinancialDeficit } },
            new[] { new TextElement { Value = "Private finance initiative (PFI) scheme", Bold = true }, new TextElement { Value = document.PartOfPfiScheme } },
            new[] { new TextElement { Value = "Is the school linked to a diocese?", Bold = true }, new TextElement { Value = document.IsSchoolLinkedToADiocese } },
            new[]
            {
               new TextElement { Value = "Distance from the converting school to the trust or other schools in the trust", Bold = true },
               new TextElement { Value = $"{document.DistanceFromSchoolToTrustHeadquarters} {document.DistanceFromSchoolToTrustHeadquartersAdditionalInformation}" }
            },
            new[] { new TextElement { Value = "Parliamentary constituency", Bold = true }, new TextElement { Value = document.ParliamentaryConstituency } },
            new[] { new TextElement { Value = "MP name and political party", Bold = true }, new TextElement { Value = document.MPNameAndParty } }
         });
         });
      }
   }
}
