/// <reference types ="Cypress"/>

describe("87641 Check mark should reflect status correctly on LA Information preview page", () =>{
    afterEach(() => {
        cy.storeSessionData()
    });

    before(() => {
        cy.login()
        cy.selectSchoolListing(2)
    });

    it("TC: Precondition checkbox status", () => {
        cy.get('[id="la-info-template-status"]').should("be.visible")
        .invoke("text")
        .then((text) => {
            if (text.includes("Completed")) {
                return
            }
            else {
                cy.get('*[href*="/confirm-local-authority-information-template-dates"]').click()
                cy.get('[id="la-info-template-complete"]').click()
                cy.get('[data-module="govuk-button"]').click()
            };
        });
    });

    it("TC01: Unchecked and returns as 'In Progress", () => {
        cy.get('*[href*="/confirm-local-authority-information-template-dates"]').click()
        cy.get('[id="la-info-template-complete"]').click()
        cy.get('[data-module="govuk-button"]').click()
        cy.get('[id="la-info-template-status"]').contains('In Progress').should('not.contain', 'Completed')
    });

    it("TC02: Checks and returns as 'Completed", () => {
        cy.get('*[href*="/confirm-local-authority-information-template-dates"]').click()
        cy.get('[id="la-info-template-complete"]').click()
        cy.get('[data-module="govuk-button"]').click()
        cy.get('[id="la-info-template-status"]').contains('Completed').should('not.contain', 'In Progress')
    });

    after(() => {
        cy.clearLocalStorage()
    });
});