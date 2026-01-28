/// <reference types="cypress" />
import BasePage from './basePage';

class SchoolOverview extends BasePage {
    public path = 'school-overview';

    private selectors = {
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
        completeCheckbox: 'school-overview-complete',
    };

    public getPan(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.panId);
    }

    public changePan(newPanNumber: string): this {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.panLink).click();
        cy.getById(this.selectors.panId).clear();
        cy.getById(this.selectors.panId).type(newPanNumber);
        cy.saveContinue().click();
        return this;
    }

    public getViabilityIssues(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.viabilityIssuesId);
    }

    public changeViabilityIssues(viabilityIssues: boolean): this {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.viabilityIssuesLink).click();
        if (viabilityIssues) {
            cy.YesRadioBtn().check();
        } else {
            cy.NoRadioBtn().check();
        }
        cy.saveContinue().click();
        return this;
    }

    public getFinancialDeficit(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.financialDeficitId);
    }

    public getPFI(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.pfiId);
    }

    public getPFIDetails(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.pfiDetailsId);
    }

    public changePFI(pfi: boolean, description = ''): this {
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
        return this;
    }

    public getDistance(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.distanceId);
    }

    public getDistanceDetails(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.distanceDetailsId);
    }

    public changeDistance(distance: string, description = ''): this {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.distanceLink).click();
        cy.getById(this.selectors.distanceId).clear();
        cy.getById(this.selectors.distanceId).type(distance);
        cy.getById(this.selectors.distanceDetailsInputId).clear();
        cy.getById(this.selectors.distanceDetailsInputId).type(description);
        cy.saveContinue().click();
        return this;
    }

    public getMP(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.mpId);
    }

    public changeMP(mp: string): this {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.mpLink).click();
        cy.getById(this.selectors.mpId).clear();
        cy.getById(this.selectors.mpId).type(mp);
        cy.saveContinue().click();
        return this;
    }

    public markComplete(): this {
        cy.checkPath(this.path);
        cy.get(`#${this.selectors.completeCheckbox}`).check();
        return this;
    }

    public markIncomplete(): this {
        cy.checkPath(this.path);
        cy.get(`#${this.selectors.completeCheckbox}`).uncheck();
        return this;
    }
}

const schoolOverview = new SchoolOverview();

export default schoolOverview;
