/// <reference types="cypress" />

import BasePage from './basePage';

export default class projectAssignmentConversion extends BasePage {
    static selectors = {
        assignInput: '[id="delivery-officer"]',
        unassignLink: '[id="unassign-link"]',
        saveButton: '[class="govuk-button"]',
    };

    static path = 'project-assignment';

    static assignProject(deliveryOfficer: string): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.assignInput).click();
        cy.get(this.selectors.assignInput).type(`${deliveryOfficer}`);
        cy.get(this.selectors.saveButton).click();
    }

    static unassignProject(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.unassignLink).click();
    }
}
