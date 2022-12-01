/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`84596 Error handling should present correctly to the user on ${viewport}`, () => {
        after(function () {
            cy.clearLocalStorage()
        });
    
        afterEach(() => {
            cy.storeSessionData()
        });
    
        beforeEach(() => {
            cy.login()
        });
		
		before(() => {
			cy.viewport(viewport)
		})

        // Place holder for accessbility tests, to unskip once issues is resolved
        it.skip('Accessbility', () => {
            let url = Cypress.env('url');
            cy.visit(url)
            cy.injectAxe()
            cy.checkA11y()

        })
    
         it('TC01: Should open first school in the list', () => {
            cy.viewport(viewport)
            cy.selectSchoolListing(0)
            cy.url().then(url => {
                let modifiedUrl = url + '/confirm-school-trust-information-project-dates'
                cy.visit(modifiedUrl)
            });
        });
        
        // Raised under 80466
        it.skip('TC02: Should display user-friendly error when incorrect return parameter passed [80466]', ()=>{
            cy.viewport(viewport)
            let modifiedUrl
            cy.get('[aria-describedby*=la-info-template-status]').click()
            cy.url().then(url =>{
                //Changes the current URL to:
                ///task-list/<SOME_VALID_ID>/confirm-local-authority-information-template-dates?return=someInvalideParam/SomeInvalidPath
                modifiedUrl = url.replace('%2FTaskList%2FIndex&backText=Back%20to%20task%20list','someInvalideParam')
                cy.visit(modifiedUrl+'/SomeInvalidPath')
            });
            cy.get('.govuk-button').click()
            cy.get('h1').should('not.contain.text','An unhandled exception occurred while processing the request.')
        });
        
        // Raised under 81652
        it('TC03: Should display user-friendly error when incorrect project ID requested', () => {
            cy.viewport(viewport)
            cy.visit(Cypress.env('url') +'/task-list/9999', {failOnStatusCode: false})
            cy.get('[id="error-heading"]').should('have.text','Page not found')
        });
        
        // Raised under 81655
        it('TC04: Should display user-friendly error when incorrect url requested', () => {
            cy.viewport(viewport)
            cy.visit(Cypress.env('url') +'/task-list-nonsense', {failOnStatusCode: false})
            cy.get('[id="error-heading"]').should('have.text','Page not found')
        });
    });
});