/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
	describe(`86341: Error messaging should be correct on ${viewport}`, () => {
		beforeEach(() => {
			cy.viewport(viewport)
			cy.login({titleFilter: 'Gloucester school'})
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

			it('TC03: Should display "Advisory Board must be a valid date" when submitting invalid month', () => {
				cy.submitDateSchoolTrust(11, 222, 2005)
				cy.saveAndContinueButton().click()
				cy.get('.govuk-error-summary__list li a')
					.should('have.text', 'Month must be between 1 and 12')
			});

			it('TC04: Should display "Advisory Board date must be a valid date" when submitting out-of-index month', () => {
				cy.submitDateSchoolTrust(11, 0, 2005)
				cy.saveAndContinueButton().click()
				cy.get('.govuk-error-summary__list li a')
					.should('have.text', 'Month must be between 1 and 12')
			});
		})
	});
});