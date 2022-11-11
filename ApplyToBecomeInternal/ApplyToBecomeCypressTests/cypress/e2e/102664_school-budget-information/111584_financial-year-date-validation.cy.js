/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
	describe(`111891: "Dates user sent/return the template" are reflected in preview on ${viewport}`, () => {
		beforeEach(() => {
		  cy.login();
		  cy.viewport(viewport);
		  cy.selectSchoolListing(2);
		  cy.url().then(url => {
		 	let modifiedUrl = url + '/confirm-school-budget-information'
			cy.visit(modifiedUrl);
		  });
		  cy.get('[data-test="change-financial-year"]').click();
		  cy.url().should('contain', '/update-school-budget-information');
		});

		it('TC01: Verifies that user cannot update the invalid values for End of current Financial year', () => {
			cy.submitEndOfCurrentFinancialYearDate(33, 3, 2020);
			cy.saveAndContinueButton().click();
			cy.get('[id="error-summary-title"]').should('be.visible');
			cy.get('[data-cy="error-summary"]')
			  .invoke('text')
			  .should('contain', 'Day must be between 1 and 31');
		});

		it('TC02: Verifies that user cannot update the invalid values for End of next Financial year', () => {
			cy.submitEndOfNextFinancialYearDate(20, 'jan', 2023);
			cy.saveAndContinueButton().click();
			cy.get('[id="error-summary-title"]').should('be.visible');
			cy.get('[data-cy="error-summary"]')
			  .invoke('text')
			  .should('contains', 'Enter a date in the correct format');
		});

		it('TC03: Verifies that user cannot save current year as greater than next financial year', () => {
			cy.submitEndOfCurrentFinancialYearDate(20, 3, 2022);
			cy.submitEndOfNextFinancialYearDate(20, 3, 2021);
			cy.saveAndContinueButton().click();
			cy.get('[id="error-summary-title"]').should('be.visible');
			cy.get('[data-cy="error-summary"]')
			  .invoke('text')
			  .should('contains', 'The next financial year cannot be before or within a year of the current financial year')
		});
	});
});
