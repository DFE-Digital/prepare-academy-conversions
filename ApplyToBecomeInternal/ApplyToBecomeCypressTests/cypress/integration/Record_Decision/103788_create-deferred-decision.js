/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
let url = Cypress.env('url') + '/task-list/2054?rd=true'

describe('Create Deferred journey', () => {
    beforeEach(() => {
        // delete decision
        cy.sqlServer('DELETE FROM [academisation].[ConversionAdvisoryBoardDecision] WHERE ConversionProjectId = 2054')
        cy.clearCookies()
        cy.visit(url)
    })

    // Edit Deferred Path - Regional Director, Additional information needed 
    it('TC01: J3 Create a recorded decision Deferred - Reg Director Region, Finance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Record a decision').click()
        //select iniital decision
        cy.deferredRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.addInfoNeededBox().click()
            .then(() => cy.addInfoNeededText().clear().type('Additional info'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // give deferred reason
        cy.deferredReasonChangeLink().click()
        // set details
        cy.addInfoNeededText().clear().type('Additional info 2nd time')

        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.deferredDecision().should('contain.text', 'Deferred')
        cy.deferredDecisionMadeBy().should('contain.text', 'Regional Director for the region')
        // check reasons
        var deferredReasons = cy.get('[id="deferred-reasons"]')
        deferredReasons.should('contain.text', 'Additional information needed:')
        deferredReasons.should('contain.text', 'Additional info 2nd time')
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.visit(Cypress.env('url') + '/project-list')
        cy.projectStateId().should('contain.text', 'DEFERRED')
    })
})