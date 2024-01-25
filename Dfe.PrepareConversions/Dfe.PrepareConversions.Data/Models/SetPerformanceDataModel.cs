using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Models
{
   public class SetPerformanceDataModel
   {
      public SetPerformanceDataModel(int id,
         string? keyStage2PerformanceAdditionalInformation,
         string? keyStage4PerformanceAdditionalInformation,
         string? keyStage5PerformanceAdditionalInformation,
         string? educationalAttendanceAdditionalInformation)
      {
         Id = id;
         KeyStage2PerformanceAdditionalInformation = keyStage2PerformanceAdditionalInformation;
         KeyStage4PerformanceAdditionalInformation = keyStage4PerformanceAdditionalInformation;
         KeyStage5PerformanceAdditionalInformation = keyStage5PerformanceAdditionalInformation;
         EducationalAttendanceAdditionalInformation = educationalAttendanceAdditionalInformation;
      }

      public int Id { get; set; }
      public string? KeyStage2PerformanceAdditionalInformation { get; set; }
      public string? KeyStage4PerformanceAdditionalInformation { get; set; }
      public string? KeyStage5PerformanceAdditionalInformation { get; set; }
      public string? EducationalAttendanceAdditionalInformation { get; set; }
   }
}
