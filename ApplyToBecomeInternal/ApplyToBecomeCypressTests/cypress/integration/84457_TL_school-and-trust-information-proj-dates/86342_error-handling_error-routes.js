/// <reference types ='Cypress'/>

describe('86342 Error message link should redirect correctly', () => {
	afterEach(() => {
		cy.storeSessionData()
	});

	before(function () {
		cy.login()
		cy.selectSchoolListing(1)
	});

	it('TC01: Should click on error link and allow user to re-enter date', () => {
		cy.url().then(url => {
            let modifiedUrl = url + '/confirm-school-trust-information-project-dates'
            cy.visit(modifiedUrl)
        })
		cy.get('*[data-test="change-advisory-board-date"]').click()
		cy.submitDateSchoolTrust(11, 11, 1980)
		cy.saveContinueBtn().click()
		cy.get('.govuk-error-summary__list li a')
			.should('have.text', 'Advisory Board date must be in the future')
			.click()
		cy.submitDateSchoolTrust(1, 2, 2025)
		cy.saveContinueBtn().click()
		cy.confirmContinueBtn().click()
		cy.generateProjectTempBtn().click()
	});

	it('TC02: Should display report link for school when Generate Report link clicked', () => {
		cy.get('.app-c-attachment__link').should('be.visible')
	});
});

after(function () {
	cy.clearLocalStorage()
});
