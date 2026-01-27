/// <reference types="cypress" />

import BasePage from './basePage';

export default class ProjectTaskList extends BasePage {
    static selectors = {
        assignProjectButton: 'a[href*="project-assignment"]',
        assignedUser: 'assigned-user',
        notificationMessage: 'notification-message',
        schoolOverviewLink: 'a[href*="/school-overview"]',
        schoolOverviewStatus: 'school-overview-status',
        budgetLink: 'a[href*="/budget"]',
        budgetStatus: 'school-budget-information-status',
        pupilForecastLink: 'a[href*="/pupil-forecast"]',
        conversionDetailsLink: 'a[href*="/conversion-details"]',
        conversionDetailsStatus: 'school-and-trust-information-status',
        publicSectorEqualityDutyLink: 'a[href*="/public-sector-equality-duty"]',
        publicSectorEqualityDutyStatus: 'select-tasklist-psed-status',
        rationaleLink: 'a[href*="/confirm-project-trust-rationale"]',
        rationaleStatus: 'rationale-status',
        riskAndIssuesLink: 'a[href*="/confirm-risks-issues"]',
        riskAndIssuesStatus: 'risks-and-issues-status',
        LALink: 'a[href*="/confirm-local-authority-information-template-dates"]',
        LAStatus: 'la-info-template-status',
        ofstedLink: 'a[href*="/school-performance-ofsted-information"]',
        keyStageLink: (keyStageNumber: number) => `a[href*="/key-stage-${keyStageNumber}-performance-tables"]`,
        createNewConversionButton: 'create_new_conversion_btn',
        recordDecisionButton: 'record_decision_error_btn',
        schoolName: 'school-name',
        urn: 'urn',
        urnId: '.govuk-caption-xl',
        acceptCookieBtn: 'cookie-banner-accept',
        hideAcceptCookieBtn: '#acceptCookieBanner > .govuk-button-group',
        previewProjectDocument: 'preview-project-template-button',
        createProjectDocument: 'generate-htb',
        errorMessage: 'error-message-0+=',
    };

    static path = 'task-list';

    static acceptCookieBtnClick(): void {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.acceptCookieBtn).click();
    }

    static hideAcceptCookieBanner(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.hideAcceptCookieBtn).click();
    }

    static selectAssignProject(): void {
        cy.checkPath(this.path);
        this.acceptCookieBtnClick();
        this.hideAcceptCookieBanner();
        cy.get(this.selectors.assignProjectButton).click();
    }

    static getAssignedUser(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getByDataId(this.selectors.assignedUser);
    }

    static getNotificationMessage(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.notificationMessage);
    }

    static selectSchoolOverview(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.schoolOverviewLink).click();
    }

    static getSchoolOverviewStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.schoolOverviewStatus);
    }

    static selectBudget(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.budgetLink).click();
    }

    static getBudgetStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.budgetStatus);
    }

    static selectPupilForecast(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.pupilForecastLink).click();
    }

    static selectConversionDetails(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.conversionDetailsLink).click();
    }

    static getConversionDetailsStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.conversionDetailsStatus);
    }

    static selectPublicSectorEqualityDuty(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.publicSectorEqualityDutyLink).click();
    }

    static publicSectorEqualityDutyStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getByDataCy(this.selectors.publicSectorEqualityDutyStatus);
    }

    static selectRationale(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.rationaleLink).click();
    }

    static getRationaleStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.rationaleStatus);
    }

    static selectRisksAndIssues(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.riskAndIssuesLink).click();
    }

    static getRisksAndIssuesStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.riskAndIssuesStatus);
    }

    static selectLA(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.LALink).click();
    }

    static getLAStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.LAStatus);
    }

    static selectOfsted(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.ofstedLink).click();
    }

    static getProjectUrn(): Cypress.Chainable<string> {
        cy.checkPath(this.path);
        return cy
            .get(this.selectors.urnId)
            .invoke('text')
            .then((text) => {
                return text.replace('URN: ', '').trim();
            });
    }

    static clickOfStedINfoBackButton(): void {
        cy.getByDataCy('ofsted-info-back-btn').click();
    }

    static selectKeyStage(keyStageNumber: number): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.keyStageLink(keyStageNumber)).click();
    }

    static clickCreateNewConversionBtn(): typeof ProjectTaskList {
        cy.getByDataCy(this.selectors.createNewConversionButton).click();
        return this;
    }

    static selectRecordDecision(): typeof ProjectTaskList {
        cy.getByDataCy(this.selectors.recordDecisionButton).click();
        return this;
    }

    static verifyProjectDetails(urn: string, schoolName: string): typeof ProjectTaskList {
        cy.getByDataCy(this.selectors.schoolName).should('contain', schoolName);
        cy.getByDataCy(this.selectors.urn).should('contain', urn);
        return this;
    }

    static clickPreviewProjectDocumentButton(): void {
        cy.checkPath(this.path);
        cy.getById(this.selectors.previewProjectDocument).click();
    }

    static clickCreateProjectDocumentButton(): void {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.createProjectDocument).click();
    }

    static getErrorMessage(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.getByDataCy(this.selectors.errorMessage);
    }
}
