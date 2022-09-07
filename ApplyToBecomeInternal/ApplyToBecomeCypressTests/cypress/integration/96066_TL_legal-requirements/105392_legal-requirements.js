/// <reference types ='Cypress'/>

// TO DO: Check Legal Requirement validation on first time use; check Empty tags.

let url = Cypress.env('url') + '/task-list/2054'

describe('Legal Requirements', () => {
    beforeEach(() => {
        cy.visit(url)
        cy.get('[data-cy="projectpage-tasklist-links-legalrequirements"]').click()
    })

    it('TC01: Governing body', () => {
        // Clicks on link
        cy.govBodyChangeLink()
        // Selects Yes
        cy.get('[data-cy="projectpage-legal-input-yes"]').click()
        cy.saveContinueBtn()
        cy.govBodyStatus().should('contain.text', 'Yes')
        // Clicks on link
        cy.govBodyChangeLink()
        // Selects No
        cy.get('[data-cy="projectpage-legal-input-no"]').click()
        cy.saveContinueBtn()
        cy.govBodyStatus().should('contain.text', 'No')
        // Clicks on link
        cy.govBodyChangeLink()
        // Selects Not applicable
        cy.get('[data-cy="projectpage-legal-input-notapplicable"]').click()
        cy.saveContinueBtn()
        cy.govBodyStatus().should('contain.text', 'Not Applicable')
    })

    it('TC02: Consultaton', () => {
        // Clicks on change link
        cy.consultationChangeLink()
        // Selects Yes
        cy.get('[data-cy="projectpage-legal-input-yes"]').click()
        cy.saveContinueBtn()
        cy.consultationStatus().should('contain.text', 'Yes')
        // Clicks on change link
        cy.consultationChangeLink()
        // Selects No
        cy.get('[data-cy="projectpage-legal-input-no"]').click()
        cy.saveContinueBtn()
        cy.consultationStatus().should('contain.text', 'No')
        // Clicks on change link
        cy.consultationChangeLink()
        // Selects Not appicable
        cy.get('[data-cy="projectpage-legal-input-notapplicable"]').click()
        cy.saveContinueBtn()
        cy.consultationStatus().should('contain.text', 'Not Applicable')
    })

    it('TC03: Diocesean consent', () => {
        // Clicks on change link
        cy.dioceseanConsentChangeLink()
        // Selects Yes
        cy.get('[data-cy="projectpage-legal-input-yes"]').click()
        cy.saveContinueBtn()
        cy.dioceseanConsentStatus().should('contain.text', 'Yes')
        // Clicks on change link
        cy.dioceseanConsentChangeLink()
        // Selects No
        cy.get('[data-cy="projectpage-legal-input-no"]').click()
        cy.saveContinueBtn()
        cy.dioceseanConsentStatus().should('contain.text', 'No')
        // Clicks on change link
        cy.dioceseanConsentChangeLink()
        // Selects Not applicable
        cy.get('[data-cy="projectpage-legal-input-notapplicable"]').click()
        cy.saveContinueBtn()
        cy.dioceseanConsentStatus().should('contain.text', 'Not Applicable')
    })

    it('TC04: Foundation consent', () => {
        // Clicks on change link
        cy.foundataionConsentChange()
        // Selects Yes
        cy.get('[data-cy="projectpage-legal-input-yes"]').click()
        cy.saveContinueBtn()
        cy.foundationConsentStatus().should('contain.text', 'Yes')
        // Clicks on change link
        cy.foundataionConsentChange()
        // Selects No
        cy.get('[data-cy="projectpage-legal-input-no"]').click()
        cy.saveContinueBtn()
        cy.foundationConsentStatus().should('contain.text', 'No')
        // Clicks on change link
        cy.foundataionConsentChange()
        // Selects Not applicable
        cy.get('[data-cy="projectpage-legal-input-notapplicable"]').click()
        cy.saveContinueBtn()
        cy.foundationConsentStatus().should('contain.text', 'Not Applicable')
    })

    it('TC05: Confirm Legal Requirements page check mark', () => {
        cy.visit(url)
        cy.get('[data-cy="projectpage-tasklist-legalrequirements-status"]')
        .invoke('text')
        .then((text) => {
            if (text.includes('Completed')) {
                return
            }
            else {
                cy.visit(url)
                cy.get('[data-cy="projectpage-legal-summary-iscomplete"]').click()
                cy.get('[data-cy="projectpage-legal-summary-submitbutton"]').click()
                cy.get('[data-cy="projectpage-tasklist-legalrequirements-status"]').should('contain.text', 'Completed')
            }
        })
    })

    it('TC06: Back to task list link', () => {
        cy.get('[data-cy="projectpage-backlink"]')
        .should('be.visible')
        .should('contain.text', 'Back to task list').click()
    })

})