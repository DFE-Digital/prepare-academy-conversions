/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
    describe(`86317 Submit and view project notes on ${viewport}`, () => {
        afterEach(() => {
            cy.storeSessionData();
        });
    
        before(function () {
            cy.viewport(viewport)
            cy.login()
            cy.selectSchoolListing(2)
        });
        
        after(function () {
            cy.clearLocalStorage()
        });
    
        let date
        let dateText
    
        it('TC01: Should display the project notes input', () => {
            cy.viewport(viewport)
            cy.get('[href*="/project-notes/"').click()
            cy.get('[href*="/new-note"').click()
        });
        
        it('TC02: Should allow the user to add a note' , () => {
            date = new Date();
            dateText = date.toTimeString()
            cy.viewport(viewport)
            cy.get('#project-note-subject').type('New subject added at: ' + dateText)
            cy.get('#project-note-body').type('New body added at: ' + dateText)
            cy.get('[type=submit]').click()
        });
        
        it('TC03: Should display the note after it is submitted', () => {
            cy.viewport(viewport)
            cy.get('#project-note-added').should('contain.text', 'Note added')
            cy.get('#project-note-subject-0').should('have.text', 'New subject added at: '+dateText)
            cy.get('#project-note-body-0').should('have.text', 'New body added at: '+dateText)
        });
    });
});