/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
	describe(`112379: Footer Links on ${viewport}`, () => {
		beforeEach(() => {
			cy.login();
			cy.viewport(viewport);
	    });

        it('TC01: should navigate to Get Support email link', () => {
          cy.get('h2').should('contain', 'Get support');
          cy.get('li').should('contain', 'Email');
          cy.get('[data-cy="get-support-email"]')
            .should('contain', 'regionalservices.rg@education.gov.uk');
        });

        it('TC01: should navigate to Give Feedback link', () => {
          cy.get('h2').should('contain', 'Give feedback');
          cy.get('li').should('contain', 'Email');
          cy.get('#footer-feedback-link')
            .should('contain', 'Give feedback on Prepare conversions and transfers (opens in a new tab)');
        });

        it('TC01: should not have unwanted content', () => {
          cy.get('h2').should('contain', 'Get support');
          cy.get('li').should('contain', 'Email');
          cy.get('[data-cy="get-support-section"]')
            .should('not.have.text', "You'll get a response in 2 working days");
        });
    });    
});
