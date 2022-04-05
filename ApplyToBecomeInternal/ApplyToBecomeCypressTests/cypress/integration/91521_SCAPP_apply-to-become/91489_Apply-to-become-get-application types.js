/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
    describe(`91489: Apply-to-become GET application types on ${viewport}`, () => {
        let url = Cypress.env('url')
    
        afterEach(() => {
            cy.storeSessionData()
        });
        
        before(function () {
            cy.viewport(viewport)
            cy.login()
            cy.get('[id="school-name-46"]').click()
        });
    
        after(() => {
            cy.clearLocalStorage()
        });
    
        it('TC01: Error Message Page not found if no existing ID', () => {
            cy.viewport(viewport)
            cy.visit(url+'/school-application-form/666', {failOnStatusCode: false})
            cy.get('[id="error-heading"]').should('contain.text', 'Page not found')
        });
    
    
        it('TC02: FormMAT type should not be valid and should display "Not Implemented"', () => {
            cy.viewport(viewport)
            cy.visit( url+'/school-application-form/521', {failOnStatusCode: false})
            cy.get('[id="error-heading"]').should('contain.text', 'Not implemented')
        });
    });
});