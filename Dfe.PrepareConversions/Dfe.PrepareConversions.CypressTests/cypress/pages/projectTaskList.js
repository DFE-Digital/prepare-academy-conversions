/// <reference types ='Cypress'/>

export const selectors = {
    assignProjectButton: 'a[href*="project-assignment"]',
    assignedUser: '[data-id="assigned-user"]',
    notificationMessage: '[id="notification-message"]',
    schoolOverviewLink: 'a[href*="/school-overview"]'
}

export const path = 'task-list';

class ProjectTaskList {

    selectAssignProject() {
        cy.checkPath(path);
        cy.get(selectors.assignProjectButton).click();
    };

    getAssignedUser() {
        cy.checkPath(path);
        return cy.get(selectors.assignedUser);
    }

    getNotificationMessage() {
        cy.checkPath(path);
        return cy.get(selectors.notificationMessage);
    }

    selectSchoolOverview() {
        cy.checkPath(path);
        cy.get(selectors.schoolOverviewLink).click()
    }

};

export default new ProjectTaskList();
