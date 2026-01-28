/// <reference types="cypress" />
import BasePage from './basePage';

class RisksAndIssues extends BasePage {
    public path = 'confirm-risks-issues';

    private selectors = {
        rationaleLink: '[data-test="change-risks-and-issues"]',
        rationaleInput: '[id="risks-and-issues"]',
        rationaleValue: '[id="risks-and-issues"]',
        saveButton: '[data-cy="select-common-submitbutton"]',
        completeCheckbox: 'risks-and-issues-complete',
    };

    public changeRisksAndIssues(rationale: string): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.rationaleLink).click();
        cy.get(this.selectors.rationaleInput).clear();
        cy.get(this.selectors.rationaleInput).type(rationale);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getRisksAndIssues(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.rationaleValue);
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

const risksAndIssues = new RisksAndIssues();

export default risksAndIssues;
