/// <reference types ='Cypress'/>

// TO DO: Check Legal Requirement validation on first time use; check Empty tags.
//import legalRequirements from "../../pages/legalRequirement"

describe('Legal Requirements', { tags: ['@dev', '@stage']}, () => {
    beforeEach(() => {
        cy.selectFirstProject().then(() =>{
        cy.get('[data-cy="select-tasklist-links-legalrequirements"]').click()
        })
    })

    it('TC01: Answer to Governing Body and changes current answer from Yes, No, Not Applicable ', () => {
        // Clicks on link
        cy.govBodyChangeLink()
        // Selects Yes
        cy.get('[data-cy="select-legal-input-yes"]').click()
        cy.saveAndContinueButton().click()
        cy.govBodyStatus().should('contain.text', 'Yes')
        // Clicks on link
        cy.govBodyChangeLink()
        // Selects No
        cy.get('[data-cy="select-legal-input-no"]').click()
        cy.saveAndContinueButton().click()
        cy.govBodyStatus().should('contain.text', 'No')
        // Clicks on link
        cy.govBodyChangeLink()
        // Selects Not applicable
        cy.get('[data-cy="select-legal-input-notapplicable"]').click()
        cy.saveAndContinueButton().click()
        cy.govBodyStatus().should('contain.text', 'Not applicable')
    })

    it('TC02: Answer to Consultaton and changes current answer from Yes, No, Not Applicable', () => {
        // Clicks on change link
        cy.consultationChangeLink()
        // Selects Yes
        cy.get('[data-cy="select-legal-input-yes"]').click()
        cy.saveAndContinueButton().click()
        cy.consultationStatus().should('contain.text', 'Yes')
        // Clicks on change link
        cy.consultationChangeLink()
        // Selects No
        cy.get('[data-cy="select-legal-input-no"]').click()
        cy.saveAndContinueButton().click()
        cy.consultationStatus().should('contain.text', 'No')
        // Clicks on change link
        cy.consultationChangeLink()
        // Selects Not appicable
        cy.get('[data-cy="select-legal-input-notapplicable"]').click()
        cy.saveAndContinueButton().click()
        cy.consultationStatus().should('contain.text', 'Not applicable')
    })

    it('TC03: Answer to Diocesan consent and changes current answer from Yes, No, Not Applicable', () => {
        // Clicks on change link
        cy.diocesanConsentChangeLink()
        // Selects Yes
        cy.get('[data-cy="select-legal-input-yes"]').click()
        cy.saveAndContinueButton().click()
        cy.diocesanConsentStatus().should('contain.text', 'Yes')
        // Clicks on change link
        cy.diocesanConsentChangeLink()
        // Selects No
        cy.get('[data-cy="select-legal-input-no"]').click()
        cy.saveAndContinueButton().click()
        cy.diocesanConsentStatus().should('contain.text', 'No')
        // Clicks on change link
        cy.diocesanConsentChangeLink()
        // Selects Not applicable
        cy.get('[data-cy="select-legal-input-notapplicable"]').click()
        cy.saveAndContinueButton().click()
        cy.diocesanConsentStatus().should('contain.text', 'Not applicable')
    })

    it('TC04: Answer to Foundation consent and changes current answer from Yes, No, Not Applicable', () => {
        // Clicks on change link
        cy.foundataionConsentChange()
        // Selects Yes
        cy.get('[data-cy="select-legal-input-yes"]').click()
        cy.saveAndContinueButton().click()
        cy.foundationConsentStatus().should('contain.text', 'Yes')
        // Clicks on change link
        cy.foundataionConsentChange()
        // Selects No
        cy.get('[data-cy="select-legal-input-no"]').click()
        cy.saveAndContinueButton().click()
        cy.foundationConsentStatus().should('contain.text', 'No')
        // Clicks on change link
        cy.foundataionConsentChange()
        // Selects Not applicable
        cy.get('[data-cy="select-legal-input-notapplicable"]').click()
        cy.saveAndContinueButton().click()
        cy.foundationConsentStatus().should('contain.text', 'Not applicable')
    })

    it('TC05: Confirm Legal Requirements page check & marked complete', () => {
        cy.selectFirstProject().then(() => {
            cy.get('[data-cy="select-tasklist-legalrequirements-status"]')
            .invoke('text')
            .then((text) => {
                if (text.includes('Completed')) {
                    return
                }
                else {
                    cy.get('[data-cy="select-tasklist-links-legalrequirements"]').click()
                    cy.get('[data-cy="select-legal-summary-iscomplete"]').click()
                    cy.get('[data-cy="select-legal-summary-submitbutton"]').click()
                    cy.get('[data-cy="select-tasklist-legalrequirements-status"]').should('contain.text', 'Completed')
                }
        })
        })
    })

    it('TC06: Back to task list button link', () => {
        cy.get('[data-cy="select-backlink"]')
        .should('be.visible')
        .should('contain.text', 'Back to task list').click()
    })

})