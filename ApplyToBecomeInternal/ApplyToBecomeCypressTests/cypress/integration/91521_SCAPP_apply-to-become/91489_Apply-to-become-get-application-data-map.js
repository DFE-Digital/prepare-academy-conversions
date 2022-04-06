/// <reference types ='Cypress'/>
import {data} from "../../fixtures/cath121-body.json"

describe('91489: Apply-to-becom GET data application types', () => {
    let url = Cypress.env('url')
    let dataAppSch = data.applyingSchools[0]
    
    afterEach(() => {
        cy.storeSessionData()
    });
    
    before(() => {
        cy.login()
        cy.get('[id="school-name-44"]').click()
        cy.visit(url+'/school-application-form/519')
    });

    after(() => {
        cy.clearLocalStorage()
    });

    // Overview
    it('TC01: Overview - Application Lead', () => {
        cy.get('[test-id="Overview1_key"]').should('contain.text', 'Application to join')
        cy.get('[test-id="Overview1_value"]').should('contain.text', data.name + ' with ' + dataAppSch.schoolName)
        cy.get('[test-id="Overview2_key"]').should('contain.text', 'Application reference')
        cy.get('[test-id="Overview2_value"]').should('contain.text', data.applicationId)
        cy.get('[test-id="Overview3_key"]').should('contain.text', 'Lead applicant') 
        cy.get('[test-id="Overview3_value"]').should('contain.text', data.applicationLeadAuthorName)
    })

    // Details of Trust
    it('TC02: Trust details', () => {
        cy.get('[test-id="Overview_Details1_key"]').should('contain.text', 'Trust name')
        cy.get('[test-id="Overview_Details1_value"]').should('contain.text', data.name)
        // 'Upload evidence' IS NOT BE COVERED IN THIS TEST AS THESE ARE UPLOAD LINKS - [test-id="Overview_Details2_key"]
        // NOTE from Catherine "There is no expectation for this field to appear on the application form view at present."
        cy.get('[test-id="Overview_Details3_key"]').should('contain.text', 'Will there be any changes to the governance of the trust due to the school joining?') 
        cy.get('[test-id="Overview_Details3_value"]').should('contain.text', 'No')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.changesToLaGovernance).to.eq(false)
        })
        cy.get('[test-id="Overview_Details4_key"]').should('contain.text', 'Will there be any changes at a local level due to this school joining?') 
        cy.get('[test-id="Overview_Details4_value"]').should('contain.text', 'No')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.changesToTrust).to.eq(false)
        })
    })

    // About the conversion
    it('TC03: The School Joining the Trust', () => {
        cy.get('[test-id="About_the_conversion_The_school_joining_the_trust1_key"]').should('contain.text', 'The name of the school')
        cy.get('[test-id="About_the_conversion_The_school_joining_the_trust1_value"]').should('contain.text', dataAppSch.schoolName)
    })
    
    // Contain Details 
    it('TC04: Contact Details', () => {
        cy.get('[test-id="About_the_conversion_Contact_details1_key"]').should('contain.text', 'Name of headteacher') 
        cy.get('[test-id="About_the_conversion_Contact_details1_value"]').should('contain.text', dataAppSch.schoolConversionContactHeadName)
        cy.get('[test-id="About_the_conversion_Contact_details2_key"]').should('contain.text', "Headteacher's email address") 
        cy.get('[test-id="About_the_conversion_Contact_details2_value"]').should('contain.text', dataAppSch.schoolConversionContactHeadEmail)
        cy.get('[test-id="About_the_conversion_Contact_details3_key"]').should('contain.text', "Headteacher's telephone number") 
        cy.get('[test-id="About_the_conversion_Contact_details3_value"]').should('contain.text', dataAppSch.schoolConversionContactHeadTel)
        cy.get('[test-id="About_the_conversion_Contact_details4_key"]').should('contain.text', 'Name of the chair of the Governing Body') 
        cy.get('[test-id="About_the_conversion_Contact_details4_value"]').should('contain.text', dataAppSch.schoolConversionContactChairName)
        cy.get('[test-id="About_the_conversion_Contact_details5_key"]').should('contain.text', "Chair's email address") 
        cy.get('[test-id="About_the_conversion_Contact_details5_value"]').should('contain.text', dataAppSch.schoolConversionContactChairEmail)
        cy.get('[test-id="About_the_conversion_Contact_details6_key"]').should('contain.text', "Chair's phone number") 
        cy.get('[test-id="About_the_conversion_Contact_details6_value"]').should('contain.text', dataAppSch.schoolConversionContactChairTel)
        cy.get('[test-id="About_the_conversion_Contact_details7_key"]').should('contain.text', "Approver's name") 
        cy.get('[test-id="About_the_conversion_Contact_details7_value"]').should('contain.text', dataAppSch.schoolConversionApproverContactName)
    })

    // Date for Conversion
    it('TC05: Date for Conversion', () => {
        cy.get('[test-id="About_the_conversion_Date_for_conversion1_key"]').should('contain.text', 'Do you want the conversion to happen on a particular date')
        cy.get('[test-id="About_the_conversion_Date_for_conversion1_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolConversionTargetDateSpecified).to.eq(true)
        })
        cy.get('[test-id="About_the_conversion_Date_for_conversion2_key"]').should('contain.text', 'Preferred date')
        cy.get('[test-id="About_the_conversion_Date_for_conversion2_value"]').should('contain.text', '1 May 2021')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolConversionTargetDate).to.eq('2021-05-01T06:03:43.824')
        })
    })

    // Reasons for Joining 
    it('TC06: Reasons for Joining', () => {
        cy.get('[test-id="About_the_conversion_Reasons_for_joining1_key"]').should('contain.text', 'Why does the school want to join this trust in particular?')
        cy.get('[test-id="About_the_conversion_Reasons_for_joining1_value"]').should('contain.text', dataAppSch.schoolConversionReasonsForJoining)
    })

    // Name changes
    it('TC07: Name Changes', () => {
        cy.get('[test-id="About_the_conversion_Name_changes1_key"]').should('contain.text','Is the school planning to change its name when it becomes an academy?')
        cy.get('[test-id="About_the_conversion_Name_changes1_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolConversionChangeNamePlanned).to.eq(true)
        })
    })

    // Further Information
    it('TC08: Additional Details', () => {
        cy.get('[test-id="Further_information_Additional_details1_key"]').should('contain.text', 'What will the school bring to the trust they are joining?')
        cy.get('[test-id="Further_information_Additional_details1_value"]').should('contain.text',  dataAppSch.schoolAdSchoolContributionToTrust)
        cy.get('[test-id="Further_information_Additional_details2_key"]').should('contain.text', 'Have Ofsted inspected the school but not published the report yet?')
        cy.get('[test-id="Further_information_Additional_details2_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolAdInspectedButReportNotPublished).to.eq(true)
        })
        // "schoolAdInspectedButReportNotPublishedExplain": "Et occaecati dolore." - NOT DISPLAYED ON FRONTEND
        // NOTE from Catherine "will be added as part of ticket 83618 - 'Add follow up answers to questions on application form'"
        cy.get('[test-id="Further_information_Additional_details3_key"]').should('contain.text', 'Are there any safeguarding investigations ongoing at the school?')
        cy.get('[test-id="Further_information_Additional_details3_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolOngoingSafeguardingInvestigations).to.eq(true)
        })
        cy.get('[test-id="Further_information_Additional_details4_key"]').should('contain.text', 'Is the school part of a local authority reorganisation?')
        cy.get('[test-id="Further_information_Additional_details4_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolPartOfLaReorganizationPlan).to.eq(true)
        })
        cy.get('[test-id="Further_information_Additional_details5_key"]').should('contain.text', 'Is the school part of any local authority closure plans?')
        cy.get('[test-id="Further_information_Additional_details5_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolPartOfLaClosurePlan).to.eq(true)
        })
        cy.get('[test-id="Further_information_Additional_details6_key"]').should('contain.text', 'Is your school linked to a diocese?')
        cy.get('[test-id="Further_information_Additional_details6_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolFaithSchool).to.eq(true)
        })
        cy.get('[test-id="Further_information_Additional_details7_key"]').should('contain.text', 'Is your school part of a federation?')
        cy.get('[test-id="Further_information_Additional_details7_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolIsPartOfFederation).to.eq(true)
        })
        cy.get('[test-id="Further_information_Additional_details8_key"]').should('contain.text', 'Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?')
        cy.get('[test-id="Further_information_Additional_details8_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolIsSupportedByFoundation).to.eq(true)
        })
        //"schoolSupportedFoundationBodyName": "Sed eius et." - THIS IS NOT INCLUDED ON THE FRONTEND
        // NOTE from Catherine "will be added as part of ticket 83618 - 'Add follow up answers to questions on application form'"
        cy.get('[test-id="Further_information_Additional_details9_key"]').should('contain.text', 'Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?')
        cy.get('[test-id="Further_information_Additional_details9_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolHasSACREException).to.eq(true)
        })
        cy.get('[test-id="Further_information_Additional_details10_key"]').should('contain.text', 'Please provide a list of your main feeder schools')
        cy.get('[test-id="Further_information_Additional_details10_value"]').should('contain.text', dataAppSch.schoolAdFeederSchools)
        // 'Upload evidence' IS NOT BE COVERED IN THIS TEST AS THESE ARE UPLOAD LINKS- [test-id="Further_information_Additional_details11_key"]
        // NOTES from Catherine "will be added as part of ticket 83618 - 'Add follow up answers to questions on application form'"
        cy.get('[test-id="Further_information_Additional_details12_key"]').should('contain.text', 'Has an equalities impact assessment been carried out and considered by the governing body?')
        cy.get('[test-id="Further_information_Additional_details12_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolAdEqualitiesImpactAssessmentCompleted).to.eq(true)
        })
        // "schoolAdEqualitiesImpactAssessmentDetails": "Odio ipsam facilis.", THIS IS NOT INCLUDED ON THE FRONTEND
        // NOTE from Catherine "schoolAdEqualitiesImpactAssessmentDetails will be added as part of ticket 83618 - 'Add follow up answers to questions on application form'"
        cy.get('[test-id="Further_information_Additional_details13_key"]').should('contain.text', 'Do you want to add any further information?')
        cy.get('[test-id="Further_information_Additional_details13_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolAdditionalInformationAdded).to.eq(true)
        })
        // "schoolAdditionalInformation": "Animi illo ut." - NOT DISPLAYED ON FRONTEND
        // NOTE from Catherine "will be added as part of ticket 83618 - 'Add follow up answers to questions on application form'"
    })

    // Finance Details 
    it('TC09: Previous Financial Year', () => {
        cy.get('[test-id="Finances_Previous_financial_year1_key"]').should('contain.text', 'End of previous financial year')
        cy.get('[test-id="Finances_Previous_financial_year1_value"]').should('contain.text', '01/09/2021')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].previousFinancialYear.fyEndDate).to.eq('2021-09-01T17:49:39.303')
        })
        cy.get('[test-id="Finances_Previous_financial_year2_key"]').should('contain.text', 'Revenue carry forward at the end of the previous financial year (31 March)')
        cy.get('[test-id="Finances_Previous_financial_year2_value"]').should('contain.text', '-£3,300.00')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].previousFinancialYear.revenueCarryForward).to.eq(-3300.00)
        })
        cy.get('[test-id="Finances_Previous_financial_year3_key"]').should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Previous_financial_year3_value"]').should('contain.text', 'Deficit')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].previousFinancialYear.revenueIsDeficit).to.eq(true)
        })
        cy.get('[test-id="Finances_Previous_financial_year4_key"]').should('contain.text', 'Capital carry forward at the end of the previous financial year (31 March)')
        cy.get('[test-id="Finances_Previous_financial_year4_value"]').should('contain.text', '£1,577.00')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].previousFinancialYear.capitalCarryForward).to.eq(1577.00)
        })
        cy.get('[test-id="Finances_Previous_financial_year5_key"]').should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Previous_financial_year5_value"]').should('contain.text', 'Surplus')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].previousFinancialYear.capitalIsDeficit).to.eq(false)
        // "capitalStatusExplained": "Soluta repellendus assumenda." NOT INCLUDED ON FRONTEND
        // NOTE from Catherine "will be added as part of ticket 83618 - 'Add follow up answers to questions on application form' and will appear on the view only if capitalIsDeficit == true"
        })
    })
    
    // Current Financial Year
    it('TC10: Current Financial Year', () => {
        cy.get('[test-id="Finances_Current_financial_year1_key"]').should('contain.text', 'End of current financial year')
        cy.get('[test-id="Finances_Current_financial_year1_value"]').should('contain.text', '16/12/2021')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].currentFinancialYear.fyEndDate).to.eq('2021-12-16T17:05:19.845')
        })
        cy.get('[test-id="Finances_Current_financial_year2_key"]').should('contain.text', 'Forecasted revenue carry forward at the end of the current financial year (31 March)')
        cy.get('[test-id="Finances_Current_financial_year2_value"]').should('contain.text', '£4,160.00')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].currentFinancialYear.revenueCarryForward).to.eq(4160.00)
        })
        cy.get('[test-id="Finances_Current_financial_year3_key"]').should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Current_financial_year3_value"]').should('contain.text', 'Surplus')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].currentFinancialYear.revenueIsDeficit).to.eq(false)
        })
        cy.get('[test-id="Finances_Current_financial_year4_key"]').should('contain.text', 'Forecasted capital carry forward at the end of the current financial year (31 March)')
        cy.get('[test-id="Finances_Current_financial_year4_value"]').should('contain.text', '£5,895.00')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].currentFinancialYear.capitalCarryForward).to.eq(5895.00)
        })
        cy.get('[test-id="Finances_Current_financial_year5_key"]').should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Current_financial_year5_value"]').should('contain.text', 'Deficit')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].currentFinancialYear.capitalIsDeficit).to.eq(true)
        })
    })

    // Next Financial Year
    it('TC11: Next Financial Year', () => {
        cy.get('[test-id="Finances_Next_financial_year1_key"]').should('contain.text', 'End of next financial year')
        cy.get('[test-id="Finances_Next_financial_year1_value"]').should('contain.text', '16/09/2021')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].nextFinancialYear.fyEndDate).to.eq('2021-09-16T19:13:02.81')
        })
        cy.get('[test-id="Finances_Next_financial_year2_key"]').should('contain.text', 'Forecasted revenue carry forward at the end of the next financial year (31 March)')
        cy.get('[test-id="Finances_Next_financial_year2_value"]').should('contain.text', '£6,943.00')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].nextFinancialYear.revenueCarryForward).to.eq(6943.00)
        })
        cy.get('[test-id="Finances_Next_financial_year3_key"]').should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Next_financial_year3_value"]').should('contain.text', 'Deficit')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].nextFinancialYear.revenueIsDeficit).to.eq(true)
        })
        cy.get('[test-id="Finances_Next_financial_year4_key"]').should('contain.text', 'Forecasted capital carry forward at the end of the next financial year (31 March)')
        cy.get('[test-id="Finances_Next_financial_year4_value"]').should('contain.text', '£1,953.00')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].nextFinancialYear.capitalCarryForward).to.eq(1953.00)
        })
        cy.get('[test-id="Finances_Next_financial_year5_key"]').should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Next_financial_year5_value"]').should('contain.text', 'Deficit')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].nextFinancialYear.capitalIsDeficit).to.eq(true)
        })
    })

    // Loans Details
    it('TC12: Loans Details', () => {
        cy.get('[test-id="Finances_Loans1_key"]').should('contain.text', 'Are there any existing loans?')
        cy.get('[test-id="Finances_Loans1_value"]').should('contain.text', 'Yes') // THIS IS NOT REPRESENTED IN THE JSON BODY | NOTE from Catherine "This is set as 'yes' if there are loans to display and 'No' if there aren't. You're right - it doesn't have its own json field"
        cy.get('[test-id="Finances_Loans2_key"]').should('contain.text', 'Total amount')
        cy.get('[test-id="Finances_Loans2_value"]').should('contain.text', '£1,000.00')
        cy.get('[test-id="Finances_Loans3_key"]').should('contain.text', 'Purpose of the loan')
        cy.get('[test-id="Finances_Loans3_value"]').should('contain.text', dataAppSch.schoolLoans[0].schoolLoanPurpose)
        cy.get('[test-id="Finances_Loans4_key"]').should('contain.text', 'Loan provider')
        cy.get('[test-id="Finances_Loans4_value"]').should('contain.text', dataAppSch.schoolLoans[0].schoolLoanProvider)
        cy.get('[test-id="Finances_Loans5_key"]').should('contain.text', 'Interest rate')
        cy.get('[test-id="Finances_Loans5_value"]').should('contain.text',  dataAppSch.schoolLoans[0].schoolLoanInterestRate)
        cy.get('[test-id="Finances_Loans6_key"]').should('contain.text', 'Schedule of repayment')
        cy.get('[test-id="Finances_Loans6_value"]').should('contain.text', dataAppSch.schoolLoans[0].schoolLoanSchedule)
    })

    // Financial leases
    it('TC13: Financial Leases', () => {
        cy.get('[test-id="Finances_Financial_leases1_key"]').should('contain.text', 'Are there any existing leases?')
        cy.get('[test-id="Finances_Financial_leases1_value"]').should('contain.text', 'Yes') // THIS IS NOT REPRESENTED IN THE JSON BODY | NOTE from Catherine "This is set as 'yes' if there are leases to display and 'No' if there aren't, as with loans"
        cy.get('[test-id="Finances_Financial_leases2_key"]').should('contain.text', 'Details of the term of the finance lease agreement')
        cy.get('[test-id="Finances_Financial_leases2_value"]').should('contain.text', dataAppSch.schoolLeases[0].schoolLeaseTerm)
        cy.get('[test-id="Finances_Financial_leases3_key"]').should('contain.text', 'Repayment value')
        cy.get('[test-id="Finances_Financial_leases3_value"]').should('contain.text', '£2,000.00')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolLeases[0].schoolLeaseRepaymentValue).to.eq(2000.00)
        })
        cy.get('[test-id="Finances_Financial_leases4_key"]').should('contain.text', 'Interest rate chargeable')
        cy.get('[test-id="Finances_Financial_leases4_value"]').should('contain.text', '10.00%')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolLeases[0].schoolLeaseInterestRate).to.eq(10.00)
        })
        cy.get('[test-id="Finances_Financial_leases5_key"]').should('contain.text', 'Value of payments made to date')
        cy.get('[test-id="Finances_Financial_leases5_value"]').should('contain.text', '£200.00')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolLeases[0].schoolLeasePaymentToDate).to.eq(200.00)
        })
        cy.get('[test-id="Finances_Financial_leases6"]').should('contain.text', 'What was the finance lease for?')
        cy.get('[test-id="Finances_Financial_leases6_value"]').should('contain.text', dataAppSch.schoolLeases[0].schoolLeasePurpose)
        cy.get('[test-id="Finances_Financial_leases7_key"]').should('contain.text', 'Value of the assests at the start of the finance lease agreement')
        cy.get('[test-id="Finances_Financial_leases7_value"]').should('contain.text', dataAppSch.schoolLeases[0].schoolLeaseValueOfAssets)
        cy.get('[test-id="Finances_Financial_leases8_key"]').should('contain.text', 'Who is responsible for the insurance, repair and maintenance of the assets covered?')
        cy.get('[test-id="Finances_Financial_leases8_value"]').should('contain.text', 'who is responsible')
    })

    // Financial investigations
    it('TC14: Financial Investigations', () => {
        cy.get('[test-id="Finances_Financial_investigations1_key"]').should('contain.text', 'Are there any financial investigations ongoing at the school?')
        cy.get('[test-id="Finances_Financial_investigations1_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].financeOngoingInvestigations).to.eq(true)
        })
        cy.get('[test-id="Finances_Financial_investigations2_key"]').should('contain.text', 'Provide a brief summary of the investigation')
        cy.get('[test-id="Finances_Financial_investigations2_value"]').should('contain.text', dataAppSch.schoolFinancialInvestigationsExplain)
        cy.get('[test-id="Finances_Financial_investigations3_key"]').should('contain.text', 'Is the trust you are joining aware of the investigation')
        cy.get('[test-id="Finances_Financial_investigations3_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolFinancialInvestigationsTrustAware).to.eq(true)
        })
    })

    // Future Pupil numbers
    it('TC15: Future Pupil Numbers Details', () => {
        cy.get('[test-id="Future_pupil_numbers_Details1_key"]').should('contain.text', 'Projected pupil numbers on roll in the year the academy opens (year 1)')
        cy.get('[test-id="Future_pupil_numbers_Details1_value"]').should('contain.text', '45')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolCapacityYear1).to.eq(45)
        })
        cy.get('[test-id="Future_pupil_numbers_Details2_key"]').should('contain.text', 'Projected pupil numbers on roll in the following year after the academy has opened (year 2)')
        cy.get('[test-id="Future_pupil_numbers_Details2_value"]').should('contain.text', '12')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolCapacityYear2).to.eq(12)
        })
        cy.get('[test-id="Future_pupil_numbers_Details3_key"]').should('contain.text', 'Projected pupil numbers on roll in the following year (year 3)')
        cy.get('[test-id="Future_pupil_numbers_Details3_value"]').should('contain.text', '44')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].schoolCapacityYear3).to.eq(44)
        })
        cy.get('[test-id="Future_pupil_numbers_Details4_key"]').should('contain.text', 'What do you base these projected numbers on?')
        cy.get('[test-id="Future_pupil_numbers_Details4_value"]').should('contain.text',  dataAppSch.schoolCapacityAssumptions)
        cy.get('[test-id="Future_pupil_numbers_Details5_key"]').should('contain.text', "What is the school's published admissions number (PAN)?")
        cy.get('[test-id="Future_pupil_numbers_Details5_value"]').should('contain.text', '0')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
    })

    // Land & Buildings
    it('TC16: Land & Buildings Details', () => {
        cy.get('[test-id="Land_and_buildings_Details1_key"]').should('contain.text', "As far as you're aware, who owns or holds the school's buildings and land?")
        cy.get('[test-id="Land_and_buildings_Details1_value"]').should('contain.text', dataAppSch.schoolBuildLandOwnerExplained)
        cy.get('[test-id="Land_and_buildings_Details2_key"]').should('contain.text', 'Are there any current planned building works?')
        cy.get('[test-id="Land_and_buildings_Details2_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
        cy.get('[test-id="Land_and_buildings_Details3_key"]').should('contain.text', 'Are there any shared facilities on site?')
        cy.get('[test-id="Land_and_buildings_Details3_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
        cy.get('[test-id="Land_and_buildings_Details4_key"]').should('contain.text', 'Has the school had any grants from Sport England, the Big Lottery Fund, or the Football Federation?')
        cy.get('[test-id="Land_and_buildings_Details4_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
        cy.get('[test-id="Land_and_buildings_Details5_key"]').should('contain.text', 'Is the school part of a Private Finance Intiative (PFI) scheme?')
        cy.get('[test-id="Land_and_buildings_Details5_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
        cy.get('[test-id="Land_and_buildings_Details6_key"]').should('contain.text', 'Is the school part of a Priority School Building Programme?')
        cy.get('[test-id="Land_and_buildings_Details6_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
        cy.get('[test-id="Land_and_buildings_Details7_key"]').should('contain.text', 'Is the school part of a Building Schools for the Future Programme?')
        cy.get('[test-id="Land_and_buildings_Details7_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
    })

    // Pre-opening support grant
    it('TC17: Pre-Opening Support Grant Details', () => {
        cy.get('[test-id="Pre-opening_support_grant_Details1_key"]').should('contain.text', 'Do you want these funds paid to the school or the trust?')
        cy.get('[test-id="Pre-opening_support_grant_Details1_value"]').should('contain.text', dataAppSch.schoolSupportGrantFundsPaidTo)
    })

    // Consultation Details
    it('TC18: Consultation Details', () => {
        cy.get('[test-id="Consultation_Details1_key"]').should('contain.text', 'Has the governing body consulted the relevant stakeholders?')
        cy.get('[test-id="Consultation_Details1_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
    })

    // Declaration Details
    it('TC19: Declaration Details', () => {
        cy.get('[test-id="Declaration_Details1_key"]').should('contain.text', 'I agree with all of these statements, and believe that the facts stated in this application are true')
        cy.get('[test-id="Declaration_Details1_value"]').should('contain.text', 'Yes')
        cy.fixture('cath121-body.json').as('userData').then((userData) => {
            expect(userData.data.applyingSchools[0].declarationBodyAgree).to.eq(true)
        })
        cy.get('[test-id="Declaration_Details2_key"]').should('contain.text', 'Signed by')
        cy.get('[test-id="Declaration_Details2_value"]').should('contain.text', dataAppSch.declarationSignedByName)
    })
});