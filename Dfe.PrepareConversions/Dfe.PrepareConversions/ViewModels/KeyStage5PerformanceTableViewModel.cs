using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Extensions;

namespace Dfe.PrepareConversions.ViewModels;

public class KeyStage5PerformanceTableViewModel
{
   public string Year { get; set; }
   public string AcademicProgress { get; set; }
   public string AcademicAverage { get; set; }
   public string AppliedGeneralProgress { get; set; }
   public string AppliedGeneralAverage { get; set; }
   public string NationalAverageAcademicProgress { get; set; }
   public string NationalAverageAcademicAverage { get; set; }
   public string NationalAverageAppliedGeneralProgress { get; set; }
   public string NationalAverageAppliedGeneralAverage { get; set; }

   public static KeyStage5PerformanceTableViewModel Build(KeyStage5PerformanceResponse keyStage5Performance)
   {
      return new KeyStage5PerformanceTableViewModel
      {
         Year = keyStage5Performance.Year.FormatKeyStageYear(),
         AcademicProgress = keyStage5Performance.AcademicProgress.NotDisadvantaged.FormatValue(),
         AcademicAverage = keyStage5Performance.AcademicQualificationAverage.FormatValue(),
         AppliedGeneralProgress = keyStage5Performance.AppliedGeneralProgress.NotDisadvantaged.FormatValue(),
         AppliedGeneralAverage = keyStage5Performance.AppliedGeneralQualificationAverage.FormatValue(),
         NationalAverageAcademicProgress = ((decimal?)null).FormatValue(),
         NationalAverageAcademicAverage = keyStage5Performance.NationalAcademicQualificationAverage.FormatValue(),
         NationalAverageAppliedGeneralProgress = ((decimal?)null).FormatValue(),
         NationalAverageAppliedGeneralAverage = keyStage5Performance.NationalAppliedGeneralQualificationAverage.FormatValue()
      };
   }
}
