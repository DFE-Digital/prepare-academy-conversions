/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
    describe(`87641 Check mark should reflect status correctly on LA Information preview page on ${viewport}`, { tags: ['@dev', '@stage']}, () =>{
		beforeEach(() => {
			cy.login()
			cy.viewport(viewport)
			cy.selectSchoolListing(2)
		})
    
        it('TC: Precondition checkbox status', () => {
            cy.statusLaInfo().should('be.visible')
            .invoke('text')
            .then((text) => {
                if (text.includes('Complete')) {
                    return
                }
                else {
                    cy.uncheckLaInfo()
                };
            });
        });
		
		context("when form submitted", () => {
			beforeEach(() => {
				cy.get('*[href*="/confirm-local-authority-information-template-dates"]').click()
				cy.completeStatusLaInfo().click()
				cy.confirmContinueBtn().click()
			})
			
			it('TC01: Unchecked and returns as "In Progress"', () => {
				cy.statusLaInfo()
					.contains('In Progress')
					.should('not.contain', 'Completed')
			});

			it('TC02: Checks and returns as "Completed"', () => {
				cy.statusLaInfo()
					.contains('Completed')
					.should('not.contain', 'In Progress')
			});
		})
    });
});