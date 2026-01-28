/// <reference types="cypress" />
import BasePage from './basePage';

class ProjectAssignmentConversion extends BasePage {
    public path = 'project-assignment';

    private readonly selectors = {
        assignInput: '[id="delivery-officer"]',
        unassignLink: '[id="unassign-link"]',
        saveButton: '[class="govuk-button"]',
    };

    public assignProject(deliveryOfficer: string): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.assignInput).click();
        cy.get(this.selectors.assignInput).type(`${deliveryOfficer}`);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public unassignProject(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.unassignLink).click();
        return this;
    }
}

const projectAssignmentConversion = new ProjectAssignmentConversion();

export default projectAssignmentConversion;
