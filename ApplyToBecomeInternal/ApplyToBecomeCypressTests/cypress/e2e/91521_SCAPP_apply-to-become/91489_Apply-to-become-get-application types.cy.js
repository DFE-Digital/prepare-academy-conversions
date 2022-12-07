/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
	describe(`91489: Apply-to-become GET application types on ${viewport}`, () => {
		beforeEach(() => {
			cy.login()

			cy.viewport(viewport)
			cy.get('[id="school-name-0"]').click()
		})

		it('TC01: Error Message Page not found if no existing ID', () => {
			cy.visit(Cypress.env('url') + '/school-application-form/666', {failOnStatusCode: false})
			cy.get('[id="error-heading"]').should('contain.text', 'Page not found')
		});

		it('TC02: FormMAT type should not be valid and should display "Page not found"', () => {
			cy.visit(Cypress.env('url') + '/school-application-form/521', {failOnStatusCode: false})
			cy.get('[id="error-heading"]').should('contain.text', 'Page not found')
		});
	});
});