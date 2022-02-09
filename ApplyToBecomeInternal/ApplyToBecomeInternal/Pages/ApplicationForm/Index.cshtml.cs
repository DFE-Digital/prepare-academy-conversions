using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.Application;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.ApplicationForm
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		protected readonly ApplicationRepository _applicationRepository;
		public IEnumerable<BaseFormSection> Sections { get; set; }

		public IndexModel(IAcademyConversionProjectRepository repository, ApplicationRepository applicationRepository) : base(repository)
		{
			_applicationRepository = applicationRepository;
		}
		
		public override async Task<IActionResult> OnGetAsync(int id)
        {
			var result = await base.OnGetAsync(id);
			var applicationReference = base.Project.ApplicationReferenceNumber;

			//var applicationResponse = await _applicationRepository.GetApplicationByReference(applicationReference);
			//if (!applicationResponse.Success)
			//{
			//	// CML handling needed for different errors
			//	// 404 logic
			//	return NotFound();
			//}
			//var application = applicationResponse.Body;
			var application = DummyApplication;
			Sections = new BaseFormSection[]
			{
				new ApplicationFormSection(application),
				new AboutConversionSection(application.SchoolApplication),
				new FurtherInformationSection(application.SchoolApplication),
				new FinanceSection(application.SchoolApplication),
				new FuturePupilNumberSection(application.SchoolApplication),
				new LandAndBuildingsSection(application.SchoolApplication),
				new PreOpeningSupportGrantSection(application.SchoolApplication),
				new ConsultationSection(application.SchoolApplication),
				new DeclarationSection(application.SchoolApplication)
			};

			return result;
		}

		private Application DummyApplication => new Application
		{
			SchoolApplication = new ApplyingSchool 
			{
				SchoolName = Project.SchoolName,
				SchoolConversionContactHeadName = "Garth Brown",
				SchoolConversionContactHeadEmail = "garth.brown@stwilfridsprimary.edu.uk",
				SchoolConversionContactHeadTel = "09876 64547563",
				SchoolConversionContactChairName = "Arna Siggurdottier",
				SchoolConversionContactChairEmail = "arna.siggurdottier@dynamicstrust.co.uk",
				SchoolConversionContactChairTel = "0972 345 119",
				SchoolConversionApproverContactName = "Garth Brown",
				SchoolConversionApproverContactEmail = "garth.brown@stwilfridsprimary.edu.uk",
				SchoolConversionTargetDate = new DateTime(2021, 04, 20),
				SchoolConversionTargetDateSpecified = true,
				SchoolConversionReasonsForJoining = "This is a rationale",
				SchoolAdSchoolContributionToTrust = "the school will bring these things to the trust",
				SchoolAdInspectedButReportNotPublished = false,
				SchoolOngoingSafeguardingInvestigations = false,
				SchoolPartOfLaReorganizationPlan = false,
				SchoolPartOfLaClosurePlan = false,
				SchoolFaithSchoolDioceseName = "Diocese of Warrington",
				SchoolIsPartOfFederation = false,
				SchoolIsSupportedByFoundation = false,
				SchoolHasSACREException = false,
				SchoolAdFeederSchools = "n/a as we are a primary school",
				GoverningBodyConsentEvidenceDocument = new Link("consent.docx", "#"),
				SchoolAdEqualitiesImpactAssessmentCompleted = false,
				PreviousFinancialYear = new FinancialYear
				{
					FYEndDate = new DateTime(2020, 03, 31),
					RevenueCarryForward = 16909,
					RevenueStatus = "Surplus" // enum FinancialYearState? CML
				},
				CurrentFinancialYear = new FinancialYear
				{
					FYEndDate = new DateTime(2021, 03, 31),
					RevenueCarryForward = 14393,
					RevenueStatus = "Surplus" // enum FinancialYearState? CML
				},
				NextFinancialYear = new FinancialYear
				{
					FYEndDate = new DateTime(2022, 03, 31),
					RevenueCarryForward = 1690,
					RevenueStatus = "Deficit" // enum FinancialYearState? CML
				},
				ExistingLoans = new List<Loan>
				{
					new Loan()
					{
						SchoolLoanAmount = 34000,
						SchoolLoanInterestRate = 3.0M,
						SchoolLoanProvider = "Loans R us",
						SchoolLoanPurpose = "repairs",
						SchoolLoanSchedule = "is this really a free text field?"
					}
				},
				ExistingLeases = new List<Lease>()
				{
					new Lease()
					{
						SchoolLeaseInterestRate = 3.0M,
						SchoolLeasePaymentToDate = 2000,
						SchoolLeasePurpose = "specialist equipment",
						SchoolLeaseRepaymentValue = 4000,
						SchoolLeaseResponsibilityForAssets = "school is responsible",
						SchoolLeaseTerms = "something about the terms of the agreement",
						SchoolLeaseValueOfAssets = 4000
					}
				},
				FinanceOngoingInvestigations = false,
				// pupil numbers
				SchoolCapacityYear1 = 189,
				SchoolCapacityYear2 = 189,
				SchoolCapacityYear3 = 189,
				SchoolCapacityAssumptions = "spreadsheets",
				SchoolCapacityPublishedAdmissionsNumber = 210,
				// land and buildings
				SchoolBuildLandOwnerExplained = "The Diocese of Warrington owns the building and the land. The LA owns the playing fields.",
				SchoolBuildLandWorksPlanned = false,
				SchoolBuildLandSharedFacilities = false,
				//	HasSchoolGrants = false,
				SchoolBuildLandPFIScheme = false,
				SchoolBuildLandPriorityBuildingProgramme = false,
				SchoolBuildLandFutureProgramme = false,
				SchoolSupportGrantFundsPaidTo = "to the school",
				SchoolHasConsultedStakeholders = true,
				SchoolDeclarationSignedByName = "Garth Brown"
				
			},			
			TrustName = Project.NameOfTrust,
			ApplicationLeadAuthorName = "Garth Brown",			
			TrustConsentEvidenceDocument = new Link("consent_dynamics.docx", "#"),
			ChangesToTrust = false,
			ChangesToLaGovernance = false,
		};
	}
}
