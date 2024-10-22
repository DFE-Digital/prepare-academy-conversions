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
        recordDecisionButton: '[data-cy="record_decision_error_btn"]',
        schoolName: '[data-cy="school-name"]',
        urn: '[data-cy="urn"]',
        urnId: '.govuk-caption-xl',
        acceptCookieBtn:'[data-test="cookie-banner-accept"]',
        hideAcceptCookieBtn: '#acceptCookieBanner > .govuk-button-group'
    }

    static path = 'task-list'

    static acceptCookieBtnClick() {
        cy.checkPath(this.path)
        cy.get(this.selectors.acceptCookieBtn).click()
    }
    static hideAcceptCookieBanner() {
        cy.checkPath(this.path)
        cy.get(this.selectors.hideAcceptCookieBtn).click()
    }
    static selectAssignProject() {
        cy.checkPath(this.path)
        this.acceptCookieBtnClick();
        this.hideAcceptCookieBanner();
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

    static getProjectUrn() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.urnId)
        .invoke('text')
        .then((text) => {
            return text.replace('URN: ', '').trim();
        });
    }

    static clickOfStedINfoBackButton(){
        cy.get('[data-cy="ofsted-info-back-btn"]').click();
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
}


