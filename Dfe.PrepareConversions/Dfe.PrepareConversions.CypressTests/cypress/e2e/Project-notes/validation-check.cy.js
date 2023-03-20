/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`107305: Vefify validation for project notes on ${viewport}`, () => {
		beforeEach(() => {
			cy.login({titleFilter: 'Gloucester school'});
			cy.viewport(viewport);
			cy.selectSchoolListing(2);
            cy.get('[href*="/project-notes/"').click();
            cy.get('[href*="/new-note"').click();
		})

        it('TC01: Should show validation message when saved without entering any values for Subject and Note', () =>  {
            cy.get('[type=submit]').click();
            cy.get('[data-cy="error-summary"]').should('contain.text', 'Enter a subject and project note');
        });

        it('TC02: should show validation message for Note when only Subject value is entered' , () => {
            cy.get('#project-note-subject').type('New subject: Test');
            cy.get('[type=submit]').click();
            cy.get('[data-cy="error-summary"]').should('contain.text', 'Enter a subject and project note');
        });

        it('TC03: should show validation message for Subject when only Note value is entered', () => {
            cy.get('#project-note-body').type('New body: Test');
            cy.get('[type=submit]').click();
            cy.get('[data-cy="error-summary"]').should('contain.text', 'Enter a subject and project note');
        });
    });
});
