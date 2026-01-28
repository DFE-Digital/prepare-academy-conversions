/// <reference types="cypress" />
import BasePage from './basePage';

class RationaleConversion extends BasePage {
    public path = 'confirm-project-trust-rationale';

    private selectors = {
        rationaleLink: '[data-test="change-rationale-for-trust"]',
        rationaleInput: '[id="trust-rationale"]',
        rationaleValue: '[id="rationale-for-trust"]',
        saveButton: '[data-cy="select-common-submitbutton"]',
        completeCheckbox: 'rationale-complete',
    };

    public changeRationale(rationale: string): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.rationaleLink).click();
        cy.get(this.selectors.rationaleInput).clear();
        cy.get(this.selectors.rationaleInput).type(rationale);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getRationale(): Cypress.Chainable<JQuery<HTMLElement>> {
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

const rationaleConversion = new RationaleConversion();

export default rationaleConversion;
