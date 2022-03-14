/// <reference types ='Cypress'/>

describe('87641 Check mark should reflect status correctly on LA Information preview page', () =>{
    afterEach(() => {
        cy.storeSessionData()
    });

    before(() => {
        cy.login()
        cy.selectSchoolListing(2)
    });

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

    it('TC01: Unchecked and returns as "In Progress"', () => {
        cy.get('*[href*="/confirm-local-authority-information-template-dates"]').click()
        cy.completeStatusLaInfo().click()
        cy.confirmContinueBtn().click()
        cy.statusLaInfo()
        .contains('In Progress')
        .should('not.contain', 'Completed')
    });

    it('TC02: Checks and returns as "Completed"', () => {
        cy.get('*[href*="/confirm-local-authority-information-template-dates"]').click()
        cy.completeStatusLaInfo().click()
        cy.confirmContinueBtn().click()
        cy.statusLaInfo()
        .contains('Completed')
        .should('not.contain', 'In Progress')
    });

    after(() => {
        cy.clearLocalStorage()
    });
});