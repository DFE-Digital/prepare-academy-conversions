/// <reference types="cypress" />
import BasePage from './basePage';

// Shared behaviour for the conversions + transfers Financial Health Assessment page objects.
// Subclasses provide `path` and how the overview textarea is located (id vs data-test).
export default abstract class FinancialHealthAssessmentBasePage extends BasePage {
    protected readonly selectors = {
        heading: 'h1',
        noDecisionDateBanner: 'fha-no-decision-date', // data-test (Scenario 7 banner)
        notRequested: 'fha-not-requested', // data-test (Scenario 7 body)
        requestedDate: 'fha-requested-date', // data-test (Scenarios 5 / 6 / requested-in-past)
        overviewReadOnly: 'fha-overview-readonly', // data-test (Scenario 8 lockdown)
        saveButton: 'select-common-submitbutton', // data-cy
        errorSummary: '.govuk-error-summary',
    };

    // Conversions locates the overview by id; transfers by data-test (its asp-for id is PascalCase).
    protected abstract overviewField(): Cypress.Chainable<JQuery<HTMLElement>>;

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
        return cy.getByDataTest(this.selectors.noDecisionDateBanner);
    }

    public getReadOnlyOverview(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getByDataTest(this.selectors.overviewReadOnly);
    }

    public getOverview(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return this.overviewField();
    }

    public enterOverview(text: string): this {
        cy.checkPath(this.path);
        this.overviewField().clear();
        if (text.length > 0) {
            this.overviewField().type(text, { delay: 0 });
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
