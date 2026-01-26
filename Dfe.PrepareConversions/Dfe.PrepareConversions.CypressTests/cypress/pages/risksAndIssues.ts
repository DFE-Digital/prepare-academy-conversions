/// <reference types="cypress" />

import BasePage from './basePage';

export default class RisksAndIssues extends BasePage {
    static selectors = {
        rationaleLink: '[data-test="change-risks-and-issues"]',
        rationaleInput: '[id="risks-and-issues"]',
        rationaleValue: '[id="risks-and-issues"]',
        saveButton: '[data-cy="select-common-submitbutton"]',
        completeCheckbox: '[id="risks-and-issues-complete"]',
    };

    static path = 'confirm-risks-issues';

    static changeRisksAndIssues(rationale: string): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.rationaleLink).click();
        cy.get(this.selectors.rationaleInput).clear();
        cy.get(this.selectors.rationaleInput).type(rationale);
        cy.get(this.selectors.saveButton).click();
    }

    static getRisksAndIssues(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.rationaleValue);
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
