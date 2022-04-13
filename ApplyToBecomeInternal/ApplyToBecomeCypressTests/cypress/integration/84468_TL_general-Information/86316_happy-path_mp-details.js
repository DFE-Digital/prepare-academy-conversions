/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
    describe(`86316 Submit and view MP details ${viewport}`, () => {
        afterEach(() => {
            cy.storeSessionData()
        });
    
        before(function () {
            cy.viewport(viewport)
            cy.login()
            cy.selectSchoolListing(2)
            cy.url().then(url =>{
                //Changes the current URL
                let modifiedUrl = url + '/confirm-general-information'
                cy.visit(modifiedUrl)
            });
         });
        
        after(function () {
            cy.clearLocalStorage()
        });
    
        
        it('TC01: Should navigate to MP details page', () => {
            cy.viewport(viewport)
            cy.get('[data-test="change-member-of-parliament-party"]').click()
            cy.url().then(href => {
                expect(href.endsWith('/confirm-general-information/enter-MP-name-and-political-party')).to.be.true;
            });
        });
    
        it('TC02: Should change the MP details', () => {
            cy.viewport(viewport)
            cy.mpName().clear().type('An MP')
            cy.mpParty().clear().type('A Party')
            cy.mpName().should('have.value', 'An MP')
            cy.mpParty().should('have.value', 'A Party')
        });
    
        it('TC03: Should go back to general information page on confirm', () => {
            cy.viewport(viewport)
            cy.saveContinueBtn().click()
            cy.url().then(href => {
                expect(href.endsWith('/confirm-general-information')).to.be.true});
        });
    
        it('TC04: Should display the MP details after it is submitted', () => {        
            cy.viewport(viewport)
            cy.mpName().should('have.text', 'An MP')
            cy.mpParty().should('have.text','A Party')
        });
    
        it('TC05: Should navigate to MP details page and remove details', () => {
            cy.viewport(viewport)
            cy.get('[data-test="change-member-of-parliament-party"]').click()
            cy.mpName().clear()
            cy.mpParty().clear()
            cy.saveContinueBtn().click()
            cy.mpName().should('have.text', 'Empty')
            cy.mpParty().should('have.text', 'Empty')
        });
    });
});