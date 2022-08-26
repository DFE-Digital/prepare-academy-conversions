/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
let url = Cypress.env('url') + '/task-list/2054?rd=true'
//let projecList = Cypress.env('url') + '/project-list'

describe('103195 Edit Approved record decision', ()=> {    
    beforeEach(() => {
        cy.ApproveDeleteAddNewRecord()
        cy.clearCookies()
        cy.visit(url)
    })
    
    // Edit Approval Path - Regional Director, No/Yes conditions set 
    it('TC01: J1 Edit a recorded decision Approval - Reg Director Region, No Conditions', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
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
        cy.continueBtn().click()
        cy.YesTextBox().clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.ApprovedDecisionPreview().should('contain.text', 'APPROVED WITH CONDITIONS')
        cy.ApprovedMadeByPreview().should('contain.text', 'Regional Director for the region')
        cy.AprrovedConditionsSet().should('contain.text', 'Yes')
        cy.ApprovedConditionDetails().should('contain.text', 'This is a test')
        cy.ApprovedDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.ApprovedMessageBanner().should('contain.text', 'Decision recorded')
    })

    // Edit Approval Path - A different Regional Director, No/Yes conditions set 
    it('TC02: J1 Edit a recorded decision Approval - Different Reg Director, No Conditions', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="otherregionaldirector-radio"]').click()
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
        cy.continueBtn().click()
        cy.YesTextBox().clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.ApprovedDecisionPreview().should('contain.text', 'APPROVED WITH CONDITIONS')
        cy.ApprovedMadeByPreview().should('contain.text', 'A different Regional Director')
        cy.AprrovedConditionsSet().should('contain.text', 'Yes')
        cy.ApprovedConditionDetails().should('contain.text', 'This is a test')
        cy.ApprovedDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.ApprovedMessageBanner().should('contain.text', 'Decision recorded')
    })

    // Edit Approval Path - Minister, No/Yes conditions set 
    it('TC03: J1 Edit a recorded decision Approval - Minister, No Conditions', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
         //select iniital decision
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="minister-radio"]').click()
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
        cy.continueBtn().click()
        cy.YesTextBox().clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.ApprovedDecisionPreview().should('contain.text', 'APPROVED WITH CONDITIONS')
        cy.ApprovedMadeByPreview().should('contain.text', 'Minister')
        cy.AprrovedConditionsSet().should('contain.text', 'Yes')
        cy.ApprovedConditionDetails().should('contain.text', 'This is a test')
        cy.ApprovedDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.ApprovedMessageBanner().should('contain.text', 'Decision recorded')
    })
    // Edit Approval Path - Director General, No/Yes conditions set
    it('TC04: J1 Edit a recorded decision Approval - Director General, No Conditions', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="directorgeneral-radio"]').click()
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
        cy.continueBtn().click()
        cy.YesTextBox().clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.ApprovedDecisionPreview().should('contain.text', 'APPROVED WITH CONDITIONS')
        cy.ApprovedMadeByPreview().should('contain.text', 'Director General')
        cy.AprrovedConditionsSet().should('contain.text', 'Yes')
        cy.ApprovedConditionDetails().should('contain.text', 'This is a test')
        cy.ApprovedDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.ApprovedMessageBanner().should('contain.text', 'Decision recorded')
    })

    // Edit Approval Path - None, No/Yes conditions set
    it('TC05: J1 Edit a recorded decision Approval - None, No Conditions', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="none-radio"]').click()
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
        cy.continueBtn().click()
        cy.YesTextBox().clear().type('This is a test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.ApprovedDecisionPreview().should('contain.text', 'APPROVED WITH CONDITIONS')
        cy.ApprovedMadeByPreview().should('contain.text', 'None')
        cy.AprrovedConditionsSet().should('contain.text', 'Yes')
        cy.ApprovedConditionDetails().should('contain.text', 'This is a test')
        cy.ApprovedDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.ApprovedMessageBanner().should('contain.text', 'Decision recorded')
    })
})