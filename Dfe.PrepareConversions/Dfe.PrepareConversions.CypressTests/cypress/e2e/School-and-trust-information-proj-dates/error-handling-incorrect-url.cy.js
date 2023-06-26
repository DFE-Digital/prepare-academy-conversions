// /// <reference types ='Cypress'/>

// Cypress._.each(['ipad-mini'], (viewport) => {
//     describe(`84596 Error handling should present correctly to the user on ${viewport}`, () => {
//         const wcagStandards = [ "wcag22aa"];
//         const impactLevel = ["critical", "minor", "moderate", "serious"];
//         const continueOnFail = false;

//         after(function () {
//             cy.clearLocalStorage()
//         });

//         beforeEach(() => {
//             cy.login({titleFilter: 'Gloucester school'})
//         });

// 		before(() => {
// 			cy.viewport(viewport)
// 		})

//        //accessibility check
//        it('Accessbility check', () => {
//             let url = Cypress.env('url');
//             cy.visit(url)
//             cy.excuteAccessibilityTests(wcagStandards, continueOnFail, impactLevel)
//         });

//         it('TC01: Should open first school in the list', () => {
//             cy.viewport(viewport)
//             cy.selectSchoolListing(0)
//             cy.urlPath().then(url => {
//                 let modifiedUrl = url + '/confirm-school-trust-information-project-dates'
//                 cy.visit(modifiedUrl)
//             });
//         });

//         it.skip('TC02: Should display user-friendly error when incorrect return parameter passed [80466]', ()=>{
//             cy.viewport(viewport)
//             cy.selectSchoolListing(0)
//             let modifiedUrl
//             cy.get('[aria-describedby*=la-info-template-status]').click()
//             cy.urlPath().then(url =>{
//                 //Changes the current URL to:
//                 ///task-list/<SOME_VALID_ID>/confirm-local-authority-information-template-dates?return=someInvalideParam/SomeInvalidPath
//                 modifiedUrl = url.replace('%2FTaskList%2FIndex&backText=Back%20to%20task%20list','someInvalideParam')
//                 cy.visit(modifiedUrl+'/SomeInvalidPath', {failOnStatusCode: false});
//             });
//             cy.get('.govuk-button').click()
//             cy.get('h1').should('not.contain.text','An unhandled exception occurred while processing the request.')
//         });

//         it('TC03: Should display user-friendly error when incorrect project ID requested', () => {
//             cy.viewport(viewport)
//             cy.visit(Cypress.env('url') +'/task-list/99990', {failOnStatusCode: false})
//             cy.get('[id="error-heading"]').should('have.text','Page not found')
//         });

//         it('TC04: Should display user-friendly error when incorrect url requested', () => {
//             cy.viewport(viewport)
//             cy.visit(Cypress.env('url') +'/task-list-nonsense', {failOnStatusCode: false})
//             cy.get('[id="error-heading"]').should('have.text','Page not found')
//         });
//     });
// });