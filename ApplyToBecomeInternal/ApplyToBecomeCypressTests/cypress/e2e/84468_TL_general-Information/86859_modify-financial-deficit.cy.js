/// <reference types ='Cypress'/>
Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`86859 Modify viability fields on ${viewport}`, () => {
        afterEach(() => {
            cy.storeSessionData()
        });

		beforeEach(() => {
			cy.login()
			cy.selectSchoolListing(2)
			cy.url().then(url => {
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

        it('TC01: Navigates to Financial deficit fields and modifies fields "Yes"', () => {
            cy.viewport(viewport)
            cy.get('[data-test="change-financial-deficit"]').click()
            cy.get('[id="financial-deficit"]').click()
            cy.saveAndContinueButton().click()
            cy.get('[id="financial-deficit"]')
            .should('contain', 'Yes')
            .should('not.contain', 'No')
            .should('not.contain', 'Empty')
        });

        it('TC02: Navigates to Financial deficit fields and modifies fields "No"', () => {
            cy.viewport(viewport)
            cy.get('[data-test="change-financial-deficit"]').click()
            cy.get('[id="financial-deficit-2"]').click()
            cy.saveAndContinueButton().click()
            cy.get('[id="financial-deficit"]')
            .should('contain', 'No')
            .should('not.contain', 'Yes')
            .should('not.contain', 'Empty')
        });
    });
});