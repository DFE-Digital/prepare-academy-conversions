/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
import ProjectList from '../../pages/projectList'

describe('103787 Error handling', { tags: '@dev'}, () => {
    beforeEach(() => {
        ProjectList.selectProject().then(() => {            
            cy.clearCookies()
        })
    })

    it('TC01: Error handling for the approved journey', () => {
        // Record the decision:
        cy.get('[id="record-decision-link"]').click()
        cy.continueBtn().click()
        cy.get('[id="AdvisoryBoardDecision-error-link"]').should('contain.text', 'Select a decision')
        cy.get('[id="approved-radio"]').click()
        cy.continueBtn().click()
        // Who made this decision:
        cy.continueBtn().click()
        cy.get('[id="DecisionMadeBy-error-link"]').should('contain.text', 'Select who made the decision')
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        cy.continueBtn().click()
        // Were conditions met:
        cy.continueBtn().click()
        cy.get('[id="ApprovedConditionsSet-error-link"]').should('contain.text', 'Select whether any conditions were set')
        cy.get('[id="no-radio"]').click()
        cy.continueBtn().click()
        // Date conversions:
        cy.continueBtn().click()
        cy.get('[data-module=govuk-error-summary]').should('contain.text', 'Enter the date when the conversion was approved')
    })

    it('TC02: Error handling for the declined journey', () => {
        // Record the decision:
        cy.get('[id="record-decision-link"]').click()
        cy.continueBtn().click()
        cy.get('[id="AdvisoryBoardDecision-error-link"]').should('contain.text', 'Select a decision')
        cy.get('[id="declined-radio"]').click()
        cy.continueBtn().click()
        // Who made this decision:
        cy.continueBtn().click()
        cy.get('[id="DecisionMadeBy-error-link"]').should('contain.text', 'Select who made the decision')
        cy.get('[id="regionaldirectorforregion-radio"]').click()

        // continue to declined reason page
        cy.continueBtn().click()

        // trigger validation
        cy.continueBtn().click()
        cy.get('[id="DeclinedReasonSet-error-link"]').should('contain.text', 'Select at least one reason')

        // check all boxes on form
        cy.declineFinancebox().click()
        cy.performanceBox().click()
        cy.governanceBox().click()
        cy.trustBox().click()
        cy.declineOtherbox().click()

        // trigger declined reasons validation
        cy.continueBtn().click()
        cy.get('[id="DeclineFinanceReason-error-link"]').should('contain.text', 'Enter a reason for selecting Finance')
        cy.get('[id="DeclinePerformanceReason-error-link"]').should('contain.text', 'Enter a reason for selecting Performance')
        cy.get('[id="DeclineGovernanceReason-error-link"]').should('contain.text', 'Enter a reason for selecting Governance')
        cy.get('[id="DeclineChoiceOfTrustReason-error-link"]').should('contain.text', 'Enter a reason for selecting Choice of trust')
        cy.get('[id="DeclineOtherReason-error-link"]').should('contain.text', 'Enter a reason for selecting Other')

        // continue to decision date form
        cy.performanceBox().click()
        cy.governanceBox().click()
        cy.trustBox().click()
        cy.declineOtherbox().click()
        cy.declineFinancText().clear().type('Finance reason....')
        cy.continueBtn().click()

        // trigger decision date validation
        cy.continueBtn().click()
        cy.get('[data-module=govuk-error-summary]').should('contain.text', 'Enter the date when the conversion was declined')
    })

    it('TC03: Error handling for the deferred journey', () => {
        // Record the decision:
        cy.get('[id="record-decision-link"]').click()
        cy.continueBtn().click()
        cy.get('[id="AdvisoryBoardDecision-error-link"]').should('contain.text', 'Select a decision')
        cy.get('[id="deferred-radio"]').click()
        cy.continueBtn().click()
        // Who made this decision:
        cy.continueBtn().click()
        cy.get('[id="DecisionMadeBy-error-link"]').should('contain.text', 'Select who made the decision')
        cy.get('[id="regionaldirectorforregion-radio"]').click()

        // continue to deferred reason page
        cy.continueBtn().click()

        // trigger validation
        cy.continueBtn().click()
        cy.get('[id="WasReasonGiven-error-link"]').should('contain.text', 'Select at least one reason')

        // check all boxes on form
        cy.addInfoNeededBox().click()
        cy.awaitOfstedReportBox().click()
        cy.performanceCheckBox().click()
        cy.OtherCheckBox().click()

        // trigger deferred reasons validation
        cy.continueBtn().click()
        cy.get('[id="AdditionalInformationNeededDetails-error-link"]').should('contain.text', 'Enter a reason for selecting Additional information needed')
        cy.get('[id="AwaitingNextOfstedReportDetails-error-link"]').should('contain.text', 'Enter a reason for selecting Awaiting next ofsted report')
        cy.get('[id="PerformanceConcernsDetails-error-link"]').should('contain.text', 'Enter a reason for selecting Performance concerns')
        cy.get('[id="OtherDetails-error-link"]').should('contain.text', 'Enter a reason for selecting Other')

        // continue to decision date form
        cy.addInfoNeededBox().click()
        cy.awaitOfstedReportBox().click()
        cy.performanceCheckBox().click()
        cy.OtherCheckText().clear().type('Other reason....')
        cy.continueBtn().click()

        // trigger decision date validation
        cy.continueBtn().click()
        cy.get('[data-module=govuk-error-summary]').should('contain.text', 'Enter the date when the conversion was deferred')
    })
})