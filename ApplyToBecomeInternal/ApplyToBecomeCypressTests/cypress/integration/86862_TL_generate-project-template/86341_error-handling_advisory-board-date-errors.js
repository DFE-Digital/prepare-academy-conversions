/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
	describe(`86341: Error messaging should be correct on ${viewport}`, () => {
		afterEach(() => {
			cy.storeSessionData()
		});
	
		before(function () {
			cy.viewport(viewport)
			cy.login()
		});
		
		after(function () {
			cy.clearLocalStorage()
		});
		
		it('TC01: Should open first school in the list', () => {
			cy.viewport(viewport)
			cy.get('#school-name-0').click()
			cy.get('*[href*="/confirm-school-trust-information-project-dates"]')
			.should('be.visible')
			cy.saveLocalStorage()
		});
	
		it('TC02: Should display "Advisory Board date must be in the future" when an elapsed date has been submitted', () => {
			cy.viewport(viewport)
			cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click()
			cy.get('*[data-test="change-advisory-board-date"]').click()
			cy.url().then(href => {
				expect(href.endsWith('/confirm-school-trust-information-project-dates/advisory-board-date')).to.be.true;})
			cy.get('h1').contains('Set the Advisory Board date')
			cy.submitDateSchoolTrust(11, 11, 1980)
			cy.saveContinueBtn().click()
			cy.get('.govuk-error-summary__list li a')
			.should('have.text', 'Advisory Board date must be in the future')
		});
	
		it('TC03: Should display "Advisory Board date must be a real date" when submitting invalid month', () => {
			cy.viewport(viewport)
			cy.submitDateSchoolTrust(11, 222, 1980)
			cy.saveContinueBtn().click()
			cy.get('.govuk-error-summary__list li a')
			.should('have.text', 'Advisory Board date must be a real date')
		});
	
		it('TC04: Should display "Advisory Board date must be a real date" when submitting out-of-index month', () => {
			cy.viewport(viewport)
			cy.submitDateSchoolTrust(11, 0, 1980)
			cy.saveContinueBtn().click()
			cy.get('.govuk-error-summary__list li a')
			.should('have.text', 'Advisory Board date must be a real date')
		});
	});
});