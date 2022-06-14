/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
    describe(`86858 Modify viability fields on ${viewport}`, () => {
        afterEach(() => {
            cy.storeSessionData()
        });

		beforeEach(() => {
			cy.login()
			cy.selectSchoolListing(2)
			cy.url().then(url =>{
				//Changes the current URL
				let modifiedUrl = url + '/confirm-general-information'
				cy.visit(modifiedUrl)
			});
		})
    
        before(() => {
            cy.viewport(viewport)
         });
    
        after(() => {
            cy.clearLocalStorage()
        });
    
        it('TC01: Navigates to Viability fields and modifies fields "Yes"', () => {
            cy.viewport(viewport)
            cy.get('[data-test="change-viability-issues"]').click()
             cy.get('[id="viability-issues"]').click()
            cy.saveContinueBtn().click()
            cy.get('[id="viability-issues"]').should('contain', 'Yes')
            .should('not.contain', 'No')
            .should('not.contain', 'Empty')
        });
    
        it('TC02: Navigates to Viability fields and modifies fields "No"', () => {
            cy.viewport(viewport)
            cy.get('[data-test="change-viability-issues"]').click()
            cy.get('[id="viability-issues-2"]').click()
            cy.saveContinueBtn().click()
            cy.get('[id="viability-issues"]').should('contain', 'No')
            .should('not.contain', 'Yes')
            .should('not.contain', 'Empty')
        });
    });
});