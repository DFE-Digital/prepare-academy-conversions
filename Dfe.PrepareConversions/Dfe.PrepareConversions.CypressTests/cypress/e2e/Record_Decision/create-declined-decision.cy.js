/// <reference types ='Cypress'/>
import ProjectList from '../../pages/projectList'
// uri to be updated once academisation API is integrated

describe('103791 Create Declined journey', { tags: '@dev'}, () => {
    beforeEach(() => {
        ProjectList.selectProject().then(id => {
            // delete decision
            cy.sqlServer(`DELETE FROM [academisation].[ConversionAdvisoryBoardDecision] WHERE ConversionProjectId = ${id}`)
            cy.clearCookies()
            cy.reload()
        })
    })

    // Edit Approval Path - Regional Director, Finance 
    it('TC01: J2 Create a recorded decision Declined - Reg Director Region, Finance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Record a decision').click()
         //select iniital decision
        cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.declineFinancebox().click()
            .then(() => cy.declineFinancText().clear().type('finance details'))
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.declineFinancebox()
        .invoke('attr', 'aria-expanded')
        cy.declineFinancText().clear().type('finance details 2nd test')
        cy.continueBtn().click()
        cy.continueBtn().click()
        checkSummary()
        // clicks on the record a decision button to submit
        cy.continueBtn().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        checkSummary()
        // check project status has been updated
        cy.url().then(url => {
            const id = ProjectList.getIdFromUrl(url)
            cy.visit(Cypress.env('url') + '/project-list')
            cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DECLINED')
        })
    })

    function checkSummary(){
          // preview answers before submit
          cy.decision().should('contain.text', 'Declined')
          cy.decisionMadeBy().should('contain.text', 'Regional Director for the region')        
          // check reasons
          cy.get('[id="decline-reasons"]')
            .should('contain.text', 'Finance:')
          cy.get('[id="decline-reasons"]')
            .should('contain.text', 'finance details 2nd test')
          // check date                
          cy.decisionDate().should('contain.text', '10 August 2022')
    }
})