/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
	describe(`86342 Error message link should redirect correctly on ${viewport}`, () => {
		beforeEach(() => {
			cy.viewport(viewport)
			cy.login()
			cy.selectSchoolListing(1)

			cy.url().then(url => {
				let modifiedUrl = url + '/confirm-school-trust-information-project-dates'
				cy.visit(modifiedUrl)
			})
			cy.get('*[data-test="change-advisory-board-date"]').click()
		})

		it('TC01: Should click on error link and allow user to re-enter date', () => {
			cy.viewport(viewport)

			cy.submitDateSchoolTrust(11, 11, 2005)
			cy.saveAndContinueButton().click()
			cy.get('.govuk-error-summary__list li a')
				.should('have.text', 'Advisory board date must be in the future')
				.click()
			cy.submitDateSchoolTrust(1, 2, 2025)
			cy.saveAndContinueButton().click()
		});

		it('TC02: Should display report link for school when Generate Report link clicked', () => {
			cy.submitDateSchoolTrust(1, 2, 2025)
			cy.saveAndContinueButton().click()
			cy.confirmContinueBtn().click()
			cy.generateProjectTempBtn().click()
			cy.get('.app-c-attachment__link').should('be.visible')
		});
	});
});