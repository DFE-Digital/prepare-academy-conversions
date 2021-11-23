describe('Submit and view project notes', () => {
    afterEach(() => {
		cy.storeSessionData();
	});

    before(function () {
        cy.login();
        cy.selectSchoolListing(2)
    });
    
    after(function () {
        cy.clearLocalStorage();
    });

    it('Should display the project notes input', () => {
        cy.get('[href*="/project-notes/"').click()
        cy.get('[href*="/new-note"').click()
    });

    it('Should allow the user to add a note', () => {
        let date = new Date();
        let dateText = date.toTimeString()
        cy.get('#project-note-subject').type('New subject added at: ' + dateText)
        cy.get('#project-note-body').type('New body added at: ' + dateText)
        cy.get('[type=submit]').click()
        cy.pause()
        cy.get('#project-note-added').should('contain.text', 'Note added')
        cy.get('#project-note-subject-0').should('have.text', 'New subject added at: '+dateText)
        cy.get('#project-note-body-0').should('have.text', 'New body added at: '+dateText)
    });

    it('Should correctly render the school application form', () => {
        
    });

    it('Should allow the user to navigate back to the task list with updates displayed', () => {
        
    });

    it
})
