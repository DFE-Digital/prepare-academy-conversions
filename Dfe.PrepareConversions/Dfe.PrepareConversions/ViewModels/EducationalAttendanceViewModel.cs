using Dfe.Academies.Contracts.V1.EducationalPerformance;


namespace Dfe.PrepareConversions.ViewModels;

public class EducationalAttendanceViewModel
{
   public string Year { get; set; }
   public string OverallAbsence { get; set; }
   public string PersistentAbsence { get; set; }

   public static EducationalAttendanceViewModel Build(SchoolAbsenceDataDto dto)
   {
      return new EducationalAttendanceViewModel
      {
         Year = dto.Year,
         OverallAbsence = dto.OverallAbsence,
         PersistentAbsence = dto.PersistentAbsence,
      };
   }
}
