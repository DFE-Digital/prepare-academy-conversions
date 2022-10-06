/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
    describe(`86296 Check mark should reflect status correctly on ${viewport}`, { tags: ['@dev', '@stage']}, () => {
		beforeEach(() => {
			cy.login()
			cy.selectSchoolListing(1)
		})

        before(function () {
            cy.viewport(viewport)
        });
    
        it('TC: Precondition checkbox status', () => {
            cy.viewport(viewport)
            cy.statusSchoolTrust().should('be.visible')
            .invoke('text')
            .then((text) => {
                if (text.includes('Completed')) {
                    return
                }
                else {
                    cy.uncheckSchoolTrust()
                };
            });
        });
        
        it('TC01: Unchecked and returns as "In Progress"', () => {
            cy.viewport(viewport)
            cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click()
            cy.completeStatusSchoolTrust().click()
            cy.confirmContinueBtn().click()
            cy.statusSchoolTrust().contains('In Progress').should('not.contain', 'Completed')
        });
        
        it('TC02: Checks and returns as "Completed"', () => {
            cy.viewport(viewport)
            cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click()
            cy.completeStatusSchoolTrust().click()
            cy.confirmContinueBtn().click()
            cy.statusSchoolTrust().contains('Completed').should('not.contain', 'In Progress')
        });
        
        after(function () {
            cy.clearLocalStorage()
        });
    });
});