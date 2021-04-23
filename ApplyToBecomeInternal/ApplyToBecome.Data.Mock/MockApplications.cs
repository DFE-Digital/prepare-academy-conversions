using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.Application;
using System;

namespace ApplyToBecome.Data.Mock
{
	public class MockApplications:IApplications
	{
		public Application GetApplication(string id)
		{
			return new Application
			{
				School = new School
				{
					Name = "St Wilfrid's Primary School"
				},
				Trust = new Trust
				{
					Name = "Dynamics Trust",
				},
				LeadApplicant = "Garth Brown",
				Details = new ApplicationDetails
				{
					EvidenceDocument = new Link("consent_dynamics.docx", "#"),
					ChangesToGovernance = false,
					ChangesAtLocalLevel = true
				},
				ConversionInformation = new ConversionInformation
				{
					HeadTeacher = new ContactDetails
					{
						Name = "Garth Brown",
						EmailAddress = "garth.brown@stwilfridsprimary.edu.uk",
						TelephoneNumber = "09876 64547563"
					},
					GoverningBodyChair = new ContactDetails
					{
						Name = "Arna Siggurdottier",
						EmailAddress = "arna.siggurdottier@dynamicstrust.co.uk",
						TelephoneNumber = "0972 345 119"
					},
					Approver = new ContactDetails
					{
						Name = "Garth Brown",
						EmailAddress = "garth.brown@stwilfridsprimary.edu.uk",
					},
					DateForConversion = new DateForConversion
					{
						HasPreferredDate = true,
						PreferredDate = new DateTime(2021,04,20)
					},
					SchoolToTrustRationale = "This is a rationale",
					WillSchoolChangeName = true
				},
				FurtherInformation = new FurtherInformation
				{
					WhatWillSchoolBringToTrust = "the school will bring these things to the trust",
					HasUnpublishedOfstedInspection = false,
					HasSafeguardingInvestigations = false,
					IsPartOfLocalAuthorityReorganisation = false,
					IsPartOfLocalAuthorityClosurePlans = false,
					IsLinkedToDiocese = true,
					NameOfDiocese = "Diocese of Warrington",
					DioceseLetterOfConsent = new Link("consent-from-diocese.docx", "#"),
					IsPartOfFederation = false,
					IsSupportedByFoundationTrustOrOtherBody = true,
					HasSACREChristianWorshipExcemption = false,
					MainFeederSchools = "n/a as we are a primary school",
					SchoolConsent = new Link("consent.docx", "#"),
					EqualitiesImpactAssessmentResult = "Considered unlikely",
					AdditionalInformation = null
				},
				Finances = new Finances
				{
					PreviousFinancialYear = new FinancialYear(new DateTime(2020, 03, 31), 169093, FinancialYearState.Surplus),
					CurrentFinancialYear = new ForecastFinancialYear(new DateTime(2021, 03, 31), 143931, FinancialYearState.Surplus, 0, FinancialYearState.Surplus),
					NextFinancialYear = new ForecastFinancialYear(new DateTime(2022, 03, 31), 169093, FinancialYearState.Surplus, 0, FinancialYearState.Surplus),
					ExistingLoans = false,
					ExistingLeases = true,
					OngoingInvestigations = false
				},
				FuturePupilNumbers = new FuturePupilNumbers
				{
					Year1 = 189,
					Year2 = 189,
					Year3 = 189,
					ProjectionReasoning = "spreadsheets 'n' shit",
					PublishedAdmissionsNumber = 210
				},
				LandAndBuildings = new LandAndBuildings
				{
					BuildingAndLandOwner = "The Diocese of Warrington owns the building and the land. The LA owns the playing fields.",
					HasCurrentPlannedBuildingWorks = true,
					HasSharedFacilitiesOnSite = false,
					HasSchoolGrants = false,
					HasPrivateFinanceInitiativeScheme = false,
					IsInPrioritySchoolBuildingProgramme = false,
					IsInBuildingSchoolsForFutureProgramme = false
				},
				FundsPaidToSchoolOrTrust = "To the trust the school is joining",
				HasGovernmentConsultedStakeholders = true,
				Declaration = new Declaration(true, "Garth Brown")
			};
		}
	}
}