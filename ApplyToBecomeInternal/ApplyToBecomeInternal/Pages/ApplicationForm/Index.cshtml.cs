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

			var applicationResponse = await _applicationRepository.GetApplicationByReference(applicationReference);
			if (!applicationResponse.Success)
			{
				// CML handling needed for different errors
				// 404 logic
				return NotFound();
			}
			var application = applicationResponse.Body;
			//var application = DummyApplication;
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
			Name = Project.SchoolName,
			FormTrustProposedNameOfTrust = Project.NameOfTrust,
			ApplicationLeadAuthorName = "Garth Brown",
//			EvidenceDocument = new Link("consent_dynamics.docx", "#"),
			ChangesToTrust = 0,
			ChangesToLaGovernance = 1,
			SchoolConversionContactHeadName = "Garth Brown",
			SchoolConversionContactHeadEmail = "garth.brown@stwilfridsprimary.edu.uk",
			SchoolConversionContactHeadTel = "09876 64547563",
			SchoolConversionContactChairName = "Arna Siggurdottier",
			SchoolConversionContactChairEmail = "arna.siggurdottier@dynamicstrust.co.uk",
			SchoolConversionContactChairTel = "0972 345 119",
			SchoolConversionApproverContactName = "Garth Brown",
			SchoolConversionApproverContactEmail = "garth.brown@stwilfridsprimary.edu.uk",
			SchoolConversionTargetDateDate = new DateTime(2021, 04, 20),
			SchoolConversionTargetDateDifferent = 1,
			SchoolConversionReasonsForJoining = "This is a rationale",
			SchoolConversionChangeName = 1,
			SchoolAdSchoolContributionToTrust = "the school will bring these things to the trust",
			SchoolAdInspectedButReportNotPublished = 0,
			SchoolAdSafeguarding = 0,
			SchoolLaReorganization = 0,
			SchoolLaClosurePlans = 0,
//				IsLinkedToDiocese = true,
			SchoolFaithSchoolDioceseName = "Diocese of Warrington",
//				DioceseLetterOfConsent = new Link("consent-from-diocese.docx", "#"),
			SchoolPartOfFederation = 0,
			SchoolSupportedFoundation = 1,
			SchoolSACREExemption = 0,
			SchoolAdFeederSchools = "n/a as we are a primary school",
			//				SchoolConsent = new Link("consent.docx", "#"),
			SchoolAdEqualitiesImpactAssessment = 0,
			SchoolAddFurtherInformation = null,
			SchoolPFYEndDate = new DateTime(2020, 03, 31),
			SchoolPFYRevenue = 169093,
			SchoolPFYRevenueStatus = 0,
			SchoolCFYEndDate = new DateTime(2021, 03, 31),
			SchoolCFYRevenue = 143931,
			SchoolCFYRevenueStatus = 1,
			SchoolNFYEndDate = new DateTime(2022, 03, 31),
			SchoolNFYRevenue = 169093,
			SchoolNFYRevenueStatus = 1, // enum FinancialYearState? CML
			SchoolLoanExists = new A2BSelectOption { Id = 0, Name = "Loan" },
			SchoolLeaseExists = new A2BSelectOption { Id = 0, Name = "Lease" },
			SchoolFinancialInvestigations = 0,
			// pupil numbers
			SchoolCapacityYear1 = 189,
			SchoolCapacityYear2 = 189,
			SchoolCapacityYear3 = 189,
			SchoolCapacityAssumptions = "spreadsheets",
			SchoolCapacityPublishedAdmissionsNumber = "210",
			// land and buildings
			SchoolBuildLandOwnerExplained = "The Diocese of Warrington owns the building and the land. The LA owns the playing fields.",
			SchoolBuildLandWorksPlanned = 0,
			SchoolBuildLandSharedFacilities = 0,
			//	HasSchoolGrants = false,
			SchoolBuildLandPFIScheme = 0,
			SchoolBuildLandPriorityBuildingProgramme = 0,
			SchoolBuildLandFutureProgramme = 0,
			SchoolSupportGrantFundsPaidTo = 1,
			SchoolConsultationStakeholdersConsult = "yes",
			SchoolDeclarationSignedByName = "Garth Brown"
		};
	}
}
