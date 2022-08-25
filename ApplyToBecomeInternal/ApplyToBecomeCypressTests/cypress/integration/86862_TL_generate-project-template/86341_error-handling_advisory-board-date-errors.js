/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
	describe(`86341: Error messaging should be correct on ${viewport}`, () => {
		beforeEach(() => {
			cy.viewport(viewport)
			cy.login()
			cy.get('#school-name-0').click()
		})

		it('TC01: Should open first school in the list', () => {
			cy.get('*[href*="/confirm-school-trust-information-project-dates"]')
				.should('be.visible')
		});
		
		context("when form submitted", () => {
			beforeEach(() => {
				cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click()
				cy.get('*[data-test="change-advisory-board-date"]').click()
			})

			it('TC02: Should display "Advisory board date must be in the future" when an elapsed date has been submitted', () => {
				cy.url().then(href => {
					expect(href.endsWith('/confirm-school-trust-information-project-dates/advisory-board-date')).to.be.true;
				})
				cy.get('h1').contains('Set the advisory board date')
				cy.submitDateSchoolTrust(11, 11, 1980)
				cy.saveContinueBtn().click()
				cy.get('.govuk-error-summary__list li a')
					.should('have.text', 'Advisory board date must be in the future')
			});

			it('TC03: Should display "Advisory Board must be a valid date" when submitting invalid month', () => {
				cy.submitDateSchoolTrust(11, 222, 1980)
				cy.saveContinueBtn().click()
				cy.get('.govuk-error-summary__list li a')
					.should('have.text', 'Month must be between 1 and 12')
			});

			it('TC04: Should display "Advisory Board date must be a valid date" when submitting out-of-index month', () => {
				cy.submitDateSchoolTrust(11, 0, 1980)
				cy.saveContinueBtn().click()
				cy.get('.govuk-error-summary__list li a')
					.should('have.text', 'Month must be between 1 and 12')
			});
		})
	});
});