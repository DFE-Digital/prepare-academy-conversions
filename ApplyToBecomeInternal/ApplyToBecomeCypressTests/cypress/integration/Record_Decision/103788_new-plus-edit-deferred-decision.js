/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
let url = Cypress.env('url') + '/task-list/2006?rd=true'
let projecList = Cypress.env('url') + '/project-list'

describe('Edit Deferred journey', () => {
    beforeEach(() => {    
        // delete deferred reasons
        cy.sqlServer(`
                    delete from 
                        academisation.ConversionAdvisoryBoardDecisionDeferredReason 
                    where 
                        AdvisoryBoardDecisionId = (select Id from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = 2006)`) 
        cy.sqlServer('delete from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = 2006')
        cy.sqlServer('insert into academisation.ConversionAdvisoryBoardDecision values (2006, \'Deferred\', null, null, getdate(), \'None\', getdate(), getdate())')
        cy.clearCookies()
        cy.visit(url)
    })

    // Edit Deferred Path - Regional Director, Additional information needed 
    it('TC01: J3 Edit a recorded decision Deferred - Reg Director Region, Finance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
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
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DEFERRED')
    })

    // Edit Deferred Path - A different Regional Director, ofsted report 
    it('TC02: J3 Edit a recorded decision Deferred - Different Reg Director, ofsted report', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.deferredRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="otherregionaldirector-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.awaitOfstedReportBox().click()
            .then(() => cy.awaitOfstedReportText().clear().type('awaiting ofsted'))

        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.deferredReasonChangeLink().click()
        cy.awaitOfstedReportText().clear().type('awaiting ofsted 2nd time')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.deferredDecision().should('contain.text', 'Deferred')
        cy.deferredDecisionMadeBy().should('contain.text', 'A different Regional Director')
        // check reasons
        var deferredReasons = cy.get('[id="deferred-reasons"]')
        deferredReasons.should('contain.text', 'Awaiting next ofsted report:')
        deferredReasons.should('contain.text', 'awaiting ofsted 2nd time')
        // check date
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DEFERRED')
    })

    // Edit Deferred Path - Director General, Performance concerns
    it('TC03: J3 Edit a recorded decision Deferred - Director General, Performance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.deferredRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="directorgeneral-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.performanceCheckBox().click()
            .then(() => cy.performanceCheckText().clear().type('performance details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.deferredReasonChangeLink().click()        
        cy.performanceCheckText().clear().type('performance details 2nd time')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.deferredDecision().should('contain.text', 'Deferred')
        cy.deferredDecisionMadeBy().should('contain.text', 'Director General')
        // check reasons
        var deferredReasons = cy.get('[id="deferred-reasons"]')
        deferredReasons.should('contain.text', 'Performance concerns:')
        deferredReasons.should('contain.text', 'performance details 2nd time')
        // check date
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DEFERRED')
    })

    // Edit Deferred Path - Minister, Other
    it('TC04: J3 Edit a recorded decision Deferred - Minister, Other', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.deferredRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="minister-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.OtherCheckBox().click()            
            .then(() => cy.OtherCheckText().clear().type('other details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.deferredReasonChangeLink().click()
        cy.OtherCheckText().clear().type('other details 2nd time')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.deferredDecision().should('contain.text', 'Deferred')
        cy.deferredDecisionMadeBy().should('contain.text', 'Minister')
        // check reasons
        var deferredReasons = cy.get('[id="deferred-reasons"]')
        deferredReasons.should('contain.text', 'Other:')
        deferredReasons.should('contain.text', 'other details 2nd time')

        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DEFERRED')
    })

    // Edit Deferred Path - None, Other
    it('TC05: J3 Edit a recorded decision Deferred - None, Other', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.deferredRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="none-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.OtherCheckBox().click()
            .then(() => cy.OtherCheckText().clear().type('other details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.deferredReasonChangeLink().click()
        cy.OtherCheckText().clear().type('other details 2nd time')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.deferredDecision().should('contain.text', 'Deferred')
        cy.deferredDecisionMadeBy().should('contain.text', 'None')
         // check reasons
         var deferredReasons = cy.get('[id="deferred-reasons"]')
         deferredReasons.should('contain.text', 'Other:')
         deferredReasons.should('contain.text', 'other details 2nd time')
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DEFERRED')
    })
})