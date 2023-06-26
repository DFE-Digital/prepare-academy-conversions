// /// <reference types ='Cypress'/>
// import ProjectList from '../../pages/projectList'
// // uri to be updated once academisation API is integrated

// describe('Create Deferred journey', { tags: '@dev'}, () => {
//     beforeEach(() => {
//         ProjectList.selectProject().then(id => {
//             // delete decision
//             cy.sqlServer(`DELETE FROM [academisation].[ConversionAdvisoryBoardDecision] WHERE ConversionProjectId = ${id}`)
//             cy.clearCookies()
//             cy.reload()
//         })
//     })

//     // Edit Deferred Path - Regional Director, Additional information needed 
//     it('TC01: J3 Create a recorded decision Deferred - Reg Director Region, Finance', () => {
//         // Click on change your decision button 
//         cy.changeDecision().should('contain.text', 'Record a decision').click()
//         //select iniital decision
//         cy.deferredRadioBtn().click()
//         // clicks on the continue button
//         cy.continueBtn().click()
//         cy.get('[id="regionaldirectorforregion-radio"]').click()
//         // clicks on the continue button
//         cy.continueBtn().click()
//         cy.addInfoNeededBox().click()
//         cy.addInfoNeededBox().clear()
//         cy.addInfoNeededBox().type('Additional info')
//         // clicks on the continue button
//         cy.continueBtn().click()
//         // date entry
//         cy.recordDecisionDate(10, 8, 2022)
//         // clicks on the continue button
//         cy.continueBtn().click()
//         // give deferred reason
//         cy.deferredReasonChangeLink().click()
//         // set details
//         cy.addInfoNeededText().clear()
//         cy.addInfoNeededText().type('Additional info 2nd time')
//         cy.continueBtn().click()
//         cy.continueBtn().click()
//         checkSummary()
//         // clicks on the record a decision button to submit
//         cy.continueBtn().click()
//         // recorded decision confirmation        
//         cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
//         checkSummary()
//         // confirm project status has been updated
//         ProjectList.selectProject().then(id => {
//             cy.visit(Cypress.env('url') + '/project-list')
//             cy.get(`[id="project-status-${id}"]`).should('contain.text', 'DEFERRED')
//         })
//     })

//     function checkSummary() {
//         // preview answers 
//         cy.deferredDecision().should('contain.text', 'Deferred')
//         cy.deferredDecisionMadeBy().should('contain.text', 'Regional Director for the region')
//         // check reasons
//         cy.get('[id="deferred-reasons"]')
//           .should('contain.text', 'Additional information needed:');
//         cy.get('[id="deferred-reasons"]')
//           .should('contain.text', 'Additional info 2nd time');
//         cy.deferredDecisionDate().should('contain.text', '10 August 2022');
//     }
// })