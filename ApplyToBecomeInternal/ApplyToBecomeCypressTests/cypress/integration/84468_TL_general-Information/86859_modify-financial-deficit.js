/// <reference types ='Cypress'/>

describe('86859 Modify viability fields', () => {
    afterEach(() => {
        cy.storeSessionData()
    });

    before(() => {
        cy.login()
        cy.selectSchoolListing(2)
        cy.url().then(url => {
            let modifiedUrl = url + '/confirm-general-information'
            cy.visit(modifiedUrl)
        });
    });

    after(() => {
        cy.clearLocalStorage()
    });

    it('TC01: Navigates to Financial deficit fields and modifies fields "Yes"', () => {
        cy.get('[data-test="change-financial-deficit"]').click()
        cy.get('[id="financial-deficit"]').click()
        cy.saveContinueBtn().click()
        cy.get('[id="financial-deficit"]')
        .should('contain', 'Yes')
        .should('not.contain', 'No')
        .should('not.contain', 'Empty')
    });

    it('TC02: Navigates to Financial deficit fields and modifies fields "No"', () => {
        cy.get('[data-test="change-financial-deficit"]').click()
        cy.get('[id="financial-deficit-2"]').click()
        cy.saveContinueBtn().click()
        cy.get('[id="financial-deficit"]')
        .should('contain', 'No')
        .should('not.contain', 'Yes')
        .should('not.contain', 'Empty')
    });

});