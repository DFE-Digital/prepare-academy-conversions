/// <reference types="cypress" />
import BasePage from './basePage';

class RequestFinancialHealthAssessment extends BasePage {
    public path = 'request-financial-health-assessment';

    private readonly selectors = {
        heading: 'h1',
        noDecisionDateInset: 'fha-no-decision-date', // data-test (Scenario 7 banner)
        notRequested: 'fha-not-requested', // data-test (Scenario 7 body)
        requestedDate: 'fha-requested-date', // data-test (Scenarios 5 / 6 / requested-in-past)
        overviewReadOnly: 'fha-overview-readonly', // data-test (Scenario 8 lockdown)
        overviewInput: 'sfso-commissioning-overview', // id
        saveButton: 'select-common-submitbutton', // data-cy
        errorSummary: '.govuk-error-summary',
    };

    public verifyOnPage(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.heading).should('contain.text', 'Financial Health Assessment');
        return this;
    }

    public getRequestedDate(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getByDataTest(this.selectors.requestedDate);
    }

    public getNoDecisionDateBanner(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getByDataTest(this.selectors.noDecisionDateInset);
    }

    public getReadOnlyOverview(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getByDataTest(this.selectors.overviewReadOnly);
    }

    public getOverview(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.overviewInput);
    }

    public enterOverview(text: string): this {
        cy.checkPath(this.path);
        cy.getById(this.selectors.overviewInput).clear();
        if (text.length > 0) {
            cy.getById(this.selectors.overviewInput).type(text, { delay: 0 });
        }
        return this;
    }

    public saveAndReturn(): this {
        cy.checkPath(this.path);
        cy.getByDataCy(this.selectors.saveButton).click();
        return this;
    }

    public getErrorSummary(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.get(this.selectors.errorSummary);
    }
}

const requestFinancialHealthAssessment = new RequestFinancialHealthAssessment();
export default requestFinancialHealthAssessment;
