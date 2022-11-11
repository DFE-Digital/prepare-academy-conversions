/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
	describe(`111891: "School budget information dates are saved and reflected in preview on ${viewport}`, () => {
		beforeEach(() => {
			cy.login();
			cy.viewport(viewport);
			cy.selectSchoolListing(2);
			cy.url().then(url => {
				//changes the current URL
				let modifiedUrl = url + '/confirm-school-budget-information'
				cy.visit(modifiedUrl);
			});

			cy.get('[data-test="change-financial-year"]').click();
			cy.submitEndOfCurrentFinancialYearDate(21, 3, 2022);
			cy.submitEndOfNextFinancialYearDate(20, 3, 2023);
			cy.saveAndContinueButton().click();
		})

		it('TC01: Verifies that End of Current financial year dates matches in confirm school budget info page', () => {
			cy.endOfCurrentFinancialYearInfo()
			  .invoke('text')
			  .should('contain', '21 March 2022' )
		      .then((text) => {
				expect(text).to.match(/^([0-9]){2}\s[a-zA-Z]{1,}\s[0-9]{4}$/)
			});
		});

		it('TC02: Verifies that End of Current financial year dates matches in preview page', () => {
            cy.get('[id="school-budget-information-complete"]').click();
			cy.confirmContinueBtn().click();
            cy.get('[id="preview-project-template-button"]').click();
			cy.url().should('include', '/preview-project-template')
			cy.get('h1').contains('Preview project template')
			cy.get('h2').contains('School budget information')
			cy.get('[id="financial-year"]')
			  .invoke('text')
			  .should('contain', '21 March 2022' )
			  .then((text) => {
				expect(text).to.match(/^([0-9]){2}\s[a-zA-Z]{1,}\s[0-9]{4}$/)
			});
		});

		it('TC03: Verifies that End of next financial year dates matches in confirm school budget info page', () => {
			cy.endOfNextFinancialYearInfo()
			  .invoke('text')
			  .should('contain', '20 March 2023')
			  .then((text) => {
			    expect(text).to.match(/^([0-9]){2}\s[a-zA-Z]{1,}\s[0-9]{4}$/)
			});
		});

		it('TC04: Verifies that End of next financial year dates matches in preview page', () => {
            cy.get('[id="school-budget-information-complete"]').click();
			cy.confirmContinueBtn().click();
            cy.get('[id="preview-project-template-button"]').click();
			cy.url().should('include', 'preview-project-template')
			cy.get('h1').contains('Preview project template')
			cy.get('h2').contains('School budget information')
			cy.get('[id="next-financial-year"]')
			  .invoke('text')
			  .should('contain', '20 March 2023' )
			  .then((text) => {
			    expect(text).to.match(/^([0-9]){2}\s[a-zA-Z]{1,}\s[0-9]{4}$/)
			});
		});

        it('TC05: Verifies that preview page has correct context for School Information', () => {
			cy.get('[id="school-budget-information-complete"]').click();
			cy.confirmContinueBtn().click();
            cy.get('[id="preview-project-template-button"]').click();
			cy.url().should('include', '/preview-project-template');
			cy.get('h1').contains('Preview project template');
			cy.get('h2').contains('School budget information');
            cy.get('dl').contains('End of current financial year');
			cy.get('dl').contains('Forecasted revenue carry forward at the end of the current financial year');
			cy.get('dl').contains('Forecasted capital carry forward at the end of the current financial year');
			cy.get('dl').contains('End of next financial year');
			cy.get('dl').contains('Forecasted revenue carry forward at the end of the next financial year');
			cy.get('dl').contains('Forecasted capital carry forward at the end of the next financial year');
		    cy.get('dl').contains('Additional information');	
		});
	});
});