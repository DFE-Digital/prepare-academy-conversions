/// <reference types ='Cypress'/>

import { data } from "../../fixtures/stagingProject-body.json"

//***************requres environment setup on yml file******************
describe('Fetch data from Internal', { tags: ['@integration'] }, () => {
    let fetchProjectsStaging = ['1241']
    let url = Cypress.env('url')

    beforeEach(() => {
        cy.login()
        cy.get('[id="school-name-0"]').click()
        if (url.toString().includes('dev')) {
            Cypress.runner.stop()
        }
        else {
            cy.visit(url + '/school-application-form/' + fetchProjectsStaging)
        }
    })

    // Overview
    it('TC01: Overview - Application Lead', () => {
        // key=Question:
        cy.get('[test-id="Overview1_key"]')
            .should('be.visible')
            .should('contain.text', 'Application to join')
        // value=Answer:
        cy.get('[test-id="Overview1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    let nameSchoolname = data.trustName + ' with ' + data.applyingSchools[0].schoolName
                    expect(nameSchoolname)
                        .to.contain(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Overview2_key"]')
            .should('be.visible')
            .should('contain.text', 'Application reference')
        cy.get('[test-id="Overview2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applicationId)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Overview3_key"]')
            .should('be.visible')
            .should('contain.text', 'Lead applicant')
        cy.get('[test-id="Overview3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applicationLeadAuthorName)
                        .to.equal(text.trim())
                }
                else {
                    return null
                }
            })
    })

    // Details of Trust
    it('TC02: Trust details', () => {
        cy.get('[test-id="Overview_Details1_key"]')
            .should('be.visible')
            .should('contain.text', 'Trust name')
        cy.get('[test-id="Overview_Details1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.trustName)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Overview_Details2_key"]')
            .should('be.visible')
            .should('contain.text', 'Will there be any changes to the governance of the trust due to the school joining?')
        cy.get('[test-id="Overview_Details2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.changesToLaGovernance)
                        .to.be.a('boolean')
                }
                else return null
            })

        cy.get('[test-id="Overview_Details2_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes') {
                    cy.get('[test-id="Overview_Details3_key"]')
                        .should('be.visible')
                        .should('contain.text', 'What are the changes?')
                    expect(data.changesToTrustExplained)
                        .to.equal(text.trim())
                }
                else return
            })

    })

    // About the conversion
    it('TC03: The School Joining the Trust', () => {
        cy.get('[test-id="About_the_conversion_The_school_joining_the_trust1_key"]')
            .should('be.visible')
            .should('contain.text', 'The name of the school')
        cy.get('[test-id="About_the_conversion_The_school_joining_the_trust1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolName)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

    })

    // Contain Details 
    it('TC04: Contact Details', () => {
        cy.get('[test-id="About_the_conversion_Contact_details1_key"]')
            .should('be.visible')
            .should('contain.text', 'Name of headteacher')
        cy.get('[test-id="About_the_conversion_Contact_details1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionContactHeadName)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="About_the_conversion_Contact_details2_key"]')
            .should('be.visible')
            .should('contain.text', "Headteacher's email address")
        cy.get('[test-id="About_the_conversion_Contact_details2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionContactHeadEmail)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="About_the_conversion_Contact_details3_key"]')
            .should('be.visible')
            .should('contain.text', "Headteacher's phone number")
        cy.get('[test-id="About_the_conversion_Contact_details3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionContactHeadTel)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="About_the_conversion_Contact_details4_key"]')
            .should('be.visible')
            .should('contain.text', 'Name of the chair of the Governing Body')
        cy.get('[test-id="About_the_conversion_Contact_details4_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionContactChairName)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="About_the_conversion_Contact_details5_key"]')
            .should('be.visible')
            .should('contain.text', "Chair's email address")
        cy.get('[test-id="About_the_conversion_Contact_details5_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionContactChairEmail)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="About_the_conversion_Contact_details6_key"]')
            .should('be.visible')
            .should('contain.text', "Chair's phone number")
        cy.get('[test-id="About_the_conversion_Contact_details6_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionContactChairTel)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="About_the_conversion_Contact_details7_key"]')
            .should('be.visible')
            .should('contain.text', "Who is the main contact for the conversion?")
        cy.get('[test-id="About_the_conversion_Contact_details7_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionContactRole)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="About_the_conversion_Contact_details8_key"]')
            .should('be.visible')
            .should('contain.text', "Approver's name")
        cy.get('[test-id="About_the_conversion_Contact_details8_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionApproverContactName)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })
    })

    it('TC05: Date for Conversion', () => {
        cy.get('[test-id="About_the_conversion_Date_for_conversion1_key"]')
            .should('be.visible')
            .should('contain.text', 'Do you want the conversion to happen on a particular date')
        cy.get('[test-id="About_the_conversion_Date_for_conversion1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionTargetDateSpecified)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="About_the_conversion_Date_for_conversion2_key"]')
            .should('be.visible')
            .should('contain.text', 'Preferred date')
        cy.get('[test-id="About_the_conversion_Date_for_conversion2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedDateString = new Date(data.applyingSchools[0].schoolConversionTargetDate).toLocaleDateString('en-GB', {
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    })
                    expect(expectedDateString)
                        .to.deep.equal(text.trim())
                }
                else {
                    return null
                }
            })
    })

    // Reasons for Joining 
    it('TC06: Reasons for Joining', () => {
        cy.get('[test-id="About_the_conversion_Reasons_for_joining1_key"]')
            .should('be.visible')
            .should('contain.text', 'Why does the school want to join this trust in particular?')
        cy.get('[test-id="About_the_conversion_Reasons_for_joining1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolConversionReasonsForJoining)
                        .to.equal(textvalue.replaceAll('\r\n', '').trim())
                }
                else {
                    return null
                }
            })
    })

    // Name changes
    it('TC07: Name Changes', () => {
        cy.get('[test-id="About_the_conversion_Name_changes1_key"]')
            .should('be.visible')
            .should('contain.text', 'Is the school planning to change its name when it becomes an academy?')
        cy.get('[test-id="About_the_conversion_Name_changes1_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolConversionChangeNamePlanned)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })
    })

    // Further Information
    it('TC08: Additional Details', () => {
        cy.get('[test-id="Further_information_Additional_details1_key"]')
            .should('be.visible')
            .should('contain.text', 'What will the school bring to the trust they are joining?')
        cy.get('[test-id="Further_information_Additional_details1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolAdSchoolContributionToTrust)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details2_key"]')
            .should('be.visible')
            .should('contain.text', 'Have Ofsted inspected the school but not published the report yet?')
        cy.get('[test-id="Further_information_Additional_details2_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolAdInspectedButReportNotPublished)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details3_key"]')
            .should('be.visible')
            .should('contain.text', 'Provide the inspection date and a short summary of the outcome')
        cy.get('[test-id="Further_information_Additional_details3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolAdInspectedButReportNotPublishedExplain)
                        .to.equal(textvalue.replaceAll('\r\n', '').trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details4_key"]')
            .should('be.visible')
            .should('contain.text', 'Are there any safeguarding investigations ongoing at the school?')
        cy.get('[test-id="Further_information_Additional_details4_value"]')
            .invoke('text')
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolOngoingSafeguardingInvestigations)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details6_key"]')
            .should('be.visible')
            .should('contain.text', 'Is the school part of a local authority reorganisation?')
        cy.get('[test-id="Further_information_Additional_details6_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolPartOfLaReorganizationPlan)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details8_key"]')
            .should('be.visible')
            .should('contain.text', 'Is the school part of any local authority closure plans?')
        cy.get('[test-id="Further_information_Additional_details8_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolPartOfLaClosurePlan)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details10_key"]')
            .should('be.visible')
            .should('contain.text', 'Is your school linked to a diocese?')
        cy.get('[test-id="Further_information_Additional_details10_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolFaithSchool)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details12_key"]')
            .should('be.visible')
            .should('contain.text', 'Is your school part of a federation?')
        cy.get('[test-id="Further_information_Additional_details12_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolIsPartOfFederation)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details13_key"]')
            .should('be.visible')
            .should('contain.text', 'Is the school supported by a foundation, trust or other body (e.g. parish council) that appoints foundation governors?')
        cy.get('[test-id="Further_information_Additional_details13_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolIsSupportedByFoundation)
                        .to.equal(true || false)
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details14_key"]')
            .should('be.visible')
            .should('contain.text', 'Name of this body')
        cy.get('[test-id="Further_information_Additional_details14_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolSupportedFoundationBodyName)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details15_key"]')
            .should('be.visible')
            .should('contain.text', 'Does the school currently have an exemption from providing broadly Christian collective worship issued by the local Standing Committee on Religious Education (SACRE)?')
        cy.get('[test-id="Further_information_Additional_details15_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolHasSACREException)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details16_key"]')
            .should('contain.text', 'When does the exemption end?')
        cy.get('[test-id="Further_information_Additional_details16_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedDateString = new Date(data.applyingSchools[0].schoolSACREExemptionEndDate).toLocaleDateString('en-GB', {
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    })
                    expect(expectedDateString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        // 'Upload evidence' IS NOT BE COVERED IN THIS TEST AS THESE ARE UPLOAD LINKS- [test-id="Further_information_Additional_details11_key"]
        cy.get('[test-id="Further_information_Additional_details18_key"]')
            .should('be.visible')
            .should('contain.text', 'Has an equalities impact assessment been carried out and considered by the governing body?')
        cy.get('[test-id="Further_information_Additional_details18_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolAdEqualitiesImpactAssessmentCompleted)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details19_key"]')
            .should('be.visible')
            .should('contain.text', 'When the governing body considered the equality duty what did they decide?')
        cy.get('[test-id="Further_information_Additional_details19_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolAdEqualitiesImpactAssessmentDetails)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details20_key"]')
            .should('be.visible')
            .should('contain.text', 'Do you want to add any further information?')
        cy.get('[test-id="Further_information_Additional_details20_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Yes' || 'No') {
                    cy.url().then(urlString => {
                        let modifiedUrl = urlString
                        cy.request(
                            'GET',
                            modifiedUrl).then((response) => {
                                expect(data.applyingSchools[0].schoolAdditionalInformationAdded)
                                    .to.be.a('boolean')
                            })
                    })
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Further_information_Additional_details21_key"]')
            .should('be.visible')
            .should('contain.text', 'Add any further information')
        cy.get('[test-id="Further_information_Additional_details21_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolAdditionalInformation)
                        .to.equal(textvalue.replaceAll('\r\n', '').trim())
                }
                else {
                    return null
                }
            })
    })

    // Finance Details **NEEDS ADDITIONAL LOGIC: To fix once Apply-to-become External becomes available**
    it('TC09: Previous Financial Year', () => {
        cy.get('[test-id="Finances_Previous_financial_year1_key"]')
            .should('be.visible')
            .should('contain.text', 'End of previous financial year')
        cy.get('[test-id="Finances_Previous_financial_year1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].previousFinancialYear.fyEndDate)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Previous_financial_year2_key"]')
            .should('be.visible')
            .should('contain.text', 'Revenue carry forward at the end of the previous financial year (31 March)')
        cy.get('[test-id="Finances_Previous_financial_year2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].previousFinancialYear.revenueCarryForward).toLocaleString('en-GB', {
                        style: 'currency',
                        currency: 'GBP'
                    })
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Previous_financial_year3_key"]')
            .should('be.visible')
            .should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Previous_financial_year3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].previousFinancialYear.revenueIsDeficit)
                        .to.be.a('boolean')
                }
                else return null
            })

        //Conditional Tests:
        cy.get('[test-id="Finances_Previous_financial_year3_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Surplus' || 'Deficit') {
                    cy.get('[test-id="Finances_Previous_financial_year4"]')
                        .should('be.visible')
                        .should('contain.text', "Capital carry forward at the end of the previous financial year (31 March)")
                }

            })

        //Conditional Tests:
        cy.get('[test-id="Finances_Previous_financial_year3_value"]')
            .invoke('text').then(cy.log)
            .then((text) => {
                if (text === 'Surplus' || 'Deficit') {
                    cy.get('[test-id="Finances_Previous_financial_year4_value"]')
                        .invoke('text')
                        .then((text) => {
                            let textvalue = text.toString()
                            if (textvalue.length > 0) {
                                var expectedNumberString = new Number(data.applyingSchools[0].previousFinancialYear.capitalCarryForward).toLocaleString('en-GB', {
                                    style: 'currency',
                                    currency: 'GBP'
                                })
                                expect(expectedNumberString)
                                    .to.equal(textvalue.trim())
                            }
                            else {
                                return null
                            }
                        })

                }

            })

        cy.get('[test-id="Finances_Previous_financial_year4_key"]')
            .should('be.visible')
            .should('contain.text', 'Capital carry forward at the end of the previous financial year (31 March)')
        cy.get('[test-id="Finances_Previous_financial_year4_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].previousFinancialYear.capitalCarryForward).toLocaleString('en-GB', {
                        style: 'currency',
                        currency: 'GBP'
                    })
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })
    })

    // Current Financial Year  **NEEDS ADDITIONAL LOGIC: To fix once Apply-to-become External becomes available**
    it('TC10: Current Financial Year', () => {
        cy.get('[test-id="Finances_Current_financial_year1_key"]')
            .should('be.visible')
            .should('contain.text', 'End of current financial year')
        cy.get('[test-id="Finances_Current_financial_year1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].currentFinancialYear.fyEndDate)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Current_financial_year2_key"]')
            .should('be.visible')
            .should('contain.text', 'Forecasted revenue carry forward at the end of the current financial year (31 March)')
        cy.get('[test-id="Finances_Current_financial_year2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].currentFinancialYear.revenueCarryForward).toLocaleString('en-GB', {
                        style: 'currency',
                        currency: 'GBP'
                    })
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Current_financial_year3_key"]')
            .should('be.visible')
            .should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Current_financial_year3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].previousFinancialYear.revenueIsDeficit)
                        .to.be.a('boolean')
                }
                else return null
            })

        cy.get('[test-id="Finances_Current_financial_year4_key"]')
            .should('be.visible')
            .should('contain.text', 'Explain the reasons for the deficit, how the school plans to deal with it, and the recovery plan')
        cy.get('[test-id="Finances_Current_financial_year4_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].currentFinancialYear.capitalStatusExplained)
                        .to.equal(text.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Current_financial_year5_key"]')
            .should('be.visible')
            .should('contain.text', 'Forecasted capital carry forward at the end of the current financial year (31 March)')
        cy.get('[test-id="Finances_Current_financial_year5_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].currentFinancialYear.capitalCarryForward).toLocaleString('en-GB', {
                        style: 'currency',
                        currency: 'GBP'
                    })
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Current_financial_year6_key"]')
            .should('be.visible')
            .should('contain.text', 'Surplus or deficit?')
    })

    // Next Financial Year
    it('TC11: Next Financial Year', () => {
        cy.get('[test-id="Finances_Next_financial_year1_key"]')
            .should('be.visible')
            .should('contain.text', 'End of next financial year')
        cy.get('[test-id="Finances_Next_financial_year1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].nextFinancialYear.fyEndDate)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Next_financial_year2_key"]')
            .should('be.visible')
            .should('contain.text', 'Forecasted revenue carry forward at the end of the next financial year (31 March)')
        cy.get('[test-id="Finances_Next_financial_year2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].nextFinancialYear.revenueCarryForward).toLocaleString('en-GB', {
                        style: 'currency',
                        currency: 'GBP'
                    })
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Next_financial_year3_key"]')
            .should('be.visible')
            .should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Next_financial_year3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue === 'Deficit' || 'Surplus') {
                    expect(data.applyingSchools[0].nextFinancialYear.revenueIsDeficit)
                        .to.be.a('boolean')
                }
                else return null

            })

        cy.get('[test-id="Finances_Next_financial_year4_key"]')
            .should('be.visible')
            .should('contain.text', 'Forecasted capital carry forward at the end of the next financial year (31 March)')
        cy.get('[test-id="Finances_Next_financial_year4_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].nextFinancialYear.capitalCarryForward).toLocaleString('en-GB', {
                        style: 'currency',
                        currency: 'GBP'
                    })
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Next_financial_year5_key"]')
            .should('be.visible')
            .should('contain.text', 'Surplus or deficit?')
        cy.get('[test-id="Finances_Next_financial_year5_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue === 'Deficit' || 'Surplus') {
                    expect(data.applyingSchools[0].nextFinancialYear.capitalIsDeficit)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })
    })

    // Loans Details
    it('TC12: Loans Details', () => {

        cy.get('[test-id="Finances_Loans1_key"]')
            .should('be.visible')
            .should('contain.text', 'Are there any existing loans?')
        cy.get('[test-id="Finances_Loans1_value"]')
            .invoke('text').then(cy.log)
        // THIS IS NOT REPRESENTED IN THE JSON BODY | NOTE from Catherine "This is set as 'yes' if there are loans to display and 'No' if there aren't"

        cy.get('[test-id="Finances_Loans2_key"]')
            .should('be.visible')
            .should('contain.text', 'Total amount')
        cy.get('[test-id="Finances_Loans2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].schoolLoans[0].schoolLoanAmount).toLocaleString('en-GB', {
                        style: 'currency',
                        currency: 'GBP'
                    })
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Loans3_key"]')
            .should('be.visible')
            .should('contain.text', 'Purpose of the loan')
        cy.get('[test-id="Finances_Loans3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolLoans[0].schoolLoanPurpose)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Loans4_key"]')
            .should('be.visible')
            .should('contain.text', 'Loan provider')
        cy.get('[test-id="Finances_Loans4_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolLoans[0].schoolLoanProvider)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Loans5_key"]')
            .should('be.visible')
            .should('contain.text', 'Interest rate')
        cy.get('[test-id="Finances_Loans5_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolLoans[0].schoolLoanInterestRate)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Finances_Loans6_key"]')
            .should('be.visible')
            .should('contain.text', 'Schedule of repayment')
        cy.get('[test-id="Finances_Loans6_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolLoans[0].schoolLoanSchedule)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })
    })

    // Financial leases
    it('TC13: Financial Leases', () => {
        cy.get('[test-id="Finances_Financial_leases1_key"]')
            .should('be.visible')
            .should('contain.text', 'Are there any existing leases?')
        cy.get('[test-id="Finances_Financial_leases1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes') {
                    // THIS IS NOT REPRESENTED IN THE JSON BODY | NOTE from Catherine "This is set as 'yes' if there are leases to display and 'No' if there aren't, as with loans"

                    cy.get('[test-id="Finances_Financial_leases2_key"]')
                        .should('be.visible')
                        .should('contain.text', 'Details of the term of the finance lease agreement')
                    cy.get('[test-id="Finances_Financial_leases2_value"]')
                        .invoke('text')
                        .then((text) => {
                            let textvalue = text.toString()
                            if (textvalue.length > 0) {
                                expect(data.applyingSchools[0].schoolLeases[0].schoolLeaseTerm)
                                    .to.equal(textvalue.trim())
                            }
                            else {
                                return null
                            }
                        })
                    cy.get('[test-id="Finances_Financial_leases3_key"]')
                        .should('be.visible')
                        .should('contain.text', 'Repayment value')
                    cy.get('[test-id="Finances_Financial_leases3_value"]')
                        .invoke('text')
                        .then((text) => {
                            let textvalue = text.toString()
                            if (textvalue.length > 0) {
                                var expectedNumberString = new Number(data.applyingSchools[0].schoolLeases[0].schoolLeaseRepaymentValue).toLocaleString('en-GB', {
                                    style: 'currency',
                                    currency: 'GBP'
                                })
                                expect(expectedNumberString)
                                    .to.equal(textvalue.trim())
                            }
                            else {
                                return null
                            }
                        })

                    cy.get('[test-id="Finances_Financial_leases4_key"]')
                        .should('be.visible')
                        .should('contain.text', 'Interest rate chargeable')
                    cy.get('[test-id="Finances_Financial_leases4_value"]')
                        .invoke('text')
                        .then((text) => {
                            let textvalue = text.toString()
                            if (textvalue.length > 0) {
                                var expectedNumberString = new Number(data.applyingSchools[0].schoolLeases[0].schoolLeaseInterestRate / 100).toLocaleString('en-GB', {
                                    style: 'percent',
                                    maximumFractionDigits: 2
                                })
                                expect(parseFloat(expectedNumberString).toFixed(2) + "%")
                                    .to.equal(textvalue.trim())
                            }
                            else {
                                return null
                            }
                        })

                    cy.get('[test-id="Finances_Financial_leases5_key"]')
                        .should('be.visible')
                        .should('contain.text', 'Value of payments made to date')
                    cy.get('[test-id="Finances_Financial_leases5_value"]')
                        .invoke('text')
                        .then((text) => {
                            let textvalue = text.toString()
                            if (textvalue.length > 0) {
                                var expectedNumberString = new Number(data.applyingSchools[0].schoolLeases[0].schoolLeasePaymentToDate).toLocaleString('en-GB', {
                                    style: 'currency',
                                    currency: 'GBP'
                                })
                                expect(expectedNumberString)
                                    .to.equal(textvalue.trim())
                            }
                            else {
                                return null
                            }
                        })

                    cy.get('[test-id="Finances_Financial_leases6"]')
                        .should('be.visible')
                        .should('contain.text', 'What was the finance lease for?')
                    cy.get('[test-id="Finances_Financial_leases6_value"]')
                        .invoke('text')
                        .then((text) => {
                            let textvalue = text.toString()
                            if (textvalue.length > 0) {
                                expect(data.applyingSchools[0].schoolLeases[0].schoolLeasePurpose)
                                    .to.equal(textvalue.trim())
                            }
                            else {
                                return null
                            }
                        })

                    cy.get('[test-id="Finances_Financial_leases7_key"]')
                        .should('be.visible')
                        .should('contain.text', 'Value of the assests at the start of the finance lease agreement')
                    cy.get('[test-id="Finances_Financial_leases7_value"]')
                        .invoke('text')
                        .then((text) => {
                            let textvalue = text.toString()
                            if (textvalue.length > 0) {
                                expect(data.applyingSchools[0].schoolLeases[0].schoolLeaseValueOfAssets)
                                    .to.equal(textvalue.trim())
                            }
                            else {
                                return null
                            }
                        })

                    cy.get('[test-id="Finances_Financial_leases8_key"]')
                        .should('be.visible')
                        .should('contain.text', 'Who is responsible for the insurance, repair and maintenance of the assets covered?')
                    cy.get('[test-id="Finances_Financial_leases8_value"]')
                        .invoke('text')
                        .then((text) => {
                            let textvalue = text.toString()
                            if (textvalue.length > 0) {
                                expect(data.applyingSchools[0].schoolLeases[0].schoolLeaseResponsibleForAssets)
                                    .to.equal(textvalue.trim())
                            }
                            else {
                                return null
                            }
                        })
                }
                else {
                    return null
                }
            })
    })


    // Financial investigations
    it('TC14: Financial Investigations', () => {
        cy.get('[test-id="Finances_Financial_investigations1_key"]')
            .should('be.visible')
            .should('contain.text', 'Are there any financial investigations ongoing at the school?')
        cy.get('[test-id="Finances_Financial_investigations1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes' || 'No') {
                    expect(data.applyingSchools[0].financeOngoingInvestigations)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })
    })

    // Future Pupil numbers
    it('TC15: Future Pupil Numbers Details', () => {

        cy.get('[test-id="Future_pupil_numbers_Details1_key"]')
            .should('be.visible')
            .should('contain.text', 'Projected pupil numbers on roll in the year the academy opens (year 1)')
        cy.get('[test-id="Future_pupil_numbers_Details1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].schoolCapacityYear1).toLocaleString('en-GB')
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Future_pupil_numbers_Details2_key"]')
            .should('be.visible')
            .should('contain.text', 'Projected pupil numbers on roll in the following year after the academy has opened (year 2)')
        cy.get('[test-id="Future_pupil_numbers_Details2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].schoolCapacityYear2).toLocaleString('en-GB')
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Future_pupil_numbers_Details3_key"]')
            .should('be.visible')
            .should('contain.text', 'Projected pupil numbers on roll in the following year (year 3)')
        cy.get('[test-id="Future_pupil_numbers_Details3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].schoolCapacityYear3).toLocaleString('en-GB')
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Future_pupil_numbers_Details4_key"]')
            .should('be.visible')
            .should('contain.text', 'What do you base these projected numbers on?')
        cy.get('[test-id="Future_pupil_numbers_Details4_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolCapacityAssumptions)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Future_pupil_numbers_Details5_key"]')
            .should('be.visible')
            .should('contain.text', "What is the school's published admissions number (PAN)?")
        cy.get('[test-id="Future_pupil_numbers_Details5_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    var expectedNumberString = new Number(data.applyingSchools[0].schoolCapacityPublishedAdmissionsNumber).toLocaleString('en-GB')
                    expect(expectedNumberString)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })
    })

    // Land & Buildings
    it('TC16: Land & Buildings Details', () => {
        cy.get('[test-id="Land_and_buildings_Details1_key"]')
            .should('be.visible')
            .should('contain.text', "As far as you're aware, who owns or holds the school's buildings and land?")
        cy.get('[test-id="Land_and_buildings_Details1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolBuildLandOwnerExplained)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details2_key"]')
            .should('be.visible')
            .should('contain.text', 'Are there any current planned building works?')
        cy.get('[test-id="Land_and_buildings_Details2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolBuildLandWorksPlanned)
                        .to.be.a('boolean')
                }
                else return null

            })

        cy.get('[test-id="Land_and_buildings_Details3_key"]')
            .should('be.visible')
            .should('contain.text', 'Provide details of the works, how they\'ll be funded and whether the funding will be affected by the conversion')
        cy.get('[test-id="Land_and_buildings_Details3_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolBuildLandWorksPlannedExplained)
                        .to.equal(text.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details4_key"]')
            .should('be.visible')
            .should('contain.text', 'When is the scheduled completion date?')
        cy.get('[test-id="Land_and_buildings_Details4_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolBuildLandWorksPlannedCompletionDate)
                        .to.equal(text.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details5_key"]')
            .should('be.visible')
            .should('contain.text', 'Are there any shared facilities on site?')
        cy.get('[test-id="Land_and_buildings_Details5_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes' || 'No') {
                    expect(data.applyingSchools[0].schoolBuildLandSharedFacilities)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details6_key"]')
            .should('be.visible')
            .should('contain.text', 'List the facilities and the school\'s plan for them after converting')
        cy.get('[test-id="Land_and_buildings_Details6_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolBuildLandSharedFacilitiesExplained)
                        .to.equal(text.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details7_key"]')
            .should('be.visible')
            .should('contain.text', 'Has the school had any grants from Sport England, the Big Lottery Fund, or the Football Federation?')
        cy.get('[test-id="Land_and_buildings_Details7_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes' || 'No') {
                    expect(data.applyingSchools[0].schoolBuildLandGrants)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details8_key"]').should('contain.text', 'Which bodies awarded the grants and what facilities did they fund?')
        cy.get('[test-id="Land_and_buildings_Details8_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolBuildLandGrantsExplained)
                        .to.equal(text.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details9_key"]')
            .should('be.visible')
            .should('contain.text', 'Is the school part of a Private Finance Intiative (PFI) scheme?')
        cy.get('[test-id="Land_and_buildings_Details9_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes' || 'No') {
                    expect(data.applyingSchools[0].schoolBuildLandPFIScheme)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details10_key"]')
            .should('be.visible')
            .should('contain.text', 'What kind of PFI Scheme is your school part of?')
        cy.get('[test-id="Land_and_buildings_Details10_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolBuildLandPFISchemeType)
                        .to.equal(text.trim())
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details11_key"]')
            .should('be.visible')
            .should('contain.text', 'Is the school part of a Priority School Building Programme?')
        cy.get('[test-id="Land_and_buildings_Details11_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes' || 'No') {
                    expect(data.applyingSchools[0].schoolBuildLandPriorityBuildingProgramme)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Land_and_buildings_Details12_key"]')
            .should('be.visible')
            .should('contain.text', 'Is the school part of a Building Schools for the Future Programme?')
        cy.get('[test-id="Land_and_buildings_Details12_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes' || 'No') {
                    expect(data.applyingSchools[0].schoolBuildLandFutureProgramme)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })
    })

    // Pre-opening support grant
    it('TC17: Pre-Opening Support Grant Details', () => {
        cy.get('[test-id="Pre-opening_support_grant_Details1_key"]')
            .should('be.visible')
            .should('contain.text', 'Do you want these funds paid to the school or the trust?')
        cy.get('[test-id="Pre-opening_support_grant_Details1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].schoolSupportGrantFundsPaidTo)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })
    })

    // Consultation Details
    it('TC18: Consultation Details', () => {
        cy.get('[test-id="Consultation_Details1_key"]')
            .should('be.visible')
            .should('contain.text', 'Has the governing body consulted the relevant stakeholders?')
        cy.get('[test-id="Consultation_Details1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes' || 'No') {
                    expect(data.applyingSchools[0].declarationBodyAgree)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })
    })

    // Declaration Details
    it('TC19: Declaration Details', () => {
        cy.get('[test-id="Declaration_Details1_key"]')
            .should('be.visible')
            .should('contain.text', 'I agree with all of these statements, and believe that the facts stated in this application are true')
        cy.get('[test-id="Declaration_Details1_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue == 'Yes' || 'No') {
                    expect(data.applyingSchools[0].declarationBodyAgree)
                        .to.be.a('boolean')
                }
                else {
                    return null
                }
            })

        cy.get('[test-id="Declaration_Details2_key"]')
            .should('be.visible')
            .should('contain.text', 'Signed by')
        cy.get('[test-id="Declaration_Details2_value"]')
            .invoke('text')
            .then((text) => {
                let textvalue = text.toString()
                if (textvalue.length > 0) {
                    expect(data.applyingSchools[0].declarationSignedByName)
                        .to.equal(textvalue.trim())
                }
                else {
                    return null
                }
            })
    })
})
