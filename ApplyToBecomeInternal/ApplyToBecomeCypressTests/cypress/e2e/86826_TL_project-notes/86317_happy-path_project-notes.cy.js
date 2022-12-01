/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`86317 Submit and view project notes on ${viewport}`, () => {
		beforeEach(() => {
			cy.login()
			cy.viewport(viewport)
			cy.selectSchoolListing(2)
		})
    
        let date
        let dateText
    
        it('TC01: Should display the project notes input', () =>  {
            cy.get('[href*="/project-notes/"').click()
            cy.get('[href*="/new-note"').click()
        });
		
		context("when form filled out", () => {
			beforeEach(() => {
				cy.get('[href*="/project-notes/"').click()
				cy.get('[href*="/new-note"').click()
				date = new Date();
				dateText = date.toTimeString()
				cy.get('#project-note-subject').type('New subject added at: ' + dateText)
				cy.get('#project-note-body').type('New body added at: ' + dateText)
			})

			it('TC02: Should allow the user to add a note' , () => {
				cy.get('[type=submit]').click()
			});

			it('TC03: Should display the note after it is submitted', () => {
				cy.get('[type=submit]').click()
				cy.get('#project-note-added').should('contain.text', 'Note added')
				cy.get('#project-note-subject-0').should('have.text', 'New subject added at: '+dateText)
				cy.get('#project-note-body-0').should('have.text', 'New body added at: '+dateText)
			});
		})
    });
});