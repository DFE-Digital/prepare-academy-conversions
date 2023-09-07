/// <reference types ='Cypress'/>

export const selectors = {
    assignInput: '[id="delivery-officer"]',
    unassignLink: '[id="unassign-link"]',
    saveButton: '[class="govuk-button"]'
};

export const path = 'project-assignment';

class ProjectAssignment {

    assignProject(deliveryOfficer) {
        cy.checkPath(path);
        cy.get(selectors.assignInput).click()
        cy.get(selectors.assignInput).type(`${deliveryOfficer}{enter}`);
        cy.get(selectors.saveButton).click();
    };

    unassignProject() {
        cy.checkPath(path)
        cy.get(selectors.unassignLink).click()
    };

};

export default new ProjectAssignment();
