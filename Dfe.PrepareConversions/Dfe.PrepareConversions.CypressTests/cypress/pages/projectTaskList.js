/// <reference types ='Cypress'/>

export default class ProjectTaskList {

    static selectors = {
        assignProjectButton: 'a[href*="project-assignment"]',
        assignedUser: '[data-id="assigned-user"]',
        notificationMessage: '[id="notification-message"]',
        schoolOverviewLink: 'a[href*="/school-overview"]',
        schoolOverviewStatus: '[id="school-overview-status"]'
    }

    static path = 'task-list';

    static selectAssignProject() {
        cy.checkPath(this.path);
        cy.get(this.selectors.assignProjectButton).click();
    };

    static getAssignedUser() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.assignedUser);
    }

    static getNotificationMessage() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.notificationMessage);
    }

    static selectSchoolOverview() {
        cy.checkPath(this.path);
        cy.get(this.selectors.schoolOverviewLink).click()
    }

    static getSchoolOverviewStatus() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.schoolOverviewStatus);
    }

};
