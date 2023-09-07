/// <reference types ='Cypress'/>

// export const this.selectors = {
//     panLink: '[data-test="change-published-admission-number"]',
//     saveButton: '[id="save-and-continue-button"]'
// }

// export const path = 'school-overview';

class SchoolOverview {

    selectors = {
        panLink: '[data-test="change-published-admission-number"]',
        panInput: '[id="published-admission-number"]',
    }

    path = 'school-overview';

    getPan() {
        cy.checkPath(path);
        return cy.get('[id="published-admission-number"]');
    }

    changePanNumber(newPanNumber) {
        cy.checkPath(path);
        cy.get(this.selectors.panLink).click();
        cy.get(this.selectors.panInput).clear().type(newPanNumber);
        cy.saveContinue().click();
    }

    getViabilityIssues() {
        cy.checkPath(path);
        return cy.get('[id="viability-issues"]');
    }

    changeViabilityIssues(viabilityIssues) {
        cy.checkPath(path);
        cy.get('[data-test="change-viability-issues"]').click();
        if (viabilityIssues) {
            cy.get('[id="viability-issues", value="Yes"]').check();
        }
        else {
            cy.get('[id="viability-issues", value="No"]').check();
        }
        cy.saveContinue().click();
    }

    getFinancialDeficit() {
        cy.checkPath(path);
        return cy.get('[id="financial-deficit"]');
    }

    changeFinancialDeficit(financialDeficit) {
        cy.checkPath(path);
        cy.get('[data-test="change-financial-deficit"]').click();
        if (financialDeficit) {
            cy.get('[id="financial-deficit", value="Yes"]').check();
        }
        else {
            cy.get('[id="financial-deficit", value="No"]').check();
        }
        cy.saveContinue().click();
    }

    getPFI() {
        cy.checkPath(path);
        return cy.get('[id="part-of-pfi"]');
    }

    changePFI(pfi, description = '') {
        cy.checkPath(path);
        cy.get('[data-test="change-part-of-pfi"]').click();
        if (pfi) {
            cy.get('[id="financial-deficit", value="Yes"]').check();
        }
        else {
            cy.get('[id="financial-deficit", value="No"]').check();
        }
        cy.saveContinue().click();
    }
};

export default new SchoolOverview();
