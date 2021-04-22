namespace ApplyToBecome.Data.Models.Application
{
	public class LandAndBuildings
	{
		public string BuildingAndLandOwner { get; set; }
		public bool HasCurrentPlannedBuildingWorks { get; set; }
		public bool HasSharedFacilitiesOnSite { get; set; }
		public bool HasSchoolGrants { get; set; }
		public bool HasPrivateFinanceInitiativeScheme { get; set; }
		public bool IsInPrioritySchoolBuildingProgramme { get; set; }
		public bool IsInBuildingSchoolsForFutureProgramme { get; set; }
	}
}