/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
	describe(`86342 Error message link should redirect correctly on ${viewport}`, () => {
		beforeEach(() => {
			cy.viewport(viewport)
			cy.login({titleFilter: 'Gloucester school'})
			cy.selectSchoolListing(1)

			cy.url().then(url => {
				let modifiedUrl = url + '/confirm-school-trust-information-project-dates'
				cy.visit(modifiedUrl)
			})
			cy.get('*[data-test="change-advisory-board-date"]').click()
		})

		it('TC01: Should click on error link and allow user to re-enter date after invalid date', () => {
			cy.viewport(viewport)

			cy.submitDateSchoolTrust(31, 12, 1999)
			cy.saveAndContinueButton().click()
			cy.get('.govuk-error-summary__list li a')
				.should('have.text', 'Year must be between 2000 and 2050')
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