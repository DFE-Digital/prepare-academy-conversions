/// <reference types ='Cypress'/>

export const selectors = {
    assignProjectButton: 'a[href*="project-assignment"]',
    assignedUser: '[data-id="assigned-user"]',
    notificationMessage: '[id="notification-message"]'
}

export const path = 'task-list';

class ProjectTaskList {

    checkProjectPage() {
        cy.url().should('include', path);
    }

    selectAssignProject() {
        cy.get(selectors.assignProjectButton).click();
    };

    getAssignedUser() {
        return cy.get(selectors.assignedUser);
    }

    getNotificationMessage() {
        return cy.get(selectors.notificationMessage);
    }

};

export default new ProjectTaskList();
