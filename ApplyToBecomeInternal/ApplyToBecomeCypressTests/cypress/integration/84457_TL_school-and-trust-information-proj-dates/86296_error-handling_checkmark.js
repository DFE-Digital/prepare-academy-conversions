/// <reference types ='Cypress'/>

describe('86296 Check mark should reflect status correctly', () => {
    afterEach(() => {
        cy.storeSessionData()
    });
    before(function () {
        cy.login()
        cy.selectSchoolListing(1)
    });

    it('TC: Precondition checkbox status', () => {
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
        cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click()
        cy.completeStatusSchoolTrust().click()
        cy.confirmContinueBtn().click()
        cy.statusSchoolTrust().contains('In Progress').should('not.contain', 'Completed')
    });

    it('TC02: Checks and returns as "Completed"', () => {
        cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click();
        cy.completeStatusSchoolTrust().click()
        cy.confirmContinueBtn().click()
        cy.statusSchoolTrust().contains('Completed').should('not.contain', 'In Progress')
    });

    after(function () {
        cy.clearLocalStorage()
    });
});
