/// <reference types="cypress" />

import BasePage from './basePage';

export default class SchoolOverview extends BasePage {
    static selectors = {
        // IDs (without #)
        panId: 'published-admission-number',
        viabilityIssuesId: 'viability-issues',
        financialDeficitId: 'financial-deficit',
        pfiId: 'part-of-pfi',
        pfiDetailsId: 'pfi-scheme-details',
        pfiDetailsInputId: 'PfiSchemeDetails',
        distanceId: 'distance-to-trust-headquarters',
        distanceDetailsId: 'distance-to-trust-headquarters-additional-text',
        distanceDetailsInputId: 'distance-to-trust-headquarters-additional-information',
        mpId: 'member-of-parliament-name-and-party',
        // data-test values
        panLink: 'change-published-admission-number',
        viabilityIssuesLink: 'change-viability-issues',
        pfiLink: 'change-part-of-pfi',
        distanceLink: 'change-distance-to-trust-headquarters',
        mpLink: 'change-member-of-parliament-name-and-party',
        // Other selectors
        completeCheckbox: '[name="school-overview-complete"]',
    };

    static path = 'school-overview';

    static getPan(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.panId);
    }

    static changePan(newPanNumber: string): void {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.panLink).click();
        cy.getById(this.selectors.panId).clear();
        cy.getById(this.selectors.panId).type(newPanNumber);
        cy.saveContinue().click();
    }

    static getViabilityIssues(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.viabilityIssuesId);
    }

    static changeViabilityIssues(viabilityIssues: boolean): void {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.viabilityIssuesLink).click();
        if (viabilityIssues) {
            cy.YesRadioBtn().check();
        } else {
            cy.NoRadioBtn().check();
        }
        cy.saveContinue().click();
    }

    static getFinancialDeficit(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.financialDeficitId);
    }

    static getPFI(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.pfiId);
    }

    static getPFIDetails(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.pfiDetailsId);
    }

    static changePFI(pfi: boolean, description = ''): void {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.pfiLink).click();
        if (pfi) {
            cy.YesRadioBtn().check();
            cy.getById(this.selectors.pfiDetailsInputId).clear();
            cy.getById(this.selectors.pfiDetailsInputId).type(description);
        } else {
            cy.NoRadioBtn().check();
        }
        cy.saveContinue().click();
    }

    static getDistance(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.distanceId);
    }

    static getDistanceDetails(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.distanceDetailsId);
    }

    static changeDistance(distance: string, description = ''): void {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.distanceLink).click();
        cy.getById(this.selectors.distanceId).clear();
        cy.getById(this.selectors.distanceId).type(distance);
        cy.getById(this.selectors.distanceDetailsInputId).clear();
        cy.getById(this.selectors.distanceDetailsInputId).type(description);
        cy.saveContinue().click();
    }

    static getMP(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.mpId);
    }

    static changeMP(mp: string): void {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.mpLink).click();
        cy.getById(this.selectors.mpId).clear();
        cy.getById(this.selectors.mpId).type(mp);
        cy.saveContinue().click();
    }

    static markComplete(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.completeCheckbox).check();
    }

    static markIncomplete(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.completeCheckbox).uncheck();
    }
}
