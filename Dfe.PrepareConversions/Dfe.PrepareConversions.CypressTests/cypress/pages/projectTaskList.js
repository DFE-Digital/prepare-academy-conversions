/// <reference types ='Cypress'/>


import BasePage from "./BasePage"

export default class ProjectTaskList extends BasePage {

    static selectors = {
        assignProjectButton: 'a[href*="project-assignment"]',
        assignedUser: '[data-id="assigned-user"]',
        notificationMessage: '[id="notification-message"]',
        schoolOverviewLink: 'a[href*="/school-overview"]',
        schoolOverviewStatus: '[id="school-overview-status"]',
        budgetLink: 'a[href*="/budget"]',
        budgetStatus: '[id="school-budget-information-status"]',
        pupilForecastLink: 'a[href*="/pupil-forecast"]',
        conversionDetailsLink: 'a[href*="/conversion-details"]',
        conversionDetailsStatus: '[id="school-and-trust-information-status"]',
        rationaleLink: 'a[href*="/confirm-project-trust-rationale"]',
        rationaleStatus: '[id="rationale-status"]',
        riskAndIssuesLink: 'a[href*="/confirm-risks-issues"]',
        riskAndIssuesStatus: '[id="risks-and-issues-status"]',
        LALink: 'a[href*="/confirm-local-authority-information-template-dates"]',
        LAStatus: '[id="la-info-template-status"]',
        ofstedLink: 'a[href*="/school-performance-ofsted-information"]',
        keyStageLink: (keyStageNumber) => `a[href*="/key-stage-${keyStageNumber}-performance-tables"]`,
        createNewConversionButton: '[data-cy="create_new_conversion_btn"]',
        recordDecisionButton: '[data-cy="record_decision_btn"]',
        schoolName: '[data-cy="school-name"]',
        urn: '[data-cy="urn"]',
        groupsLink: '.dfe-header__navigation-link[href*="/groups/project-list"]', // Selector for Groups link in the navigation menu
        createNewGroupButton: 'a.govuk-button[href*="/groups/create-a-new-group"]', // Selector for Create New Group button
        createGroupButton: '[data-cy="create-group-btn"]', // Selector for Create Group button
        urnField: '[data-cy="UKPRN"]',
        continueButton: '[data-cy="submit-btn"]',
        conversionSelection: '[id="available-conversion-\\[0\\]"]',
        confirmandcontinue: '[data-cy="select-common-submitbutton"]'
       
       }

    static path = 'task-list'
    static path1 = 'groups/project-list'

    static selectAssignProject() {
        cy.checkPath(this.path)
        cy.get(this.selectors.assignProjectButton).click()
    }

    static getAssignedUser() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.assignedUser)
    }

    static getNotificationMessage() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.notificationMessage)
    }

    static selectSchoolOverview() {
        cy.checkPath(this.path)
        cy.get(this.selectors.schoolOverviewLink).click()
    }

    static getSchoolOverviewStatus() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.schoolOverviewStatus)
    }

    static selectBudget() {
        cy.checkPath(this.path)
        cy.get(this.selectors.budgetLink).click()
    }

    static getBudgetStatus() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.budgetStatus)
    }

    static selectPupilForecast() {
        cy.checkPath(this.path)
        cy.get(this.selectors.pupilForecastLink).click()
    }

    static selectConversionDetails() {
        cy.checkPath(this.path)
        cy.get(this.selectors.conversionDetailsLink).click()
    }

    static getHomePage() {
        cy.visit(`${Cypress.env('url')}`)
    }

    static getConversionDetailsStatus() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.conversionDetailsStatus)
    }

    static selectRationale() {
        cy.checkPath(this.path)
        cy.get(this.selectors.rationaleLink).click()
    }

    static getRationaleStatus() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.rationaleStatus)
    }

    static selectRisksAndIssues() {
        cy.checkPath(this.path)
        cy.get(this.selectors.riskAndIssuesLink).click()
    }

    static getRisksAndIssuesStatus() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.riskAndIssuesStatus)
    }

    static selectLA() {
        cy.checkPath(this.path)
        cy.get(this.selectors.LALink).click()
    }

    static getLAStatus() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.LAStatus)
    }

    static selectOfsted() {
        cy.checkPath(this.path)
        cy.get(this.selectors.ofstedLink).click()
    }

    static selectKeyStage(keyStageNumber) {
        cy.checkPath(this.path)
        cy.get(this.selectors.keyStageLink(keyStageNumber)).click()
    }

    static clickCreateNewConversionBtn() {
        cy.get(this.selectors.createNewConversionButton).click();
        return this;
    }

    static selectRecordDecision() {
        cy.get(this.selectors.recordDecisionButton).click();
        return this;
    }

    static verifyProjectDetails(urn, schoolName) {
        cy.get(this.selectors.schoolName).should('contain', schoolName);
        cy.get(this.selectors.urn).should('contain', urn);
        return this;
    }

    static clickGroupsLink() {
        cy.checkPath(this.path1);
        cy.get(this.selectors.groupsLink).click();
        return this;
    }

    static clickCreateNewGroupBtn() {
        cy.get(this.selectors.createNewGroupButton).click();
        return this;
    }

    static clickCreateGroupBtn() {
        cy.get(this.selectors.createGroupButton).click();
        return this;
    }

    static checkURNAndContinue(expectedURN) {
        cy.get(this.selectors.urnField).should('contain', expectedURN);
        cy.get(this.selectors.continueButton).click();
        return this;
    }

    static selectConversion() {
        cy.get(this.selectors.conversionSelection).click();
        cy.get(this.selectors.confirmandcontinue).click();
        return this;
    }

    static clickContinue() {
        cy.get(this.selectors.continueButton).click();
        return this;
    }

    static removeSchoolFromGroup() {
        cy.get('[data-cy="remove-link"] > .govuk-link').click();
        cy.get('#remove-conversion-confirmation').click();
        return this;
    }

    static deleteGroup() {
        cy.get('[data-cy="tes"]').click();
        cy.get('#delete-group-confirmation').click();
        return this;
    }
}
