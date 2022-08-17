/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
let url = Cypress.env('url') + '/task-list/2050?rd=true'

describe('103195 Approved record decision', ()=> {
    
    beforeEach(() => {
        cy.visit(url)
        cy.clearCookies()

    })
    // Approve Happy Path journey
    // *** A new project is need to run tests - skipping for now
    it.skip('TC01: J1 - Navigate to project to record a decision', () => {
        cy.get('[id="record-decision-link"]').click()
        cy.get('[id="approved-radio"]').click()
    })

    // Edit Approval Path - Regional Director, No/Yes conditions set 
    it('TC02: J1 Edit a recorded decision Approval - Reg Director Region, No Conditions', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.get('[id="approved-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects 'no' on conditions met
        cy.get('[id="no-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 10, 2021)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.get('[id="change-conditions-set-btn"]').click()
        cy.get('[id="yes-radio"]').click()
        cy.continueBtn().click()
        cy.get('[id="conditions-textarea"]').clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
    })

    // Edit Approval Path - A different Regional Director, No/Yes conditions set 
    it('TC03: J1 Edit a recorded decision Approval - Different Reg Director, No Conditions', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.get('[id="approved-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="otherregionaldirector-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects 'no' on conditions met
        cy.get('[id="no-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 10, 2021)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.get('[id="change-conditions-set-btn"]').click()
        cy.get('[id="yes-radio"]').click()
        cy.continueBtn().click()
        cy.get('[id="conditions-textarea"]').clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // clicks on the record a decision button to submit 
        cy.recordThisDecision().click()
    })
    
    // Edit Approval Path - Director General, No/Yes conditions set
    it('TC04: J1 Edit a recorded decision Approval - Director General, No Conditions', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.get('[id="approved-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="directorgeneral-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects 'no' on conditions met
        cy.get('[id="no-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 10, 2021)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.get('[id="change-conditions-set-btn"]').click()
        cy.get('[id="yes-radio"]').click()
        cy.continueBtn().click()
        cy.get('[id="conditions-textarea"]').clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // clicks on the record a decision button to submit 
        cy.recordThisDecision().click()
    })

    // Edit Approval Path - Minister, No/Yes conditions set 
    it('TC05: J1 Edit a recorded decision Approval - Minister, No Conditions', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.get('[id="approved-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="minister-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects 'no' on conditions met
        cy.get('[id="no-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 10, 2021)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.get('[id="change-conditions-set-btn"]').click()
        cy.get('[id="yes-radio"]').click()
        cy.continueBtn().click()
        cy.get('[id="conditions-textarea"]').clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // clicks on the record a decision button to submit 
        cy.recordThisDecision().click()
    })

    // Edit Approval Path - None, No/Yes conditions set
    it('TC06: J1 Edit a recorded decision Approval - None, No Conditions', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.get('[id="approved-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="none-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects 'no' on conditions met
        cy.get('[id="no-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 10, 2021)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.get('[id="change-conditions-set-btn"]').click()
        cy.get('[id="yes-radio"]').click()
        cy.continueBtn().click()
        cy.get('[id="conditions-textarea"]').clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // clicks on the record a decision button to submit 
        cy.recordThisDecision().click()
    })

})