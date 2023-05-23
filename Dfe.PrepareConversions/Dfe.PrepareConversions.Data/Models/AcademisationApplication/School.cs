using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.AcademisationApplication;

public class School
{
   public int Id { get; set; }
   public int Urn { get; set; }
   public string SchoolName { get; set; }
   public LandAndBuildings LandAndBuildings { get; set; }
   public string TrustBenefitDetails { get; set; }
   public string OfstedInspectionDetails { get; set; }
   public bool? Safeguarding { get; set; }
   public string SafeguardingDetails { get; set; }
   public string LocalAuthorityReorganisationDetails { get; set; }
   public string LocalAuthorityClosurePlanDetails { get; set; }
   public string DioceseName { get; set; }
   public string DioceseFolderIdentifier { get; set; }
   public bool PartOfFederation { get; set; }
   public string FoundationTrustOrBodyName { get; set; }
   public string FoundationConsentFolderIdentifier { get; set; }
   public DateTimeOffset? ExemptionEndDate { get; set; }
   public string MainFeederSchools { get; set; }
   public string ResolutionConsentFolderIdentifier { get; set; }
   public string FurtherInformation { get; set; }
   public FinancialYear PreviousFinancialYear { get; set; }
   public FinancialYear CurrentFinancialYear { get; set; }
   public FinancialYear NextFinancialYear { get; set; }
   public List<Loan> Loans { get; set; }
   public List<Lease> Leases { get; set; }
   public string SchoolConversionContactHeadName { get; set; }
   public string SchoolConversionContactHeadEmail { get; set; }
   public string SchoolConversionContactChairName { get; set; }
   public string SchoolConversionContactChairEmail { get; set; }
   public string SchoolConversionContactRole { get; set; }
   public string SchoolConversionMainContactOtherName { get; set; }
   public string SchoolConversionMainContactOtherEmail { get; set; }
   public string SchoolConversionMainContactOtherRole { get; set; }
   public string SchoolConversionApproverContactName { get; set; }
   public string SchoolConversionApproverContactEmail { get; set; }
   public bool SchoolConversionTargetDateSpecified { get; set; }
   public DateTime SchoolConversionTargetDate { get; set; }
   public string SchoolConversionTargetDateExplained { get; set; }
   public bool ConversionChangeNamePlanned { get; set; }
   public string ProposedNewSchoolName { get; set; }
   public string ApplicationJoinTrustReason { get; set; }
   public int? ProjectedPupilNumbersYear1 { get; set; }
   public int? ProjectedPupilNumbersYear2 { get; set; }
   public int? ProjectedPupilNumbersYear3 { get; set; }
   public string SchoolCapacityAssumptions { get; set; }
   public int SchoolCapacityPublishedAdmissionsNumber { get; set; }
   public string SchoolSupportGrantFundsPaidTo { get; set; }
   public bool ConfirmPaySupportGrantToSchool { get; set; }
   public bool SchoolHasConsultedStakeholders { get; set; }
   public string SchoolPlanToConsultStakeholders { get; set; }
   public bool FinanceOngoingInvestigations { get; set; }
   public string FinancialInvestigationsExplain { get; set; }
   public bool FinancialInvestigationsTrustAware { get; set; }
   public bool DeclarationBodyAgree { get; set; }
   public bool DeclarationIAmTheChairOrHeadteacher { get; set; }
   public string DeclarationSignedByName { get; set; }
   public string ProtectedCharacteristics { get; set; }
}
