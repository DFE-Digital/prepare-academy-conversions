/// <reference types ='Cypress'/>

describe('91489: Apply-to-become GET application types', () => {
    afterEach(() => {
        cy.storeSessionData()
    });
    
    before(function () {
        cy.login()
        cy.get('[id="school-name-46"]').click()
    });

    after(() => {
        cy.clearLocalStorage()
    });

    it('TC01: Error Message Page not found if no existing ID', () => {
        cy.visit(Cypress.env('url') + '/school-application-form/666', {failOnStatusCode: false})
        cy.get('[id="error-heading"]').should('contain.text', 'Page not found')
    });


    it('TC02: FormMAT type should not be valid and should display "Not Implemented"', () => {
        cy.visit(Cypress.env('url') + '/school-application-form/521', {failOnStatusCode: false})
        cy.get('[id="error-heading"]').should('contain.text', 'Not implemented')
    })
})