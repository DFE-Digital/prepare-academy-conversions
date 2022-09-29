/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
import RecordDecision from '../../pages/recordDecision'

describe('103195 Record new Approved decision', () => {

    beforeEach(() => {
        RecordDecision.selectProject().then(id => {
            // delete decision
            cy.sqlServer(`DELETE FROM [academisation].[ConversionAdvisoryBoardDecision] WHERE ConversionProjectId = ${id}`)
            cy.clearCookies()
            //cy.visit(url)
            cy.url().then(url => cy.visit(`${url}?rd=true`))
        })
    })

    // Edit Approval Path - Regional Director, No/Yes conditions set
    it.only('TC01: J1 Create a new recorded decision Approval - Reg Director Region, No Conditions', () => {
        // Click on change your decision button
        cy.get('[id="record-decision-link"]').should('contain.text', 'Record a decision').click()
        //select iniital decision
        cy.get('[id="approved-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects 'no' on conditions met
        cy.NoRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.ChangeConditionsLink().click()
        cy.YesRadioBtn().click()
        cy.YesTextBox().clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        checkSummary()
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.ApprovedMessageBanner().should('contain.text', 'Decision recorded')
        checkSummary()
        // confirm project status has been updated
        cy.url().then(url => {
            const id = RecordDecision.getIdFromUrl(url)
            cy.visit(Cypress.env('url') + '/project-list')
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'APPROVED WITH CONDITIONS')
        })
    })

    // Displayed at the end of the journey
    function checkSummary() {
        cy.ApprovedDecisionPreview().should('contain.text', 'APPROVED WITH CONDITIONS')
        cy.ApprovedMadeByPreview().should('contain.text', 'Regional Director for the region')
        cy.AprrovedConditionsSet().should('contain.text', 'Yes')
        cy.AprrovedConditionsSet().should('contain.text', 'This is a test')
        cy.ApprovedDecisionDate().should('contain.text', '10 August 2022')

    }

})