/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
let url = Cypress.env('url') + '/task-list/2050?rd=true'

describe('103787 Error handling', () => {
    beforeEach(() => {
        cy.visit(url)
        cy.clearCookies()

    })

    it('Error handling for the approved journey', () => {
        // Record the decision:
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
        cy.continueBtn().click()
        cy.get('[id="AdvisoryBoardDecision-error-link "]').should('contain.text', 'Please select the result of the decision')
        cy.get('[id="approved-radio"]').click()
        cy.continueBtn().click()
        // Who made this decision:
        cy.continueBtn().click()
        cy.get('[id="DecisionMadeBy-error-link "]').should('contain.text', 'Select who made the decision')
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        cy.continueBtn().click()
        // Were conditions met:
        cy.continueBtn().click()
        cy.get('[id="ApprovedConditionsSet-error-link "]').should('contain.text', 'Please choose an option')
        cy.get('[id="no-radio"]').click()
        cy.continueBtn().click()
        // Date conversions:
        cy.continueBtn().click()
        cy.get('[data-module=govuk-error-summary]').should('contain.text', 'Enter the date when the conversion was approved')
    })

    it('Error handling for the declined journey', () => {
        // Record the decision:
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
        cy.continueBtn().click()
        cy.get('[id="AdvisoryBoardDecision-error-link "]').should('contain.text', 'Please select the result of the decision')
        cy.get('[id="declined-radio"]').click()
        cy.continueBtn().click()
        // Who made this decision:
        cy.continueBtn().click()
        cy.get('[id="DecisionMadeBy-error-link "]').should('contain.text', 'Select who made the decision')
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        cy.continueBtn().click()
        // Why was the project declined?
        // **Error Message needs updating on Frontend**
        //cy.continueBtn().click()
    })
})