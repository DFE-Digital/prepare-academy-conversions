/// <reference types ='Cypress'/>

export default class ProjectAssignment {
    static selectors = {
        assignInput: '[id="delivery-officer"]',
        unassignLink: '[id="unassign-link"]',
        saveButton: '[class="govuk-button"]'
    };

    static path = 'project-assignment';

    static assignProject(deliveryOfficer) {
        cy.checkPath(this.path);
        cy.get(this.selectors.assignInput).click()
        cy.get(this.selectors.assignInput).type(`${deliveryOfficer}{enter}`);
        cy.get(this.selectors.saveButton).click();
    };

    static unassignProject() {
        cy.checkPath(this.path)
        cy.get(this.selectors.unassignLink).click()
    };

};
