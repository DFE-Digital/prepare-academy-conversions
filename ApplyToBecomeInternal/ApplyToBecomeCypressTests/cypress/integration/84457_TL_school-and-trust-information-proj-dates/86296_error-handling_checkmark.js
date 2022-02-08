/// <reference types ="Cypress"/>

describe("86296 Check mark should reflect status correctly", () => {
    afterEach(() => {
        cy.storeSessionData(); 
    })
    before(function () {
        cy.login();
        cy.selectSchoolListing(1)
    });

    it("TC: Precondition checkbox status", () => {
        cy.get('[id=school-and-trust-information-status]').should("be.visible")
        .invoke("text")
        .then((text) => {
            // text == "Completed" || "In Progress"
            if (text.includes("Completed")) {
                cy.log("Completed")
                return
            }
            else {
                cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click();
                cy.get('[id="school-and-trust-information-complete"]').click();
                cy.get("#confirm-and-continue-button").click();
            }
        })
    })

    it("TC01: Unchecked and returns as 'In Progress", () => {
        cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click();
        cy.get('[id="school-and-trust-information-complete"]').click()
        cy.get("#confirm-and-continue-button").click();
        cy.get('[id=school-and-trust-information-status]').contains('In Progress').should('not.contain', 'Completed')
    })

    it("TC02: Checks and returns as 'Completed'", () => {
        cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click();
        cy.get('[id="school-and-trust-information-complete"]').click()
        cy.get("#confirm-and-continue-button").click();
        cy.get('[id=school-and-trust-information-status]').contains('Completed').should('not.contain', 'In Progress')
    });

    after(function () {
        cy.clearLocalStorage();
    });
});
