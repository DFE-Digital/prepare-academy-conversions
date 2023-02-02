using System;

namespace ApplyToBecome.Data.Models.AcademisationApplication;

public class LandAndBuildings
{
   public string OwnerExplained { get; set; }
   public bool WorksPlanned { get; set; }
   public DateTime WorksPlannedDate { get; set; }
   public string WorksPlannedExplained { get; set; }
   public bool FacilitiesShared { get; set; }
   public string FacilitiesSharedExplained { get; set; }
   public bool Grants { get; set; }
   public string GrantsAwardingBodies { get; set; }
   public bool PartOfPfiScheme { get; set; }
   public string PartOfPfiSchemeType { get; init; }
   public bool PartOfPrioritySchoolsBuildingProgramme { get; set; }
   public bool PartOfBuildingSchoolsForFutureProgramme { get; set; }
}