/// <reference types ='Cypress'/>
import RecordDecision from '../../pages/recordDecision'
let projectList = Cypress.env('url') + '/project-list'

describe('103791 Edit Declined journey', () => {
    beforeEach(() => {
        RecordDecision.selectProject().then(id => {
            cy.sqlServer(`
                    delete from academisation.ConversionAdvisoryBoardDecisionDeclinedReason 
                    where AdvisoryBoardDecisionId = (select id from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = ${id})`);
            cy.sqlServer(`delete from academisation.ConversionAdvisoryBoardDecision where ConversionProjectId = ${id}`);
            cy.sqlServer(`insert into academisation.ConversionAdvisoryBoardDecision values (${id}, \'Declined\', null, null, getdate(), \'None\', getdate(), getdate())`);
            cy.clearCookies();
            cy.url().then(url => cy.visit(`${url}?rd=true`))
        })
    })

    // Edit Approval Path - Regional Director, Finance 
    it('TC01: J2 Edit a recorded decision Declined - Reg Director Region, Finance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.declineFinancebox().click()
            .then(() => cy.declineFinancText().clear().type('Finance details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.declineFinancText().clear().type('Finance details 2nd test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'Regional Director for the region')
        // check reasons
        var declinedReasons = cy.get('[id="decline-reasons"]')
        declinedReasons.should('contain.text', 'Finance:')
        declinedReasons.should('contain.text', 'Finance details 2nd test')
        // check date
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = RecordDecision.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DECLINED')
        })
    })

    // Edit Approval Path - A different Regional Director, Performance 
    it('TC02: J2 Edit a recorded decision Declined - Different Reg Director, Performance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="otherregionaldirector-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.performanceBox().click()
            .then(() => cy.performanceText().clear().type('Performance details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.performanceText().clear().type('Performance details 2nd test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'A different Regional Director')
        // check reasons
        var declinedReasons = cy.get('[id="decline-reasons"]')
        declinedReasons.should('contain.text', 'Performance:')
        declinedReasons.should('contain.text', 'Performance details 2nd test')
        // check date
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = RecordDecision.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DECLINED')
        })
    })

    // Edit Approval Path - Director General, Governance
    it('TC03: J2 Edit a recorded decision Declined - Director General, Governance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="directorgeneral-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.governanceBox().click()
            .then(() => cy.governanceText().clear().type('Governance details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.governanceText().clear().type('Governance details 2nd test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'Director General')
        // check reasons
        var declinedReasons = cy.get('[id="decline-reasons"]')
        declinedReasons.should('contain.text', 'Governance:')
        declinedReasons.should('contain.text', 'Governance details 2nd test')
        // check date
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = RecordDecision.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DECLINED')
        })
    })

    // Edit Approval Path - Minister, Choice of trust
    it('TC04: J2 Edit a recorded decision Declined - Minister, Choice of trust', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="minister-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.trustBox().click()
            .then(() => cy.trustText().clear().type('Trust details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.trustText().clear().type('Trust details 2nd test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'Minister')
        // check reasons
        var declinedReasons = cy.get('[id="decline-reasons"]')
        declinedReasons.should('contain.text', 'Choice of trust:')
        declinedReasons.should('contain.text', 'Trust details 2nd test')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = RecordDecision.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DECLINED')
        })
    })

    // Edit Approval Path - None, Other
    it('TC05: J2 Edit a recorded decision Declined - None, Other', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
        //select iniital decision
        cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="none-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.declineOtherbox().click()
            .then(() => cy.declineOthertxt().clear().type('Other details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.declineOthertxt().clear().type('Other details 2nd test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'None')
        // check reasons
        var declinedReasons = cy.get('[id="decline-reasons"]')
        declinedReasons.should('contain.text', 'Other:')
        declinedReasons.should('contain.text', 'Other details 2nd test')
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.url().then(url => {
            const id = RecordDecision.getIdFromUrl(url)
            cy.visit(projectList)
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DECLINED')
        })
    })
    
})