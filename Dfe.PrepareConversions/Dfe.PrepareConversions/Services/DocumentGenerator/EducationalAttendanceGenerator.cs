using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using System.Collections.Generic;
using static Dfe.PrepareConversions.Utils.KeyStageDataStatusHelper;
using static Dfe.PrepareConversions.Services.DocumentGenerator.DocumentGenerator;
using DocumentFormat.OpenXml.EMMA;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Dfe.PrepareConversions.Services.DocumentGenerator;

public static class EducationalAttendanceGenerator
{
   public static void AddEducationalAttendanceInformation(IDocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
   {
      if (document.EducationalAttendance == null || !project.SchoolType.ToLower().Contains("special"))
      {
         documentBuilder.ReplacePlaceholderWithContent("EducationalAttendanceData", builder => builder.AddParagraph(""));
         return;
      }

      documentBuilder.ReplacePlaceholderWithContent("EducationalAttendanceData", builder =>
      {
         builder.AddHeading("Educational Attendance", HeadingLevel.One);
         builder.AddHeading($"Overall absence", HeadingLevel.Two);

         var educationalAttendanceViewModels = document.EducationalAttendance.OrderByDescending(x => x.Year).ToList();
         var table = new List<TextElement[]>();

         var textElements = new List<TextElement>() { new TextElement("") { Bold = true } };

         for (int i = 0; i < 3; i++)
         {
            var vm = educationalAttendanceViewModels[i];

            textElements.Add(new TextElement(vm.Year) { Bold = true });
         }

         table.Add(textElements.ToArray());
         textElements = new List<TextElement>() { new TextElement($"{project.SchoolName}") { Bold = true }, };

         for (int i = 0; i < 3; i++)
         {
            var vm = educationalAttendanceViewModels[i];

            textElements.Add(new TextElement(vm.OverallAbsence) { Bold = true });
         }
         table.Add(textElements.ToArray());
         builder.AddTable(table);

         ///******************///
         table = new List<TextElement[]>();
         textElements = new List<TextElement>() { new TextElement("") { Bold = true }, };
         builder.AddHeading($"Persistent absence of 10% or more", HeadingLevel.Two);

         for (int i = 0; i < 3; i++)
         {
            var vm = educationalAttendanceViewModels[i];

            textElements.Add(new TextElement(vm.Year) { Bold = true });
         }

         table.Add(textElements.ToArray());     
         textElements = new List<TextElement>() { new TextElement($"{project.SchoolName}") { Bold = true }, };

         for (int i = 0; i < 3; i++)
         {
            var vm = educationalAttendanceViewModels[i];

            textElements.Add(new TextElement(vm.PersistentAbsence) { Bold = true });
         }
         table.Add(textElements.ToArray());
         builder.AddTable(table);

         builder.AddParagraph("");
         builder.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.EducationalAttendanceAdditionalInformation) }
         });
      });
   }
}