/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`87641 Check mark should reflect status correctly on LA Information preview page on ${viewport}`, () =>{
		beforeEach(() => {
			cy.login({titleFilter: 'Gloucester school'})
			cy.viewport(viewport)
			cy.selectSchoolListing(2)
		})

        it('TC01: Precondition checkbox status', () => {
            cy.statusLaInfo().should('be.visible')
            .invoke('text')
            .then((text) => {
                if (text.includes('Complete')) {
                    return
                }
                else {
                    cy.uncheckLaInfo()
                }
            });
        });

		context("when form submitted", () => {
			beforeEach(() => {
				cy.get('*[href*="/confirm-local-authority-information-template-dates"]').click()
				cy.completeStatusLaInfo().click()
				cy.confirmContinueBtn().click()
			})

			it('TC02: Unchecked and returns as "In Progress"', () => {
				cy.statusLaInfo()
					.contains('In Progress')
					.should('not.contain', 'Completed')
			});

			it('TC03: Checks and returns as "Completed"', () => {
				cy.statusLaInfo()
					.contains('Completed')
					.should('not.contain', 'In Progress')
			});
		})
    });
});