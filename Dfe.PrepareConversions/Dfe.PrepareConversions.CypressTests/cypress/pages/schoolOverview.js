/// <reference types ='Cypress'/>

export default class SchoolOverview {

    static selectors = {
        panValue: '[id="published-admission-number"]',
        panLink: '[data-test="change-published-admission-number"]',
        panInput: '[id="published-admission-number"]',
        viabilityIssuesValue: '[id="viability-issues"]',
        viabilityIssuesLink: '[data-test="change-viability-issues"]',
        financialDeficitValue: '[id="financial-deficit"]',
        financialDeficitLink: '[data-test="change-financial-deficit"]',
        pfiValue: '[id="part-of-pfi"]',
        pifDetails: '[id="pfi-scheme-details"]',
        pfiLink: '[data-test="change-part-of-pfi"]',
        pfiDetailsInput: '[id="PfiSchemeDetails"]',
        distanceValue: '[id="distance-to-trust-headquarters"]',
        distanceDetails: '[id="distance-to-trust-headquarters-additional-text"]',
        distanceLink: '[data-test="change-distance-to-trust-headquarters"]',
        distanceInput: '[id="distance-to-trust-headquarters"]',
        distanceDetailsInput: '[id="distance-to-trust-headquarters-additional-information"]',
        mpValue: '[id="member-of-parliament-name-and-party"',
        mpLink: '[data-test="change-member-of-parliament-name-and-party"]',
        mpInput: '[id="member-of-parliament-name-and-party"]',
        completeCheckbox: '[name="school-overview-complete"]'
    }

    static path = 'school-overview';

    static getPan() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.panValue);
    }

    static changePan(newPanNumber) {
        cy.checkPath(this.path);
        cy.get(this.selectors.panLink).click();
        cy.get(this.selectors.panInput).clear().type(newPanNumber);
        cy.saveContinue().click();
    }

    static getViabilityIssues() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.viabilityIssuesValue);
    }

    static changeViabilityIssues(viabilityIssues) {
        cy.checkPath(this.path);
        cy.get(this.selectors.viabilityIssuesLink).click();
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
        return cy.get(this.selectors.financialDeficitValue);
    }

    static changeFinancialDeficit(financialDeficit) {
        cy.checkPath(this.path);
        cy.get(this.selectors.financialDeficitLink).click();
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
        return cy.get(this.selectors.pfiValue);
    }

    static getPFIDetails() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.pifDetails);
    }

    static changePFI(pfi, description = '') {
        cy.checkPath(this.path);
        cy.get(this.selectors.pfiLink).click();
        if (pfi) {
            cy.YesRadioBtn().check();
            cy.get(this.selectors.pfiDetailsInput).clear().type(description);
        }
        else {
            cy.NoRadioBtn().check();
        }
        cy.saveContinue().click();
    }

    static getDistance() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.distanceValue);
    }

    static getDistanceDetails() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.distanceDetails);
    }

    static changeDistance(distance, description = '') {
        cy.checkPath(this.path);
        cy.get(this.selectors.distanceLink).click();
        cy.get(this.selectors.distanceInput).clear().type(distance);
        cy.get(this.selectors.distanceDetailsInput).clear().type(description);
        cy.saveContinue().click()
    }

    static getMP() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.mpValue);
    }

    static changeMP(mp) {
        cy.checkPath(this.path);
        cy.get(this.selectors.mpLink).click();
        cy.get(this.selectors.mpInput).clear().type(mp);
        cy.saveContinue().click();
    }

    static markComplete() {
        cy.checkPath(this.path);
        cy.get(this.selectors.completeCheckbox).check();
    }

    static markIncomplete() {
        cy.checkPath(this.path);
        cy.get(this.selectors.completeCheckbox).uncheck();
    }
};
