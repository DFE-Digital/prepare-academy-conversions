using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Services.DocumentGenerator;

public static class EducationalAttendanceGenerator
{
   public static void AddEducationalAttendanceInformation(IDocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
   {
      if (document.EducationalAttendance == null || !project.SchoolType.Contains("special", System.StringComparison.CurrentCultureIgnoreCase))
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

         var textElements = new List<TextElement>() { new("") { Bold = true } };

         for (int i = 0; i < educationalAttendanceViewModels.Count; i++)
         {
            var vm = educationalAttendanceViewModels[i];

            textElements.Add(new TextElement(vm.Year) { Bold = true });
         }

         table.Add([.. textElements]);
         textElements = [new TextElement($"{project.SchoolName}") { Bold = true },];

         for (int i = 0; i < educationalAttendanceViewModels.Count; i++)
         {
            var vm = educationalAttendanceViewModels[i];

            textElements.Add(new TextElement(vm.OverallAbsence) { Bold = true });
         }
         table.Add([.. textElements]);
         builder.AddTable(table);

         ///******************///
         table = [];
         textElements = [new TextElement("") { Bold = true },];
         builder.AddHeading($"Persistent absence of 10% or more", HeadingLevel.Two);

         for (int i = 0; i < educationalAttendanceViewModels.Count; i++)
         {
            var vm = educationalAttendanceViewModels[i];

            textElements.Add(new TextElement(vm.Year) { Bold = true });
         }

         table.Add([.. textElements]);     
         textElements = [new TextElement($"{project.SchoolName}") { Bold = true },];

         for (int i = 0; i < educationalAttendanceViewModels.Count; i++)
         {
            var vm = educationalAttendanceViewModels[i];

            textElements.Add(new TextElement(vm.PersistentAbsence) { Bold = true });
         }
         table.Add([.. textElements]);
         builder.AddTable(table);

         builder.AddParagraph("");
         builder.AddTable([]);
      });
   }
}