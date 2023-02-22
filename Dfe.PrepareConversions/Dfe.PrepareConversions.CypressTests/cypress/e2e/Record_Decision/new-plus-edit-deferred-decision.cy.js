/// <reference types ='Cypress'/>
// uri to be updated once academisation API is integrated
let projectList = Cypress.env('url') + '/project-list'
import ProjectList from '../../pages/projectList'

describe('Edit Deferred journey', {tags: '@dev'}, () => {
    beforeEach(() => {    
        ProjectList.selectProject().then(id => {
            // delete deferred reasons
            cy.sqlServer(`
            delete from 
                academisation.ConversionAdvisoryBoardDecisionDeferredReason 
            where 
                AdvisoryBoardDecisionId = (select Id from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = ${id})`) 
            cy.sqlServer(`delete from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = ${id}`)
            cy.sqlServer(`insert into academisation.ConversionAdvisoryBoardDecision values (${id}, \'Deferred\', null, null, getdate(), \'None\', getdate(), getdate())`)
            cy.clearCookies()
            cy.reload()
        })
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
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'Additional information needed:')
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'Additional info 2nd time')
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = ProjectList.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DEFERRED')
        })
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
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'Awaiting next ofsted report:')
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'awaiting ofsted 2nd time')
        // check date
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = ProjectList.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DEFERRED')
        })
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
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'Performance concerns:')
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'performance details 2nd time')
        // check date
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = ProjectList.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DEFERRED')
        })
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
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'Other:')
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'other details 2nd time')
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = ProjectList.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DEFERRED')
        })
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
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'Other:')
        cy.get('[id="deferred-reasons"]')
          .should('contain.text', 'other details 2nd time')
        cy.deferredDecisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = ProjectList.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DEFERRED')
        })
    })
})
