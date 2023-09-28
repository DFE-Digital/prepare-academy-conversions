/// <reference types ='Cypress'/>

import BasePage from "./BasePage";

export default class Rationale extends BasePage {
    static selectors = {
        rationaleLink: '[data-test="change-rationale-for-trust"]',
        rationaleInput: '[id="trust-rationale"]',
        rationaleValue: '[id="rationale-for-trust"]',
        saveButton: '[class="govuk-button"]',
        completeCheckbox: '[id="rationale-complete"]'
    };

    static path = 'confirm-project-trust-rationale';

    static changeRationale(rationale) {
        cy.checkPath(this.path);
        cy.get(this.selectors.rationaleLink).click()
        cy.get(this.selectors.rationaleInput).clear().type(rationale);
        cy.get(this.selectors.saveButton).click();
    };

    static getRationale() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.rationaleValue);
    };

    static markComplete() {
        cy.checkPath(this.path);
        cy.get(this.selectors.completeCheckbox).check();
    }

    static markIncomplete() {
        cy.checkPath(this.path);
        cy.get(this.selectors.completeCheckbox).uncheck();
    }
};
