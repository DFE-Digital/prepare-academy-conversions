/// <reference types ='Cypress'/>

export default class SchoolOverview {

    static selectors = {
        panLink: '[data-test="change-published-admission-number"]',
        panInput: '[id="published-admission-number"]',
    }

    static path = 'school-overview';

    static getPan() {
        cy.checkPath(this.path);
        return cy.get('[id="published-admission-number"]');
    }

    static changePan(newPanNumber) {
        cy.checkPath(this.path);
        cy.get(this.selectors.panLink).click();
        cy.get(this.selectors.panInput).clear().type(newPanNumber);
        cy.saveContinue().click();
    }

    static getViabilityIssues() {
        cy.checkPath(this.path);
        return cy.get('[id="viability-issues"]');
    }

    static changeViabilityIssues(viabilityIssues) {
        cy.checkPath(this.path);
        cy.get('[data-test="change-viability-issues"]').click();
        if (viabilityIssues) {
            cy.YesRadioBtn().check();
        }
        else {
            cy.NoRadioBtn().check();
        }
        cy.saveContinue().click();
    }

    static getFinancialDeficit() {
        cy.checkPath(this.path);
        return cy.get('[id="financial-deficit"]');
    }

    static changeFinancialDeficit(financialDeficit) {
        cy.checkPath(this.path);
        cy.get('[data-test="change-financial-deficit"]').click();
        if (financialDeficit) {
            cy.YesRadioBtn().check();
        }
        else {
            cy.NoRadioBtn().check();
        }
        cy.saveContinue().click();
    }

    static getPFI() {
        cy.checkPath(this.path);
        return cy.get('[id="part-of-pfi"]');
    }

    static getPFIDetails() {
        cy.checkPath(this.path);
        return cy.get('[id="pfi-scheme-details"]');
    }

    static changePFI(pfi, description = '') {
        cy.checkPath(this.path);
        cy.get('[data-test="change-part-of-pfi"]').click();
        if (pfi) {
            cy.YesRadioBtn().check();
            cy.get('[id="PfiSchemeDetails"]').clear().type(description);
        }
        else {
            cy.NoRadioBtn().check();
        }
        cy.saveContinue().click();
    }

    static getDistance() {
        cy.checkPath(this.path);
        return cy.get('[id="distance-to-trust-headquarters"]');
    }

    static getDistanceDetails() {
        cy.checkPath(this.path);
        return cy.get('[id="distance-to-trust-headquarters-additional-text"]');
    }

    static changeDistance(distance, description = '') {
        cy.checkPath(this.path);
        cy.get('[data-test="change-distance-to-trust-headquarters"]').click();
        cy.get('[id="distance-to-trust-headquarters"]').clear().type(distance);
        cy.get('[id="distance-to-trust-headquarters-additional-information"]').clear().type(description);
    }
};
