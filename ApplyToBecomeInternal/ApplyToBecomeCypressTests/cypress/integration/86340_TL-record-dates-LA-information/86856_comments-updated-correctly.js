/// <reference types ="Cypress"/>

describe("86856 Comments should accept alphanumeric inputs", () => {
    afterEach(() => {
        cy.storeSessionData()
    });

    before(() => {
        cy.login()
        cy.selectSchoolListing(2)
        cy.url().then(url => {
            //changes the current URL
            let modifiedUrl = url + "/confirm-local-authority-information-template-dates"
            cy.visit(modifiedUrl)
        });
    });

    after(() => {
        cy.clearLocalStorage();
    });

    it("TC: Precondition comment box", () => {
        cy.get('[id="la-info-template-comments"]').should("be.visible")
        .invoke("text")
        .then((text) => {
            if (text.includes("Empty")) {
                return
            }
            else {
                cy.get('[data-test="change-la-info-template-comments"]').click()
                cy.get('[id="la-info-template-comments"]').clear()
                cy.get('[data-module="govuk-button"]').click()
            };
        });
    });

    it("TC01: Navigates to comment section & type alphanumerical characters", () => {
        let alphanumeric = 'abcdefghijklmnopqrstuvwxyz 1234567890 !"£$%^&*(){}[]:@,./<>?~|'
        cy.get('[data-test="change-la-info-template-comments"]').click()
        cy.get('[id="la-info-template-comments"]').type(alphanumeric)
        cy.get('[data-module="govuk-button"]').click()
        cy.get('[id="la-info-template-comments"]').should('contain', alphanumeric)
    });

    it('TC02: Clears text input', () => {
        cy.get('[data-test="change-la-info-template-comments"]').click()
        cy.get('[id="la-info-template-comments"]').clear()
        cy.get('[data-module="govuk-button"]').click()
        cy.get('[id="la-info-template-comments"]').should('contain', 'Empty')
    });
});